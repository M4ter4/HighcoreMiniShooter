using System;
using Data;
using UnityEngine;

namespace Core.Weapons
{
    public interface IWeapon
    {
        public WeaponData Data { get; }
        public int CurrentAmmo { get; }
        public bool IsReloading { get; }
        public bool CanFire { get; }

        public event Action OnFired;
        public event Action OnReloadStarted;
        public event Action OnReloadFinished;

        public void Fire(Vector3 origin, Vector3 direction, Action<Vector3, Vector3> spawnProjectile);
        public void Reload();
        public void Tick(float deltaTime);
        public void ResetState();
    }
}