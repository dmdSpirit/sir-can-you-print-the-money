#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.Time;
using UnityEngine;

namespace NovemberProject.Rounds.UI
{
    public sealed class NotificationsPanel : UIElement<object?>
    {
        private Timer? _showTimer;

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

        protected override void OnShow(object? value)
        {
        }

        protected override void OnHide()
        {
            HideAll();
        }

        public void ShowNotification(NotificationType notificationType, int count)
        {
            _showTimer?.Cancel();
            HideAll();
            switch (notificationType)
            {
                case NotificationType.None:
                    return;
                case NotificationType.FolkStarved:
                    _folkStarvedNotification.Show(count);
                    break;
                case NotificationType.ArmyStarved:
                    _armyStarvedNotification.Show(count);
                    break;
                case NotificationType.FolkExecuted:
                    _folkExecutedNotification.Show(count);
                    break;
                case NotificationType.ArmyDeserted:
                    _armyDesertedNotification.Show(count);
                    break;
            }

            _showTimer = Game.Instance.TimeSystem.CreateUnscaledTimer(_showDuration, OnNotificationExpire);
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