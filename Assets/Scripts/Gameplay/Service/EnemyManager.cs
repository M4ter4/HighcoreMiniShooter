using System;
using System.Collections.Generic;
using Gameplay.Enemy;

namespace Gameplay.Service
{
    public interface IEnemyManager
    {
        int KillCount { get; }
        event Action<int> OnKillCountChanged;
        void TrackSpawn(EnemyController controller);
        void ClearAll();
        void ResetKillCount();
    }

    public class EnemyManager : IEnemyManager
    {
        private readonly List<EnemyController> _activeEnemies = new();

        public int KillCount { get; private set; }
        public event Action<int> OnKillCountChanged;

        public void TrackSpawn(EnemyController controller)
        {
            _activeEnemies.Add(controller);
            controller.OnDied += HandleDied;

            void HandleDied()
            {
                controller.OnDied -= HandleDied;
                _activeEnemies.Remove(controller);
                KillCount++;
                OnKillCountChanged?.Invoke(KillCount);
            }
        }

        public void ClearAll()
        {
            foreach (var enemy in _activeEnemies.ToArray())
                if (enemy != null)
                    UnityEngine.Object.Destroy(enemy.gameObject);

            _activeEnemies.Clear();
        }

        public void ResetKillCount()
        {
            KillCount = 0;
            OnKillCountChanged?.Invoke(KillCount);
        }
    }
}