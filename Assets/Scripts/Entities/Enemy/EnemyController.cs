using System;
using System.Collections;
using Cards;
using Events;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Config")] 
    [SerializeField] private int attackDamage = 10;
    [Header("Debugging and Testing")]
    [SerializeField] private float takeHitAnimDelay = 0.25f;
    [SerializeField] private float deathAnimDelay = 0.25f;
    [SerializeField] private Vector3 attackMovement = new Vector3(2f, 0, 0);
    
    
    private Vector3 _originalPosition;
    private Animator _animator;
    private EnemyHealthController _enemyHealthController;
    
    private static readonly int MoveAnimEvent = Animator.StringToHash("isMoving");
    private static readonly int HitAnimParam = Animator.StringToHash("TakeHit");
    private static readonly int DeathAnimParam = Animator.StringToHash("Death");
    private static readonly int Attack1AnimEvent = Animator.StringToHash("Attack1");
    private static readonly int Attack2AnimEvent = Animator.StringToHash("Attack2");
    
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        if (!_animator) Debug.LogError("Animator not found in EnemyController.");
        _enemyHealthController = GetComponent<EnemyHealthController>();
        if (!_enemyHealthController) Debug.LogError("EnemyHealthController missing in EnemyController.");
    }

    private void Start()
    {
        _originalPosition = transform.position;
    }

    private void OnEnable()
    {
        EnemyEvents.OnEnemyHit += HandleEnemyHit;
        TurnEvents.OnEnemyTurnStart += Attack;
    }

    private void OnDisable()
    {
        EnemyEvents.OnEnemyHit -= HandleEnemyHit;
        TurnEvents.OnEnemyTurnStart -= Attack;
    }

    private void HandleEnemyHit(CardData cardData)
    {
        print("Enemy was hit!");
        _enemyHealthController.TakeDamage(cardData.attackPower);
        if (_enemyHealthController.IsAlive()) Invoke(nameof(TakeHitAnimEvent), takeHitAnimDelay);
        else
        {
            Invoke(nameof(DeathAnimEvent), deathAnimDelay);
            EnemyEvents.EnemyDeath();
        }
        
    }

    private void Attack()
    {
        print("Enemy is attacking!");
        StartCoroutine(EnemyAttackAnimEventCo());
    }

    private IEnumerator EnemyAttackAnimEventCo()
    {
        Vector3 targetPosition = _originalPosition + new Vector3(-2f, 0, 0);
        float duration = 0.5f;
        float elapsedTime = 0f;
        
        _animator.SetBool(MoveAnimEvent, true);
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(_originalPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _animator.SetBool(MoveAnimEvent, false);
        _animator.SetTrigger(Attack1AnimEvent);
        PlayerEvents.PlayerHit(attackDamage);
        yield return new WaitForSeconds(1f);
        
        elapsedTime = 0f;
        
        _animator.SetBool(MoveAnimEvent, true);
        transform.localScale = new Vector3(-1, 1, 1);
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(targetPosition, _originalPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        _animator.SetBool(MoveAnimEvent, false);
        transform.localScale = new Vector3(1, 1, 1);
        
        yield return null;
    }
    
    private void TakeHitAnimEvent() => _animator.SetTrigger(HitAnimParam);
    
    private void DeathAnimEvent() => _animator.SetTrigger(DeathAnimParam);
}
