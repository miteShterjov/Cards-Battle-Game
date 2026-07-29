using System;
using Cards;
using Entities;
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
            Debug.Log($"Spell hit: {other.gameObject.name} tag: {other.gameObject.tag}!");
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
            {
                Debug.Log($"Spell hit: {other.gameObject.name} tag: {other.gameObject.tag}!!");
                Instantiate(onHitEffectPrefab, transform.position, Quaternion.identity);

                int damage = spellCard ? spellCard.attackPower : _damage;
                other.GetComponent<HealthController>()?.TakeDamage(damage, DamageType.Magical);

                Destroy(gameObject, destroyDelay);
            }
        }
    }
}
