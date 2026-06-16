using System.Collections;
using UnityEngine;

namespace Entities
{
    public class CombatantController : MonoBehaviour
    {
        [Header("Movement Config")]
        [SerializeField] protected float attackMoveDuration = 0.5f;

        protected Animator Animator;
        protected SpriteRenderer SpriteRenderer;
        private Vector3 _originalPosition;

        private static readonly int MoveAnimEvent = Animator.StringToHash("isMoving");
        private static readonly int HitAnimParam = Animator.StringToHash("TakeHit");
        private static readonly int DeathAnimParam = Animator.StringToHash("Death");
        private static readonly int Attack1AnimEvent = Animator.StringToHash("Attack1");
        protected static readonly int StunnedAnimEvent = Animator.StringToHash("isStunned");

        protected virtual void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
            if (!Animator) Debug.LogError($"Animator not found on {gameObject.name}.");
            SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (!SpriteRenderer) Debug.LogError($"SpriteRenderer not found on {gameObject.name}.");
        }

        protected virtual void Start()
        {
            _originalPosition = transform.position;
        }

        protected IEnumerator AttackMoveCo(Vector3 attackMovement, System.Action onHit)
        {
            Vector3 targetPosition = _originalPosition + attackMovement;
            float elapsedTime = 0f;

            Animator.SetBool(MoveAnimEvent, true);
            while (elapsedTime < attackMoveDuration)
            {
                transform.position = Vector3.Lerp(_originalPosition, targetPosition, elapsedTime / attackMoveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Animator.SetBool(MoveAnimEvent, false);
            Animator.SetTrigger(Attack1AnimEvent);
            onHit?.Invoke(); // fire damage at peak of attack
            yield return new WaitForSeconds(1f);

            elapsedTime = 0f;
            Animator.SetBool(MoveAnimEvent, true);
            transform.localScale = new Vector3(-1, 1, 1);
            while (elapsedTime < attackMoveDuration)
            {
                transform.position = Vector3.Lerp(targetPosition, _originalPosition, elapsedTime / attackMoveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            Animator.SetBool(MoveAnimEvent, false);
            transform.localScale = Vector3.one;
        }

        protected void TakeHitAnimEvent() => Animator.SetTrigger(HitAnimParam);
        protected void DeathAnimEvent() => Animator.SetTrigger(DeathAnimParam);
    }
}
