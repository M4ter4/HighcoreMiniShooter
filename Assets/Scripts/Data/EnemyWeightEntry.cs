using System;
using System.Collections.Generic;
using Core.Spawning;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class EnemyWeightEntry : IWeighted
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float weight = 1f;
        [SerializeField] private WeaponWeightEntry[] weaponPool;

        public GameObject EnemyPrefab => enemyPrefab;
        public float Weight => weight;
        public IReadOnlyList<WeaponWeightEntry> WeaponPool => weaponPool;
    }
}