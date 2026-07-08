using UnityEngine;

namespace Core.Spawning
{
    public class SpawnRateCurve : ISpawnRateCurve
    {
        private readonly float _baseInterval;
        private readonly float _minInterval;
        private readonly float _intervalReductionPerSecond;
        private readonly float _intervalReductionPerKill;

        public SpawnRateCurve(float baseInterval, float minInterval, float intervalReductionPerSecond, float intervalReductionPerKill)
        {
            _baseInterval = baseInterval;
            _minInterval = minInterval;
            _intervalReductionPerSecond = intervalReductionPerSecond;
            _intervalReductionPerKill = intervalReductionPerKill;
        }

        public float GetInterval(float elapsedTime, int killCount)
        {
            float reduction = elapsedTime * _intervalReductionPerSecond + killCount * _intervalReductionPerKill;
            float interval = _baseInterval - reduction;

            return Mathf.Max(_minInterval, interval);
        }
    }
}