using System;
using Core.ObjectManagement;
using Core.Weapons;
using Data;
using Gameplay.Combat;
using Gameplay.ObjectPooling;
using UnityEngine;
using VContainer;

namespace Gameplay.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float followRadius = 3f;
        [SerializeField] private float shootRadius = 5f;

        private IWeaponFactory _weaponFactory;
        private IProjectilePool _projectilePool;
        private HealthComponent _health;

        private IWeapon _weapon;
        private Transform _target;

        public event Action OnDied;

        [Inject]
        public void Construct(IWeaponFactory weaponFactory, IProjectilePool projectilePool)
        {
            _weaponFactory = weaponFactory;
            _projectilePool = projectilePool;
        }

        private void Awake()
        {
            _health = GetComponent<HealthComponent>();

            if (_health != null)
                _health.OnDeath += HandleDeath;
        }

        private void OnValidate()
        {
            followRadius = Mathf.Max(0f, followRadius);
            shootRadius = Mathf.Max(followRadius, shootRadius);
        }

        public void SetWeapon(WeaponData weaponData)
        {
            _weapon = _weaponFactory.Create(weaponData);
        }

        public void SetTarget(Transform newTarget)
        {
            _target = newTarget;
        }

        private void Update()
        {
            if (_weapon == null || _target == null)
                return;

            _weapon.Tick(Time.deltaTime);

            Vector3 toTarget = _target.position - transform.position;
            toTarget.z = 0f;

            float distance = toTarget.magnitude;
            if (distance < 0.0001f)
                return;

            Vector3 direction = toTarget / distance;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);

            if (distance < followRadius)
                transform.position -= direction * (moveSpeed * Time.deltaTime);
            else if (distance > followRadius)
                transform.position += direction * (moveSpeed * Time.deltaTime);

            if (distance > shootRadius || !_weapon.CanFire)
                return;

            _weapon.Fire(firePoint.position, direction, SpawnProjectile);
        }

        private void SpawnProjectile(Vector3 position, Vector3 direction)
        {
            direction.z = 0f;
            direction = direction.normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(0f, 0f, angle);

            var projectile = _projectilePool.Get(_weapon.Data.ProjectilePrefab, position, rotation);
            projectile.Launch(_projectilePool, _weapon.Data, direction, this);
        }

        private void HandleDeath()
        {
            enabled = false;
            OnDied?.Invoke();
        }
    }
}