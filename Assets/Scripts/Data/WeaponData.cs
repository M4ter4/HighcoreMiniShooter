using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "SOCreation/Weapons/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [Header("General")]
        [SerializeField] private string weaponName;
        [SerializeField] private WeaponType fireMode = WeaponType.Single;

        [Header("Stats")]
        [SerializeField] private int damage = 10;
        [SerializeField] private float fireRate = 1f;
        [SerializeField] private float range = 50f;
        [SerializeField] private float spread = 0f;
        [SerializeField] private float reloadTime = 1.5f;
        [SerializeField] private int magazineSize = 10;

        [Header("Shotgun-specific")]
        [SerializeField] private int pelletsPerShot = 1;

        [Header("Projectile")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 30f;

        [Header("Status Effect (optional)")]
        [SerializeField] private StatusEffectData statusEffectData;

        public string WeaponName => weaponName;
        public WeaponType FireMode => fireMode;

        public int Damage => damage;
        public float FireRate => fireRate;
        public float Range => range;
        public float Spread => spread;
        public float ReloadTime => reloadTime;
        public int MagazineSize => magazineSize;

        public int PelletsPerShot => pelletsPerShot;

        public GameObject ProjectilePrefab => projectilePrefab;
        public float ProjectileSpeed => projectileSpeed;

        public StatusEffectData StatusEffectData => statusEffectData;
        public bool HasStatusEffect => statusEffectData != null;
    }
}