#nullable enable
using NovemberProject.System.Messages;
using UniRx;
using UnityEngine.Assertions;

namespace NovemberProject.Treasures
{
    public sealed class TreasureController
    {
        private readonly ReactiveProperty<int> _treasures = new();
        private readonly TreasureControllerSettings _settings;
        private readonly MessageBroker _messageBroker;

        public IReadOnlyReactiveProperty<int> Treasures => _treasures;

        public TreasureController(TreasureControllerSettings treasureControllerSettings, MessageBroker messageBroker)
        {
            _settings = treasureControllerSettings;
            _messageBroker = messageBroker;
            _messageBroker.Receive<NewGameMessage>().Subscribe(OnNewGame);
        }

        private void OnNewGame(NewGameMessage message)
        {
            _treasures.Value = _settings.StartingTreasures;
        }

        public void AddTreasures(int treasures)
        {
            _treasures.Value += treasures;
        }

        public void SpendTreasures(int cost)
        {
            Assert.IsTrue(_treasures.Value >= cost);
            _treasures.Value -= cost;
        }
    }
}