#nullable enable
using System;
using NovemberProject.ClicheSpeech.UI;
using NovemberProject.System;
using UniRx;
using UnityEngine;

namespace NovemberProject.ClicheSpeech
{
    public class ShowClicheBubble : MonoBehaviour
    {
        private bool _isShown;
        private IDisposable? _timerSub;
        private readonly Subject<Unit> _onHidden = new();

        [SerializeField]
        private SpeechBubble _speechBubble = null!;

        [SerializeField]
        private float _perCharShowDuration = 1f;

        public IObservable<Unit> OnHidden => _onHidden;

        public void ShowBubble()
        {
            var clicheBible = Game.Instance.ClicheBible;
            string text = clicheBible.GetCliche();
            if (_isShown)
            {
                _timerSub?.Dispose();
            }

            _speechBubble.Show(text);
            _isShown = true;
            float duration = _perCharShowDuration * text.Length;
            _timerSub = Observable
                .Timer(TimeSpan.FromSeconds(duration))
                .Subscribe(_ => HideBubble());
        }

        public void HideBubble()
        {
            if (!_isShown)
            {
                return;
            }

            _isShown = false;
            _speechBubble.Hide();
            _timerSub?.Dispose();
            _onHidden.OnNext(Unit.Default);
        }

        private void OnDisable()
        {
            if (!_isShown)
            {
                return;
            }

            _isShown = false;
            _speechBubble.Hide();
            _timerSub?.Dispose();
        }
    }
}