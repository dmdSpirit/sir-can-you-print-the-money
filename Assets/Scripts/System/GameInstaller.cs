#nullable enable
using NovemberProject.Buildings;
using NovemberProject.CameraSystem;
using NovemberProject.ClicheSpeech;
using NovemberProject.Core;
using NovemberProject.Core.FolkManagement;
using NovemberProject.GameStates;
using NovemberProject.Input;
using NovemberProject.MovingResources;
using NovemberProject.Rounds;
using NovemberProject.System.UI;
using NovemberProject.TechTree;
using NovemberProject.Time;
using NovemberProject.Treasures;
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

        [SerializeField]
        private ArmyManagerSettings _armyManagerSettings = null!;

        [SerializeField]
        private TimeSystemSettings _timeSystemSettings = null!;

        [SerializeField]
        private RoundSystemSettings _roundSystemSettings = null!;

        [SerializeField]
        private StoneControllerSettings _stoneControllerSettings = null!;

        [SerializeField]
        private TreasureControllerSettings _treasureControllerSettings = null!;

        [SerializeField]
        private BuildingSelectorSettings _buildingSelectorSettings = null!;

        [SerializeField]
        private UIManagerSettings _uiManagerSettings = null!;

        [SerializeField]
        private CombatControllerSettings _combatControllerSettings = null!;

        [SerializeField]
        private CoreGameplaySettings _coreGameplaySettings = null!;

        [SerializeField]
        private ResourceMoveEffectSpawnerSettings _resourceMoveEffectSpawnerSettings = null!;

        // Temporary references.
        [SerializeField]
        private CameraController _cameraController = null!;

        [SerializeField]
        private BuildingNameHover _buildingNameHover = null!;

        [SerializeField]
        private MouseOverObserver _mouseOverObserver = null!;

        [SerializeField]
        private ResourceObjectFactory _resourceObjectFactory = null!;

        [SerializeField]
        private GameStarter _gameStarter = null!;

        public override void InstallBindings()
        {
            Container.Bind<FolkManager>().AsSingle();
            Container.Bind<FoodController>().AsSingle();
            Container.Bind<MoneyController>().AsSingle();
            Container.Bind<BuildingsController>().AsSingle();
            Container.Bind<MessageBroker>().AsSingle();
            Container.Bind<Expeditions>().AsSingle();
            Container.Bind<GameStateMachine>().AsSingle();
            Container.Bind<ArmyManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<TimeSystem>().AsSingle();
            Container.Bind<TechController>().AsSingle();
            Container.Bind<RoundSystem>().AsSingle();
            Container.Bind<StoneController>().AsSingle();
            Container.Bind<TreasureController>().AsSingle();
            Container.Bind<BuildingSelector>().AsSingle();
            Container.Bind<UIManager>().AsSingle();
            Container.Bind<CombatController>().AsSingle();
            Container.Bind<CoreGameplay>().AsSingle();
            Container.Bind<ResourceMoveEffectSpawner>().AsSingle();
            Container.Bind<ClicheBible>().AsSingle();
            Container.BindInterfacesAndSelfTo<InputSystem>().AsSingle();


            InstallSettingsBindings();
            InstallTemporaryBindings();
        }

        private void InstallTemporaryBindings()
        {
            // Unfinished.
            Container.Bind<GameStarter>().FromInstance(_gameStarter);
            Container.Bind<CameraController>().FromInstance(_cameraController);
            Container.Bind<BuildingNameHover>().FromInstance(_buildingNameHover);
            Container.Bind<MouseOverObserver>().FromInstance(_mouseOverObserver);
            Container.Bind<ResourceObjectFactory>().FromInstance(_resourceObjectFactory);
        }

        private void InstallSettingsBindings()
        {
            Container.Bind<FolkManagerSettings>().FromInstance(_folkManagerSettings);
            Container.Bind<FoodControllerSettings>().FromInstance(_foodControllerSettings);
            Container.Bind<MoneyControllerSettings>().FromInstance(_moneyControllerSettings);
            Container.Bind<ExpeditionSettings>().FromInstance(_expeditionSettings);
            Container.Bind<ArmyManagerSettings>().FromInstance(_armyManagerSettings);
            Container.Bind<TimeSystemSettings>().FromInstance(_timeSystemSettings);
            Container.Bind<RoundSystemSettings>().FromInstance(_roundSystemSettings);
            Container.Bind<StoneControllerSettings>().FromInstance(_stoneControllerSettings);
            Container.Bind<TreasureControllerSettings>().FromInstance(_treasureControllerSettings);
            Container.Bind<BuildingSelectorSettings>().FromInstance(_buildingSelectorSettings);
            Container.Bind<UIManagerSettings>().FromInstance(_uiManagerSettings);
            Container.Bind<CombatControllerSettings>().FromInstance(_combatControllerSettings);
            Container.Bind<CoreGameplaySettings>().FromInstance(_coreGameplaySettings);
            Container.Bind<ResourceMoveEffectSpawnerSettings>().FromInstance(_resourceMoveEffectSpawnerSettings);
        }
    }
}