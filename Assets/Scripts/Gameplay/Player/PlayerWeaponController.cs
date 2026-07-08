using System;
using Core.ObjectManagement;
using Core.Weapons;
using Data;
using Gameplay.Input;
using Gameplay.ObjectPooling;
using UnityEngine;
using VContainer;

namespace Gameplay.Player
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private WeaponData[] weaponLoadout;

        private IInputProvider _input;
        private IProjectilePool _projectilePool;
        private IWeaponFactory _weaponFactory;

        private IWeapon[] _weapons;
        private int _currentIndex;

        public event Action<IWeapon> OnWeaponChanged;

        public IWeapon CurrentWeapon => _weapons[_currentIndex];
        
        public int WeaponCount => weaponLoadout.Length;

        [Inject]
        public void Construct(IInputProvider input, IProjectilePool projectilePool, IWeaponFactory weaponFactory)
        {
            _input = input;
            _projectilePool = projectilePool;
            _weaponFactory = weaponFactory;
        }

        public void ResetWeapons()
        {
            foreach (var weapon in _weapons)
            {
                weapon.ResetState();
            }
        }

        private void Awake()
        {
            _weapons = new IWeapon[weaponLoadout.Length];
            for (int i = 0; i < weaponLoadout.Length; i++)
                _weapons[i] = _weaponFactory.Create(weaponLoadout[i]);

            _currentIndex = 0;
        }

        private void Update()
        {
            if (_input.WeaponSwitchIndex >= 0 && _input.WeaponSwitchIndex < _weapons.Length
                                              && _input.WeaponSwitchIndex != _currentIndex)
            {
                _currentIndex = _input.WeaponSwitchIndex;
                OnWeaponChanged?.Invoke(CurrentWeapon);
            }

            var weapon = _weapons[_currentIndex];
            weapon.Tick(Time.deltaTime);

            if (_input.ReloadPressed)
                weapon.Reload();

            bool wantsToFire = weapon.Data.FireMode == WeaponType.Auto ? _input.FireHeld : _input.FirePressed;

            if (wantsToFire)
                weapon.Fire(firePoint.position, firePoint.right, SpawnProjectile);
        }

        private void SpawnProjectile(Vector3 position, Vector3 direction)
        {
            var weapon = _weapons[_currentIndex];
            direction.z = 0f;
            direction = direction.normalized;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(0f, 0f, angle);

            var projectile = _projectilePool.Get(weapon.Data.ProjectilePrefab, position, rotation);
            projectile.Launch(_projectilePool, weapon.Data, direction, this);
        }
    }
}