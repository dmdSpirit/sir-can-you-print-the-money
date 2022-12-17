﻿#nullable enable
using NovemberProject.Buildings;
using NovemberProject.CoreGameplay;
using NovemberProject.CoreGameplay.FolkManagement;
using NovemberProject.MovingResources;
using NovemberProject.TechTree;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.System
{
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private FolkManagerSettings _folkManagerSettings = null!;

        [SerializeField]
        private FoodControllerSettings _foodControllerSettings = null!;

        [SerializeField]
        private MoneyControllerSettings _moneyControllerSettings = null!;

        [SerializeField]
        private ExpeditionSettings _expeditionSettings = null!;

        // Temporary references.
        [SerializeField]
        private TechController _techController = null!;

        [SerializeField]
        private ResourceMoveEffectSpawner _resourceMoveEffectSpawner = null!;

        public override void InstallBindings()
        {
            Container.Bind<FolkManager>().AsSingle();
            Container.Bind<FoodController>().AsSingle();
            Container.Bind<MoneyController>().AsSingle();
            Container.Bind<BuildingsController>().AsSingle();
            Container.Bind<MessageBroker>().AsSingle();
            Container.Bind<Expeditions>().AsSingle();
            Container.Bind<FolkManagerSettings>().FromInstance(_folkManagerSettings);
            Container.Bind<FoodControllerSettings>().FromInstance(_foodControllerSettings);
            Container.Bind<MoneyControllerSettings>().FromInstance(_moneyControllerSettings);
            Container.Bind<ExpeditionSettings>().FromInstance(_expeditionSettings);

            // Unfinished.
            Container.Bind<TechController>().FromInstance(_techController);
            Container.Bind<ResourceMoveEffectSpawner>().FromInstance(_resourceMoveEffectSpawner);
            Container.Bind<Game>().FromInstance(Game.Instance);
        }
    }
}