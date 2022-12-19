#nullable enable
using System;
using NovemberProject.ClicheSpeech.UI;
using NovemberProject.System;
using NovemberProject.Time;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.ClicheSpeech
{
    public sealed class ShowClicheBubble : MonoBehaviour
    {
        private TimeSystem _timeSystem = null!;
        private bool _isShown;
        private readonly Subject<Unit> _onHidden = new();
        private Timer? _displayTimer;

        [SerializeField]
        private SpeechBubble _speechBubble = null!;

        [SerializeField]
        private float _perCharShowDuration = 1f;

        public IObservable<Unit> OnHidden => _onHidden;

        [Inject]
        private void Construct(TimeSystem timeSystem)
        {
            _timeSystem = timeSystem;
        }

        public void ShowBubble()
        {
            ClicheBible clicheBible = Game.Instance.ClicheBible;
            string text = clicheBible.GetCliche();
            _displayTimer?.Cancel();

            _speechBubble.Show(text);
            _isShown = true;
            float duration = _perCharShowDuration * text.Length;
            _displayTimer = _timeSystem.CreateTimer(duration, _ => HideBubble());
            _displayTimer.Start();
        }

        private void HideBubble()
        {
            if (!_isShown)
            {
                return;
            }

            _isShown = false;
            _speechBubble.Hide();
            _displayTimer?.Cancel();
            _displayTimer = null;
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
            _displayTimer?.Cancel();
            _displayTimer = null;
        }
    }
}