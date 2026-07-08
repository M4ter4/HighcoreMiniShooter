using System;
using System.Collections.Generic;
using Core.Damage;
using Core.StatusEffects;
using UnityEngine;

namespace Gameplay.Combat
{
    public class StatusEffectRunner : MonoBehaviour
    {
        private readonly List<IStatusEffect> _activeEffects = new();
        private IDamageable _damageable;

        public event Action<IStatusEffect> OnEffectApplied;
        public event Action<IStatusEffect> OnEffectRemoved;

        private void Awake()
        {
            _damageable = GetComponent<IDamageable>();
        }

        public void ApplyEffect(IStatusEffect effect)
        {
            _activeEffects.Add(effect);
            OnEffectApplied?.Invoke(effect);
        }

        private void Update()
        {
            for (int i = _activeEffects.Count - 1; i >= 0; i--)
            {
                var effect = _activeEffects[i];
                effect.Tick(Time.deltaTime, _damageable);

                if (effect.IsFinished)
                {
                    _activeEffects.RemoveAt(i);
                    OnEffectRemoved?.Invoke(effect);
                }
            }
        }
    }
}