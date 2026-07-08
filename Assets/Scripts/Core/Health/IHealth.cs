using System;

namespace Core.Health
{
    public interface IHealth
    {
        public int Current { get; }
        public int Max { get; }
        public bool IsDead { get; }

        public event Action<int, int> OnDamaged;
        public event Action<int, int> OnHealed;
        public event Action OnDeath;
        public event Action OnRevived;

        public void ApplyDamage(int amount);
        public void Heal(int amount);
        public void Revive();
    }
}