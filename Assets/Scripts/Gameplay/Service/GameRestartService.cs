using Gameplay.Combat;
using Gameplay.Player;
using UnityEngine;
using VContainer.Unity;

namespace Gameplay.Service
{
    public interface IGameRestartService
    {
        void Restart();
    }

    public class GameRestartService : IGameRestartService, IStartable
    {
        private readonly HealthComponent _playerHealth;
        private readonly PlayerWeaponController _playerWeapon;
        private readonly Transform _playerTransform;
        private readonly EnemySpawner _enemySpawner;
        private readonly IEnemyManager _enemyManager;

        private Vector3 _playerSpawnPosition = Vector3.zero;

        public GameRestartService(
            HealthComponent playerHealth,
            PlayerWeaponController playerWeapon,
            EnemySpawner enemySpawner,
            IEnemyManager enemyManager)
        {
            _playerHealth = playerHealth;
            _playerWeapon = playerWeapon;
            _playerTransform = playerHealth.transform;
            _enemySpawner = enemySpawner;
            _enemyManager = enemyManager;
        }

        public void Start()
        {
            _playerSpawnPosition = _playerTransform.position;
        }

        public void Restart()
        {
            _enemyManager.ClearAll();
            _enemyManager.ResetKillCount();

            _playerTransform.position = _playerSpawnPosition;
            _playerHealth.Health.Revive();
            _playerWeapon.ResetWeapons(); 

            _enemySpawner.ResetSpawnState();
        }
    }
}