using System;
using Cards;
using Entities;
using Entities.Enemy;
using Entities.Player;
using Events;
using UnityEngine;

namespace Misc
{
    public class LockSpell : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private GameObject onHitEffectPrefab;
        [SerializeField] public string caster;
        [SerializeField] public CardData spellCard;
        [SerializeField] private float destroyDelay = 0.1f;
        [SerializeField] private float delay = 4f;
        
        private int _damage;

        private void Start()
        {
            Destroy(gameObject, delay);
        }

        private void Update()
        {
            SpellLocomotion();
        }
        
        public void SetDamage(int damage) => _damage = damage;
    
        private void SpellLocomotion()
        {
            switch (caster == "Player")
            {
                case true:
                    transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    transform.Translate(Vector3.right * (Time.deltaTime * moveSpeed));
                    break;
                case false:
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    transform.Translate(Vector3.left * (Time.deltaTime * moveSpeed));
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Enemy")) return;

            Instantiate(onHitEffectPrefab, transform.position, Quaternion.identity);

            int damage = spellCard ? spellCard.attackPower : _damage;
            HealthController healthController = other.GetComponent<HealthController>();
    
            if (healthController == null) return;
    
            healthController.TakeDamage(damage, DamageType.Magical);

            // fire the appropriate death event if target died
            if (!healthController.IsAlive())
            {
                if (other.gameObject.CompareTag("Enemy"))
                {
                    other.GetComponent<EnemyController>()?.TriggerDeathAnim();
                    EnemyEvents.EnemyDeath();
                }
                else if (other.gameObject.CompareTag("Player"))
                {
                    other.GetComponent<PlayerController>()?.TriggerDeathAnim();
                    PlayerEvents.PlayerDeath();
                }
            }

            Destroy(gameObject, destroyDelay);
        }
    }
}
