#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.Time;
using UniRx;
using UnityEngine;

namespace NovemberProject.ClicheSpeech
{
    public sealed class RandomClicheTrigger : InitializableBehaviour
    {
        private Timer? _timer;

        [SerializeField]
        private ShowClicheBubble _showClicheBubble = null!;

        [SerializeField]
        private Vector2 _warmUp;

        [SerializeField]
        private Vector2 _cooldown;

        protected override void Initialize()
        {
            _showClicheBubble.OnHidden
                .TakeUntilDisable(this)
                .Subscribe(_ => OnBubbleHidden());
            float warmupTime = Random.Range(_warmUp.x, _warmUp.y);
            ShowBubbleAfterDelay(warmupTime);
        }

        private void ShowNextBubble()
        {
            _showClicheBubble.ShowBubble();
        }

        private void OnBubbleHidden()
        {
            float cooldown = Random.Range(_cooldown.x, _cooldown.y);
            ShowBubbleAfterDelay(cooldown);
        }

        private void ShowBubbleAfterDelay(float delay)
        {
            _timer = Game.Instance.TimeSystem.CreateTimer(delay, _ => ShowNextBubble());
            _timer.Start();
        }

        private void OnDisable()
        {
            _timer?.Cancel();
            _timer = null;
        }
    }
}