#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.Core;
using NovemberProject.Time;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Rounds.UI
{
    public interface INotificationsPanel : IUIScreen
    {
    }

    public sealed class NotificationsPanel : UIScreen, INotificationsPanel
    {
        private Timer? _showTimer;

        private TimeSystem _timeSystem = null!;
        private MessageBroker _messageBroker = null!;

        [SerializeField]
        private Notification _folkStarvedNotification = null!;

        [SerializeField]
        private Notification _armyStarvedNotification = null!;

        [SerializeField]
        private Notification _folkExecutedNotification = null!;

        [SerializeField]
        private Notification _armyDesertedNotification = null!;

        [SerializeField]
        private float _showDuration = 5f;

        [Inject]
        private void Construct(TimeSystem timeSystem, MessageBroker messageBroker)
        {
            _timeSystem = timeSystem;
            _messageBroker = messageBroker;
            _messageBroker.Receive<FolkStarvedMessage>().Subscribe(ShowNotification);
            _messageBroker.Receive<ArmyStarvedMessage>().Subscribe(ShowNotification);
            _messageBroker.Receive<FolkExecutedMessage>().Subscribe(ShowNotification);
            _messageBroker.Receive<ArmyDesertedMessage>().Subscribe(ShowNotification);
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
            HideAll();
        }

        private void ShowNotification(INotificationMessage message)
        {
            _showTimer?.Cancel();
            HideAll();
            switch (message)
            {
                case FolkStarvedMessage folkStarvedMessage:
                    _folkStarvedNotification.Show(folkStarvedMessage.Count);
                    break;
                case ArmyStarvedMessage armyStarvedMessage:
                    _armyStarvedNotification.Show(armyStarvedMessage.Count);
                    break;
                case FolkExecutedMessage folkExecuted:
                    _folkExecutedNotification.Show(folkExecuted.Count);
                    break;
                case ArmyDesertedMessage armyDeserted:
                    _armyDesertedNotification.Show(armyDeserted.Count);
                    break;
            }

            _showTimer = _timeSystem.CreateUnscaledTimer(_showDuration, OnNotificationExpire);
            _showTimer.Start();
        }

        private void OnNotificationExpire(Timer _)
        {
            _showTimer = null;
            Hide();
        }

        private void HideAll()
        {
            _folkStarvedNotification.Hide();
            _armyStarvedNotification.Hide();
            _folkExecutedNotification.Hide();
            _armyDesertedNotification.Hide();
        }
    }
}