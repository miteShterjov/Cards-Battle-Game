using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BaseClasses
{
    public class HealthController : MonoBehaviour
    {
        [Header("Health Config")]
        [SerializeField] private int maxHealth = 100;
        [Header("Health Visuals Config")]
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Slider healthSlider;
    
        [Header("Debugging and Testing")]
        [SerializeField] private int currentHealth;
    
        private void Start()
        {
            currentHealth = maxHealth;
            UpdateHealthUI();
        }
    
        public void HealDamage(int amount)
        {
            if (currentHealth < 0) return;
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            UpdateHealthUI();
        }

        public void TakeDamage(int amount)
        {
            if (currentHealth <= 0) return;
            currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
            UpdateHealthUI();
        }
    
        public bool IsAlive() => currentHealth > 0;

        private void UpdateHealthUI()
        {
            healthText.text = currentHealth + "/" + maxHealth;
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }
}
