using Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Entities
{
    public class HealthController : MonoBehaviour
    {
        [Header("Health Config")]
        [SerializeField] private int maxHealth = 100;

        [Header("Health Visuals Config")]
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Slider healthSlider;

        [Header("Armor Config")]
        [SerializeField] private TextMeshPro armorText;
        [SerializeField] private GameObject shieldVisual;

        [Header("Debugging and Testing")]
        [SerializeField] private int currentHealth;
        [SerializeField] private int currentArmor;

        private int ArmorCap => maxHealth / 3;

        private void Start()
        {
            currentHealth = maxHealth;
            currentArmor = 0;
            UpdateHealthUI();
            UpdateArmorUI();
        }
        
        public void SetMaxHealth(int value)
        {
            maxHealth = value;
            currentHealth = value;
            currentArmor = 0;
            UpdateHealthUI();
            UpdateArmorUI();
        }

        public void HealDamage(int amount)
        {
            if (currentHealth <= 0) return;
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            UpdateHealthUI();
        }

        public void TakeDamage(int amount, DamageType type = DamageType.Physical)
        {
            if (currentHealth <= 0) return;

            if (type is DamageType.Magical or DamageType.Pure)
            {
                currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
            }
            else
            {
                int damageToArmor = Mathf.Min(amount, currentArmor);
                currentArmor -= damageToArmor;
                int remainingDamage = amount - damageToArmor;
                currentHealth = Mathf.Clamp(currentHealth - remainingDamage, 0, maxHealth);
            }

            UpdateHealthUI();
            UpdateArmorUI();
        }

        public void AddArmor(int amount)
        {
            currentArmor = Mathf.Min(currentArmor + amount, ArmorCap);
            UpdateArmorUI();
        }

        public bool IsAlive() => currentHealth > 0;

        private void UpdateHealthUI()
        {
            healthText.text = currentHealth + "/" + maxHealth;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        private void UpdateArmorUI()
        {
            armorText.text = currentArmor.ToString();
            shieldVisual.SetActive(currentArmor > 0);
        }
    }
}