using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class PlayerController : MonoBehaviour
{
    [Header("HealVisual Config")] 
    [SerializeField] private GameObject healVisualPrefab;
    [SerializeField] private float healVisualScale = 0.6f;
    
    private Vector3 _originalPosition;
    private Animator _animator;
    private PlayerHealthController _playerHealthController;
    
    private static readonly int MoveAnimEvent = Animator.StringToHash("isMoving");
    private static readonly int HitAnimParam = Animator.StringToHash("TakeHit");
    private static readonly int DeathAnimParam = Animator.StringToHash("Death");
    private static readonly int Attack1AnimEvent = Animator.StringToHash("Attack1");
    private static readonly int Attack2AnimEvent = Animator.StringToHash("Attack2");

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        if (!_animator) Debug.LogError("No Animator component found on PlayerController.");
        _playerHealthController = GetComponent<PlayerHealthController>();
        if (!_playerHealthController) Debug.LogError("No PlayerHealthController component found on PlayerController.");
    }

    private void Start()
    {
        _originalPosition = transform.position;
    }
    
    private void OnEnable()
    {
        PlayerEvents.OnCardPlayed += HandleCardPlayed;
        PlayerEvents.OnPlayerHit += PlayerHit;
    }
    
    private void OnDisable()
    {
        PlayerEvents.OnCardPlayed -= HandleCardPlayed;
        PlayerEvents.OnPlayerHit -= PlayerHit;
    }

    private void HandleCardPlayed(CardData cardData)
    {
        print("Card Played: " + cardData.cardName);
        if (cardData.attackPower > 0) Attack(cardData);
        if (cardData.healPower > 0) Heal(cardData);
    }

    private void Attack(CardData cardData)
    {
        print("Attacking, doing " + cardData.attackPower + " damage.");
        StartCoroutine(PlayerAttackAnimEventCo(cardData));
    }

    private void Heal(CardData cardData)
    {
        _playerHealthController.HealDamage(cardData.healPower);
        CastHealVisualEffect();
    }

    private IEnumerator PlayerAttackAnimEventCo(CardData cardData)
    {
        Vector3 targetPosition = _originalPosition + new Vector3(4f, 0, 0);
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
        EnemyEvents.EnemyHit(cardData);
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

    private IEnumerator DoAttackAnimationCo()
    {
        int randomIndex = UnityEngine.Random.Range(0, 2);
        _animator.SetTrigger(randomIndex == 0 ? Attack1AnimEvent : Attack2AnimEvent);
        yield return null;
    }

    private void PlayerHit(int damage)
    {
        _playerHealthController.TakeDamage(damage);
        if (_playerHealthController.IsAlive()) _animator.SetTrigger(HitAnimParam);
        else
        {
            _animator.SetTrigger(DeathAnimParam);
            PlayerEvents.PlayerDeath();
        }
    }

    private void CastHealVisualEffect()
    {
        GameObject healVisual = 
            Instantiate(
                healVisualPrefab, 
                transform.position, 
                Quaternion.identity,
                this.transform);
        
        healVisual.transform.localScale = new Vector3(healVisualScale, healVisualScale, healVisualScale);
    }
}
