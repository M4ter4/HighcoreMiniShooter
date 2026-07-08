using Gameplay.Input;
using Gameplay.Player;
using Gameplay.Service;
using UI.DeathScreen;
using UI.HUD;
using VContainer.Unity;

namespace Composition
{
    public class GameBootstrap : IStartable
    {
        private readonly EnemySpawner _spawner;
        private readonly IEnemyManager _manager;
        private readonly IHudView _hudView;
        private readonly IDeathScreenView _deathView;
        private readonly PlayerInputProvider _inputProvider;
        private readonly PlayerWeaponController _weaponController;

        public GameBootstrap(EnemySpawner spawner, IEnemyManager manager,
            IHudView hudView, IDeathScreenView deathView,
            PlayerInputProvider inputProvider, PlayerWeaponController weaponController)
        {
            _spawner = spawner;
            _manager = manager;
            _hudView = hudView;
            _deathView = deathView;
            _inputProvider = inputProvider;
            _weaponController = weaponController;
        }

        public void Start()
        {
            _inputProvider.ConfigureWeaponSlots(_weaponController.WeaponCount);

            _spawner.OnEnemySpawned += _manager.TrackSpawn;

            _deathView.SetVisible(false);
            _hudView.SetVisible(true);
        }
    }
}