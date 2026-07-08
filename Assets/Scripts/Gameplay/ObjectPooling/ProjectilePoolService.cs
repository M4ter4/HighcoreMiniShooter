using System.Collections.Generic;
using System.Linq;
using Gameplay.Combat;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.ObjectPooling
{
    public class ProjectilePoolService : IProjectilePool
    {
        private readonly Dictionary<GameObject, ObjectPool<Projectile>> _pools = new();
        private readonly Dictionary<Projectile, GameObject> _instanceToPrefab = new();
        private readonly HashSet<Projectile> _activeProjectiles = new();
        private readonly Transform _poolRoot;

        public ProjectilePoolService(Transform poolRoot = null)
        {
            _poolRoot = poolRoot;
        }

        public Projectile Get(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var pool = GetOrCreatePool(prefab);
            var projectile = pool.Get();
            projectile.transform.SetPositionAndRotation(position, rotation);
            _activeProjectiles.Add(projectile);
            return projectile;
        }

        public void Release(Projectile projectile)
        {
            if (!_instanceToPrefab.TryGetValue(projectile, out var prefab))
                return;

            _activeProjectiles.Remove(projectile);
            _pools[prefab].Release(projectile);
        }
        
        public void ReleaseAll()
        {
            foreach (var projectile in _activeProjectiles.ToArray())
                Release(projectile);
        }

        private ObjectPool<Projectile> GetOrCreatePool(GameObject prefab)
        {
            if (_pools.TryGetValue(prefab, out var existingPool))
                return existingPool;

            var pool = new ObjectPool<Projectile>(
                createFunc: () => CreateInstance(prefab),
                actionOnGet: p => p.gameObject.SetActive(true),
                actionOnRelease: p => p.gameObject.SetActive(false),
                actionOnDestroy: p => Object.Destroy(p.gameObject),
                collectionCheck: false,
                defaultCapacity: 16
            );

            _pools[prefab] = pool;
            return pool;
        }

        private Projectile CreateInstance(GameObject prefab)
        {
            var instance = Object.Instantiate(prefab, _poolRoot);
            var projectile = instance.GetComponent<Projectile>();
            _instanceToPrefab[projectile] = prefab;
            return projectile;
        }
    }
}