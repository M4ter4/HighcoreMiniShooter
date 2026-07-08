using Gameplay.Combat;
using UnityEngine;

namespace Gameplay.ObjectPooling
{
    public interface IProjectilePool
    {
        public Projectile Get(GameObject prefab, Vector3 position, Quaternion rotation);
        public void Release(Projectile projectile);
        public void ReleaseAll();
    }
}