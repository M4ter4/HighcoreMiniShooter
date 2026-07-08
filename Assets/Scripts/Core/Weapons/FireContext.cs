using System;
using Data;
using UnityEngine;

namespace Core.Weapons
{
    public readonly struct FireContext
    {
        public readonly Vector3 Origin;
        public readonly Vector3 Direction;
        public readonly WeaponData Data;
        public readonly Action<Vector3, Vector3> SpawnProjectile;

        public FireContext(Vector3 origin, Vector3 direction, WeaponData data, Action<Vector3, Vector3> spawnProjectile)
        {
            Origin = origin;
            Direction = direction;
            Data = data;
            SpawnProjectile = spawnProjectile;
        }
    }
}