using System;

namespace Core.Health
{
    public class Health : IHealth
    {
        public int Current { get; private set; }
        public int Max { get; private set; }
        public bool IsDead { get; private set; }

        public event Action<int, int> OnDamaged;
        public event Action<int, int> OnHealed;
        public event Action OnDeath;
        public event Action OnRevived;

        public Health(int max)
        {
            Max = max;
            Current = max;
            IsDead = false;
        }

        public void ApplyDamage(int amount)
        {
            if (IsDead || amount <= 0)
                return;

            int clamped = Math.Min(amount, Current);
            Current -= clamped;

            OnDamaged?.Invoke(clamped, Current);

            if (Current <= 0)
            {
                Current = 0;
                IsDead = true;
                OnDeath?.Invoke();
            }
        }

        public void Heal(int amount)
        {
            if (IsDead || amount <= 0)
                return;

            int clamped = Math.Min(amount, Max - Current);
            if (clamped <= 0)
                return;

            Current += clamped;
            OnHealed?.Invoke(clamped, Current);
        }
        
        public void Revive()
        {
            Current = Max;
            IsDead = false;
            OnRevived?.Invoke();
        }
    }
}