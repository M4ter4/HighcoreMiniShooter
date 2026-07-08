using System;
using Core.Weapons;
using Gameplay.Combat;
using Gameplay.Player;
using Gameplay.Service;
using VContainer.Unity;

namespace UI.HUD
{
    public class HudPresenter : IStartable, IDisposable
    {
        private readonly IHudView _view;
        private readonly HealthComponent _health;
        private readonly PlayerWeaponController _weaponController;
        private readonly IEnemyManager _enemyManager;

        private IWeapon _currentWeapon;

        public HudPresenter(
            IHudView view,
            HealthComponent health,
            PlayerWeaponController weaponController,
            IEnemyManager enemyManager)
        {
            _view = view;
            _health = health;
            _weaponController = weaponController;
            _enemyManager = enemyManager;
        }

        public void Start()
        {
            _health.OnDamaged += HandleHealthChanged;
            _health.OnHealed += HandleHealthChanged;
            _health.OnRevived += Reset;

            _weaponController.OnWeaponChanged += HandleWeaponChanged;

            _enemyManager.OnKillCountChanged += _view.SetKillCount;

            Reset();
            SubscribeToWeapon(_weaponController.CurrentWeapon);
        }

        public void Dispose()
        {
            _health.OnDamaged -= HandleHealthChanged;
            _health.OnHealed -= HandleHealthChanged;
            _weaponController.OnWeaponChanged -= HandleWeaponChanged;

            _enemyManager.OnKillCountChanged -= _view.SetKillCount;

            UnsubscribeFromWeapon();
        }

        private void Reset()
        {
            _view.SetHealth(_health.Health.Max, _health.Health.Max);
            _view.SetKillCount(_enemyManager.KillCount);
        }

        private void HandleHealthChanged(int amount, int current)
        {
            _view.SetHealth(current, _health.Health.Max);
        }

        private void HandleWeaponChanged(IWeapon weapon)
        {
            UnsubscribeFromWeapon();
            SubscribeToWeapon(weapon);
        }

        private void SubscribeToWeapon(IWeapon weapon)
        {
            _currentWeapon = weapon;
            _currentWeapon.OnFired += HandleAmmoChanged;
            _currentWeapon.OnReloadStarted += HandleReloadStarted;
            _currentWeapon.OnReloadFinished += HandleReloadFinished;

            _view.SetWeaponName(_currentWeapon.Data.WeaponName);
            HandleAmmoChanged();
            _view.SetReloading(_currentWeapon.IsReloading);
        }

        private void UnsubscribeFromWeapon()
        {
            if (_currentWeapon == null) return;
            _currentWeapon.OnFired -= HandleAmmoChanged;
            _currentWeapon.OnReloadStarted -= HandleReloadStarted;
            _currentWeapon.OnReloadFinished -= HandleReloadFinished;
        }

        private void HandleAmmoChanged()
        {
            _view.SetAmmo(_currentWeapon.CurrentAmmo, _currentWeapon.Data.MagazineSize);
        }

        private void HandleReloadStarted() => _view.SetReloading(true);
        private void HandleReloadFinished()
        {
            _view.SetReloading(false);
            HandleAmmoChanged();
        }
    }
}