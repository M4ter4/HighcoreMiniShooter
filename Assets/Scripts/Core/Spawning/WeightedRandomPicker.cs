using System.Collections.Generic;
using UnityEngine;

namespace Core.Spawning
{
    public class WeightedRandomPicker<T> where T : IWeighted
    {
        public T Pick(IReadOnlyList<T> items)
        {
            float totalWeight = 0f;
            for (int i = 0; i < items.Count; i++)
                totalWeight += Mathf.Max(0f, items[i].Weight);

            if (totalWeight <= 0f)
                return items[Random.Range(0, items.Count)];

            float roll = Random.Range(0f, totalWeight);
            float cumulative = 0f;

            for (int i = 0; i < items.Count; i++)
            {
                cumulative += Mathf.Max(0f, items[i].Weight);
                if (roll <= cumulative)
                    return items[i];
            }

            return items[items.Count - 1];
        }
    }
}