#nullable enable
using NovemberProject.Buildings;
using NovemberProject.CoreGameplay;
using NovemberProject.CoreGameplay.FolkManagement;
using NovemberProject.GameStates;
using NovemberProject.Input;
using NovemberProject.MovingResources;
using NovemberProject.TechTree;
using NovemberProject.Time;
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

        // Temporary references.
        [SerializeField]
        private TechController _techController = null!;

        [SerializeField]
        private ResourceMoveEffectSpawner _resourceMoveEffectSpawner = null!;

        [SerializeField]
        private CoreGameplay.CoreGameplay _coreGameplay = null!;

        [SerializeField]
        private CombatController _combatController = null!;

        [SerializeField]
        private InputSystem _inputSystem = null!;

        [SerializeField]
        private TimeSystemUpdater _timeSystemUpdater = null!;

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
            Container.Bind<TimeSystem>().AsSingle();
            Container.Bind<TechController>().AsSingle();

            InstallSettingsBindings();
            InstallTemporaryBindings();
        }

        private void InstallTemporaryBindings()
        {
            // Unfinished.
            Container.Bind<ResourceMoveEffectSpawner>().FromInstance(_resourceMoveEffectSpawner);
            Container.Bind<Game>().FromInstance(Game.Instance);
            Container.Bind<CoreGameplay.CoreGameplay>().FromInstance(_coreGameplay);
            Container.Bind<CombatController>().FromInstance(_combatController);
            Container.Bind<TimeSystemUpdater>().FromInstance(_timeSystemUpdater);
            Container.Bind<InputSystem>().FromInstance(_inputSystem);
        }

        private void InstallSettingsBindings()
        {
            Container.Bind<FolkManagerSettings>().FromInstance(_folkManagerSettings);
            Container.Bind<FoodControllerSettings>().FromInstance(_foodControllerSettings);
            Container.Bind<MoneyControllerSettings>().FromInstance(_moneyControllerSettings);
            Container.Bind<ExpeditionSettings>().FromInstance(_expeditionSettings);
            Container.Bind<ArmyManagerSettings>().FromInstance(_armyManagerSettings);
            Container.Bind<TimeSystemSettings>().FromInstance(_timeSystemSettings);
        }
    }
}