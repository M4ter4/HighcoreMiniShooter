using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "StatusEffectData", menuName = "SOCreation/Weapons/Status Effect Data")]
    public class StatusEffectData : ScriptableObject
    {
        [SerializeField] private int tickDamage = 2;
        [SerializeField] private float tickInterval = 0.5f;
        [SerializeField] private float duration = 3f;
        [SerializeField] private Sprite icon;

        public int TickDamage => tickDamage;
        public float TickInterval => tickInterval;
        public float Duration => duration;
        public Sprite Icon => icon;
    }
}