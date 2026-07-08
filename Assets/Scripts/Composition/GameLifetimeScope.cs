using Core.ObjectManagement;
using Gameplay.Combat;
using Gameplay.Input;
using Gameplay.ObjectPooling;
using Gameplay.Player;
using Gameplay.Service;
using UI.DeathScreen;
using UI.HUD;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Composition
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private Transform projectilePoolRoot;
        [SerializeField] private HealthComponent playerHealthComponent;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterCore(builder);
            RegisterPlayer(builder);
            RegisterEnemies(builder);
            RegisterUi(builder);
            RegisterEntryPoints(builder);
        }

        private void RegisterCore(IContainerBuilder builder)
        {
            builder.Register<WeaponFactory>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<IProjectilePool>(_ => new ProjectilePoolService(projectilePoolRoot), Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<Camera>();
        }

        private void RegisterPlayer(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<PlayerInputProvider>().As<IInputProvider>().AsSelf();
            builder.RegisterComponentInHierarchy<PlayerMotion>();
            builder.RegisterComponentInHierarchy<PlayerAimController>();
            builder.RegisterComponentInHierarchy<PlayerWeaponController>();
            builder.RegisterComponent(playerHealthComponent);
        }

        private void RegisterEnemies(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<EnemySpawner>();

            builder.Register<EnemyManager>(Lifetime.Scoped)
                .As<IEnemyManager>()
                .AsSelf();
        }

        private void RegisterUi(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<HudView>().As<IHudView>();
            builder.RegisterComponentInHierarchy<DeathScreenView>().As<IDeathScreenView>();
        }

        private void RegisterEntryPoints(IContainerBuilder builder)
        {
            builder.Register<GameRestartService>(Lifetime.Scoped)
                .As<IGameRestartService>()
                .As<IStartable>();
            
            builder.RegisterEntryPoint<GameBootstrap>();
            builder.RegisterEntryPoint<HudPresenter>();
            builder.RegisterEntryPoint<DeathScreenPresenter>();
        }
    }
}