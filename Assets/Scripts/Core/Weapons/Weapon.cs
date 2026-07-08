using System;
using Core.Weapons.FirePatterns;
using Data;
using UnityEngine;

namespace Core.Weapons
{
    public class Weapon : IWeapon
    {
        private readonly IFirePattern _firePattern;
        private float _cooldownTimer;
        private float _reloadTimer;

        public WeaponData Data { get; }
        public int CurrentAmmo { get; private set; }
        public bool IsReloading { get; private set; }
        public bool CanFire => !IsReloading && _cooldownTimer <= 0f && CurrentAmmo > 0;

        public event Action OnFired;
        public event Action OnReloadStarted;
        public event Action OnReloadFinished;

        public Weapon(WeaponData data, IFirePattern firePattern)
        {
            Data = data;
            _firePattern = firePattern;
            CurrentAmmo = data.MagazineSize;
        }

        public void Fire(Vector3 origin, Vector3 direction, Action<Vector3, Vector3> spawnProjectile)
        {
            if (!CanFire)
                return;

            CurrentAmmo--;
            _cooldownTimer = 1f / Mathf.Max(0.01f, Data.FireRate);

            var context = new FireContext(origin, direction, Data, spawnProjectile);
            _firePattern.Fire(context);

            OnFired?.Invoke();

            if (CurrentAmmo <= 0)
                Reload();
        }

        public void Reload()
        {
            if (IsReloading || CurrentAmmo == Data.MagazineSize)
                return;

            IsReloading = true;
            _reloadTimer = Data.ReloadTime;
            OnReloadStarted?.Invoke();
        }

        public void Tick(float deltaTime)
        {
            if (_cooldownTimer > 0f)
                _cooldownTimer -= deltaTime;

            if (!IsReloading)
                return;

            _reloadTimer -= deltaTime;
            if (_reloadTimer > 0f)
                return;

            CurrentAmmo = Data.MagazineSize;
            IsReloading = false;
            OnReloadFinished?.Invoke();
        }

        public void ResetState()
        {
            IsReloading = false;
            CurrentAmmo = Data.MagazineSize;
            
            _cooldownTimer = 0f;
            _reloadTimer = 0f;
        }
    }
}