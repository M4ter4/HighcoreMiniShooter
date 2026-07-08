using Gameplay.Combat;
using UnityEngine;

namespace Gameplay.Enemy
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private HealthComponent healthComponent;
        [SerializeField] private SpriteRenderer fillSpriteRenderer;

        [Header("Settings")]
        [SerializeField] private bool hideWhenFull = true;

        private float _maxSpriteWidth;

        public void Start()
        {
            if (healthComponent == null)
                healthComponent = GetComponentInParent<HealthComponent>();

            if (fillSpriteRenderer != null)
                _maxSpriteWidth = fillSpriteRenderer.size.x;

            if (healthComponent != null && healthComponent.Health != null)
            {
                UpdateHealthBar(healthComponent.Health.Max, healthComponent.Health.Current);

                healthComponent.OnDamaged += UpdateHealthBar;
                healthComponent.OnHealed += UpdateHealthBar;
                healthComponent.OnDeath += HandleDeath;
            }
        }

        private void OnDestroy()
        {
            if (healthComponent != null)
            {
                healthComponent.OnDamaged -= UpdateHealthBar;
                healthComponent.OnHealed -= UpdateHealthBar;
                healthComponent.OnDeath -= HandleDeath;
            }
        }

        private void UpdateHealthBar(int amount, int currentHealth)
        {
            if (fillSpriteRenderer != null && healthComponent != null && healthComponent.Health != null)
            {
                float healthPercentage = (float)currentHealth / healthComponent.Health.Max;
                Vector2 newSize = fillSpriteRenderer.size;
                newSize.x = _maxSpriteWidth * healthPercentage;
                fillSpriteRenderer.size = newSize;
            }

            UpdateVisibility();
        }

        private void HandleDeath()
        {
            gameObject.SetActive(false);
        }

        private void UpdateVisibility()
        {
            if (!hideWhenFull || fillSpriteRenderer == null) return;

            bool isFullHealth = Mathf.Approximately(fillSpriteRenderer.size.x, _maxSpriteWidth);
            gameObject.SetActive(!isFullHealth);
        }
    }
}