#nullable enable
using NovemberProject.CoreGameplay;
using NovemberProject.CoreGameplay.FolkManagement;
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

        public override void InstallBindings()
        {
            Container.Bind<FolkManager>().AsSingle();
            Container.Bind<FolkManagerSettings>().FromInstance(_folkManagerSettings);
            Container.Bind<MessageBroker>().FromInstance(Game.Instance.MessageBroker);
            Container.Bind<FoodController>().FromInstance(Game.Instance.FoodController);
            Container.Bind<MoneyController>().FromInstance(Game.Instance.MoneyController);
            Container.Bind<TechController>().FromInstance(Game.Instance.TechController);
        }
    }
}