using System;
using Core.Spawning;
using Data;
using Gameplay.Enemy;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Random = UnityEngine.Random;

namespace Gameplay.Service
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private EnemySpawnConfig config;
        [SerializeField] private Transform target;
        [SerializeField] private Transform parent;

        private readonly WeightedRandomPicker<EnemyWeightEntry> _enemyPicker = new();
        private readonly WeightedRandomPicker<WeaponWeightEntry> _weaponPicker = new();

        private IObjectResolver _resolver;
        private ISpawnRateCurve _spawnRateCurve;
        private IEnemyManager _enemyManager;

        private float _elapsedTime;
        private float _timeSinceLastSpawn;

        public event Action<EnemyController> OnEnemySpawned;

        [Inject]
        public void Construct(IObjectResolver resolver, IEnemyManager enemyManager)
        {
            _resolver = resolver;
            _enemyManager = enemyManager;
        }
        public void ResetSpawnState()
        {
            _elapsedTime = 0f;
            _timeSinceLastSpawn = 0f;
        }
        
        private void Awake()
        {
            _spawnRateCurve = new SpawnRateCurve(
                config.BaseInterval,
                config.MinInterval,
                config.IntervalReductionPerSecond,
                config.IntervalReductionPerKill);
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            _timeSinceLastSpawn += Time.deltaTime;

            float interval = _spawnRateCurve.GetInterval(_elapsedTime, _enemyManager.KillCount);
            if (_timeSinceLastSpawn < interval)
                return;

            _timeSinceLastSpawn = 0f;
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            if (config.Enemies.Count == 0 || target == null)
                return;

            var enemyEntry = _enemyPicker.Pick(config.Enemies);
            var spawnPosition = GetRandomSpawnPosition();
            var spawnRotation = Quaternion.identity;

            var instance = _resolver.Instantiate(enemyEntry.EnemyPrefab, spawnPosition,
                spawnRotation, parent);
            var controller = instance.GetComponent<EnemyController>();

            if (controller == null)
                return;

            if (enemyEntry.WeaponPool.Count > 0)
            {
                var weaponEntry = _weaponPicker.Pick(enemyEntry.WeaponPool);
                controller.SetWeapon(weaponEntry.WeaponData);
            }

            controller.SetTarget(target);
            controller.OnDied += () => Destroy(controller.gameObject, 2f);

            OnEnemySpawned?.Invoke(controller);
        }

        private Vector3 GetRandomSpawnPosition()
        {
            float minRadius = config.MinSpawnRadius;
            float maxRadius = Mathf.Max(minRadius, config.SpawnRadius);
            float distance = Random.Range(minRadius, maxRadius);
            float angle = Random.Range(0f, Mathf.PI * 2f);

            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * distance;
            Vector3 spawnPosition = target.position + offset;
            spawnPosition.z = target.position.z;
            return spawnPosition;
        }
    }
}