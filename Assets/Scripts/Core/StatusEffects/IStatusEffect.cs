using Core.Damage;
using UnityEngine;

namespace Core.StatusEffects
{
    public interface IStatusEffect
    {
        public bool IsFinished { get; }
        public Sprite Icon { get; }
        public void Tick(float deltaTime, IDamageable target);
    }
}