using System.Collections;
using StatusEffects;
using UnityEngine;

namespace Entities
{
    public class CombatantController : MonoBehaviour
    {
        [Header("Movement Config")]
        [SerializeField] protected float attackMoveDuration = 0.5f;

        [Header("Heal Visual Config")]
        [SerializeField] private GameObject healVisualPrefab;
        [SerializeField] private float healVisualScale = 0.6f;
        
        [Header("Spell Casting Visual Config")]
        [SerializeField] private GameObject boltSpellVisualPrefab;
        [SerializeField] private GameObject waveSpellVisualPrefab;
        

        protected StatusEffectController StatusEffectController;
        protected HealthController HealthController;
        
        protected Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Vector3 _originalPosition;

        private static readonly int MoveAnimEvent = Animator.StringToHash("isMoving");
        private static readonly int HitAnimParam = Animator.StringToHash("TakeHit");
        private static readonly int DeathAnimParam = Animator.StringToHash("Death");
        private static readonly int Attack1AnimEvent = Animator.StringToHash("Attack1");
        private static readonly int StunnedAnimEvent = Animator.StringToHash("isStunned");

        protected virtual void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            if (!_animator) Debug.LogError($"Animator not found on {gameObject.name}.");
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (!_spriteRenderer) Debug.LogError($"SpriteRenderer not found on {gameObject.name}.");
            StatusEffectController = GetComponent<StatusEffectController>();
            if (!StatusEffectController) Debug.LogError($"StatusEffectController not found on {gameObject.name}.");
            HealthController = GetComponent<HealthController>();
            if (!HealthController) Debug.LogError($"HealthController not found on {gameObject.name}.");
            
            StatusEffectController.OnDeathFromStatusEffect += HandleDeathFromStatusEffect;
        }

        protected virtual void Start()
        {
            _originalPosition = transform.position;
        }

        protected virtual void Update()
        {
            _animator.SetBool(StunnedAnimEvent, StatusEffectController.IsStunned);
            _spriteRenderer.color = GetStatusColor();
        }
        
        protected virtual void OnDestroy()
        {
            StatusEffectController.OnDeathFromStatusEffect -= HandleDeathFromStatusEffect;
        }
        
        public void TriggerDeathAnim() => DeathAnimEvent();
        
        protected void Heal(int amount)
        {
            HealthController.HealDamage(amount);
            CastHealVisualEffect();
        }

        private void CastHealVisualEffect()
        {
            if (!healVisualPrefab) return;
            GameObject healVisual = Instantiate(healVisualPrefab, transform.position, Quaternion.identity, transform);
            healVisual.transform.localScale = Vector3.one * healVisualScale;
        }

        protected IEnumerator AttackMoveCo(Vector3 attackMovement, System.Action onHit)
        {
            Vector3 targetPosition = _originalPosition + attackMovement;
            float elapsedTime = 0f;

            _animator.SetBool(MoveAnimEvent, true);
            while (elapsedTime < attackMoveDuration)
            {
                transform.position = Vector3.Lerp(_originalPosition, targetPosition, elapsedTime / attackMoveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _animator.SetBool(MoveAnimEvent, false);
            _animator.SetTrigger(Attack1AnimEvent);
            onHit?.Invoke();
            yield return new WaitForSeconds(1f);

            elapsedTime = 0f;
            _animator.SetBool(MoveAnimEvent, true);
            transform.localScale = new Vector3(-1, 1, 1);
            while (elapsedTime < attackMoveDuration)
            {
                transform.position = Vector3.Lerp(targetPosition, _originalPosition, elapsedTime / attackMoveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _animator.SetBool(MoveAnimEvent, false);
            transform.localScale = Vector3.one;
        }

        protected virtual void HandleDeathFromStatusEffect() => DeathAnimEvent();
        protected void TakeHitAnimEvent() => _animator.SetTrigger(HitAnimParam);
        protected void DeathAnimEvent() => _animator.SetTrigger(DeathAnimParam);
        
        private Color GetStatusColor()
        {
            if (StatusEffectController.IsBurned)    return new Color(1f, 0.3f, 0f);
            if (StatusEffectController.IsPoisoned)  return new Color(0f, 0.5f, 0f);
            return StatusEffectController.IsWeakened ? new Color(0.6f, 0.6f, 0.6f) : Color.white;
        }
    }
}