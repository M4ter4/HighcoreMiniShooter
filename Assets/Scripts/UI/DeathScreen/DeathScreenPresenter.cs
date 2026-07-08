using System;
using Gameplay.Combat;
using Gameplay.ObjectPooling;
using Gameplay.Service;
using UI.HUD;
using UnityEngine;
using VContainer.Unity;

namespace UI.DeathScreen
{
    public class DeathScreenPresenter : IStartable, IDisposable
    {
        private readonly IDeathScreenView _deathView;
        private readonly IHudView _hudView;
        private readonly HealthComponent _playerHealth;
        private readonly IGameRestartService _restartService;
        private readonly IProjectilePool _projectilePool;

        public DeathScreenPresenter(
            IDeathScreenView deathView,
            IHudView hudView,
            HealthComponent playerHealth,
            IGameRestartService restartService,
            IProjectilePool projectilePool)
        {
            _deathView = deathView;
            _hudView = hudView;
            _playerHealth = playerHealth;
            _restartService = restartService;
            _projectilePool = projectilePool;
        }

        public void Start()
        {
            _playerHealth.OnDeath += HandleDeath;
            _deathView.OnRestartClicked += HandleRestartClicked;
        }

        public void Dispose()
        {
            _playerHealth.OnDeath -= HandleDeath;
            _deathView.OnRestartClicked -= HandleRestartClicked;
        }

        private void HandleDeath()
        {
            _hudView.SetVisible(false);
            _deathView.SetVisible(true);
            _playerHealth.gameObject.SetActive(false);
            _projectilePool.ReleaseAll();
            Time.timeScale = 0f;
        }

        private void HandleRestartClicked()
        {
            _restartService.Restart();
            _deathView.SetVisible(false);
            _hudView.SetVisible(true);
            _playerHealth.gameObject.SetActive(true);
            Time.timeScale = 1f;
        }
    }
}