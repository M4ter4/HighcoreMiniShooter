using Core.StatusEffects;
using UnityEngine;

namespace Gameplay.Combat
{
    public class StatusEffectIconView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private StatusEffectRunner statusEffectRunner;
        [SerializeField] private SpriteRenderer iconSpriteRenderer;

        public void Start()
        {
            if (statusEffectRunner == null)
                statusEffectRunner = GetComponentInParent<StatusEffectRunner>();

            iconSpriteRenderer.enabled = false;

            if (statusEffectRunner != null)
            {
                statusEffectRunner.OnEffectApplied += HandleEffectApplied;
                statusEffectRunner.OnEffectRemoved += HandleEffectRemoved;
            }
        }

        private void OnDestroy()
        {
            if (statusEffectRunner != null)
            {
                statusEffectRunner.OnEffectApplied -= HandleEffectApplied;
                statusEffectRunner.OnEffectRemoved -= HandleEffectRemoved;
            }
        }

        private void HandleEffectApplied(IStatusEffect effect)
        {
            iconSpriteRenderer.sprite = effect.Icon;
            iconSpriteRenderer.enabled = true;
        }

        private void HandleEffectRemoved(IStatusEffect effect)
        {
            iconSpriteRenderer.enabled = false;
        }
    }
}