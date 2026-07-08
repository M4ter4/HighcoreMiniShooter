using Core.Damage;
using Data;
using UnityEngine;

namespace Core.StatusEffects
{
    public class ElectricDotEffect : IStatusEffect
    {
        private readonly StatusEffectData _data;
        private float _elapsed;
        private float _sinceLastTick;

        public bool IsFinished => _elapsed >= _data.Duration;
        
        public Sprite Icon => _data.Icon;

        public ElectricDotEffect(StatusEffectData data)
        {
            _data = data;
        }

        public void Tick(float deltaTime, IDamageable target)
        {
            if (IsFinished)
                return;

            _elapsed += deltaTime;
            _sinceLastTick += deltaTime;

            if (_sinceLastTick < _data.TickInterval)
                return;

            _sinceLastTick -= _data.TickInterval;
            target.TakeDamage(new DamageInfo(_data.TickDamage, this));
        }
    }
}