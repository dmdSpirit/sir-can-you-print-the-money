#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.Time;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.ClicheSpeech
{
    public sealed class RandomClicheTrigger : InitializableBehaviour
    {
        private TimeSystem _timeSystem = null!;
        private Timer? _timer;

        [SerializeField]
        private ShowClicheBubble _showClicheBubble = null!;

        [SerializeField]
        private Vector2 _warmUp;

        [SerializeField]
        private Vector2 _cooldown;

        [Inject]
        private void Construct(TimeSystem timeSystem)
        {
            _timeSystem = timeSystem;
        }

        private void Start()
        {
            _showClicheBubble.OnHidden
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
            _timer = _timeSystem.CreateTimer(delay, _ => ShowNextBubble());
            _timer.Start();
        }

        private void OnDisable()
        {
            _timer?.Cancel();
            _timer = null;
        }
    }
}