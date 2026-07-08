using System;
using Core.Damage;
using Core.Health;
using UnityEngine;

namespace Gameplay.Combat
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField] private int maxHealth = 100;

        private IHealth _health;

        public IHealth Health => _health;

        public event Action<int, int> OnDamaged;
        public event Action<int, int> OnHealed;
        public event Action OnDeath;
        public event Action OnRevived;

        private void Awake()
        {
            _health = new Health(maxHealth);
            _health.OnDamaged += (amount, current) => OnDamaged?.Invoke(amount, current);
            _health.OnHealed += (amount, current) => OnHealed?.Invoke(amount, current);
            _health.OnDeath += () => OnDeath?.Invoke();
            _health.OnRevived += () => OnRevived?.Invoke();
        }

        public void TakeDamage(DamageInfo damageInfo)
        {
            _health.ApplyDamage(damageInfo.Amount);
        }
    }
}