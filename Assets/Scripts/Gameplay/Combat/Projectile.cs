using Core.Damage;
using Core.StatusEffects;
using Data;
using Gameplay.ObjectPooling;
using UnityEngine;

namespace Gameplay.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 5f;

        private IProjectilePool _pool;
        private WeaponData _weaponData;
        private Vector3 _direction;
        private float _speed;
        private float _range;
        private float _traveledDistance;
        private float _lifeTimer;
        private object _damageSource;

        public void Launch(IProjectilePool pool, WeaponData weaponData, Vector3 direction, object damageSource)
        {
            _pool = pool;
            _weaponData = weaponData;
            direction.z = 0f;
            _direction = direction.sqrMagnitude > 0f ? direction.normalized : Vector3.right;
            _speed = weaponData.ProjectileSpeed;
            _range = weaponData.Range;
            _damageSource = damageSource;

            _traveledDistance = 0f;
            _lifeTimer = lifeTime;
        }

        private void Update()
        {
            float step = _speed * Time.deltaTime;
            transform.position += _direction * step;
            _traveledDistance += step;

            _lifeTimer -= Time.deltaTime;

            if (_traveledDistance >= _range || _lifeTimer <= 0f)
                ReturnToPool();
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            var damageable = other.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                var damageInfo = new DamageInfo(_weaponData.Damage, _damageSource);
                damageable.TakeDamage(damageInfo);

                if (_weaponData.HasStatusEffect)
                {
                    var runner = other.GetComponentInParent<StatusEffectRunner>();
                    if (runner != null)
                        runner.ApplyEffect(new ElectricDotEffect(_weaponData.StatusEffectData));
                }
                
                ReturnToPool();
            }
        }

        private void ReturnToPool()
        {
            _pool.Release(this);
        }
    }
}