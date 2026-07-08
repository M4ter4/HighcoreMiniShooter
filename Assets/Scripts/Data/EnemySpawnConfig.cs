using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "EnemySpawnConfig", menuName = "SOCreation/Spawning/Enemy Spawn Config")]
    public class EnemySpawnConfig : ScriptableObject
    {
        [Header("Enemies")]
        [SerializeField] private EnemyWeightEntry[] enemies;

        [Header("Spawn Rate")]
        [SerializeField] private float baseInterval = 5f;
        [SerializeField] private float minInterval = 1f;
        [SerializeField] private float intervalReductionPerSecond = 0.02f;
        [SerializeField] private float intervalReductionPerKill = 0.1f;

        [Header("Spawn Area")]
        [SerializeField] private float minSpawnRadius = 8f;
        [SerializeField] private float spawnRadius = 15f;

        public IReadOnlyList<EnemyWeightEntry> Enemies => enemies;
        public float BaseInterval => baseInterval;
        public float MinInterval => minInterval;
        public float IntervalReductionPerSecond => intervalReductionPerSecond;
        public float IntervalReductionPerKill => intervalReductionPerKill;
        public float MinSpawnRadius => minSpawnRadius;
        public float SpawnRadius => spawnRadius;
    }
}