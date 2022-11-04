#nullable enable
using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NovemberProject
{
    public class PopBehaviour : MonoBehaviour
    {
        private IDisposable? _timerSub;

        [SerializeField]
        private ShowClicheBubble _showClicheBubble = null!;

        [SerializeField]
        private Vector2 _warmUp;

        [SerializeField]
        private Vector2 _cooldown;

        private void Start()
        {
            float warmupTime = Random.Range(_warmUp.x, _warmUp.y);
            ShowBubbleAfterDelay(warmupTime);
        }

        private void ShowNextBubble()
        {
            _timerSub?.Dispose();
            _showClicheBubble.ShowBubble();
            _timerSub = _showClicheBubble.OnHidden.Subscribe(_ => OnBubbleHidden());
        }

        private void OnBubbleHidden()
        {
            _timerSub?.Dispose();
            float cooldown = Random.Range(_cooldown.x, _cooldown.y);
            ShowBubbleAfterDelay(cooldown);
        }

        private void ShowBubbleAfterDelay(float delay)
        {
            _timerSub?.Dispose();
            _timerSub = Observable
                .Timer(TimeSpan.FromSeconds(delay))
                .Subscribe(_ => ShowNextBubble());
        }

        private void OnDisable()
        {
            _timerSub?.Dispose();
        }
    }
}