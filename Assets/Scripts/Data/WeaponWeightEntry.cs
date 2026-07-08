using System;
using Core.Spawning;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class WeaponWeightEntry : IWeighted
    {
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private float weight = 1f;

        public WeaponData WeaponData => weaponData;
        public float Weight => weight;
    }
}