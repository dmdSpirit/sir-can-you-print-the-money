#nullable enable
using System;
using NovemberProject.ClicheSpeech.UI;
using NovemberProject.System;
using NovemberProject.TimeSystem;
using UniRx;
using UnityEngine;

namespace NovemberProject.ClicheSpeech
{
    public class ShowClicheBubble : MonoBehaviour
    {
        private bool _isShown;
        private readonly Subject<Unit> _onHidden = new();
        private Timer? _displayTimer;

        [SerializeField]
        private SpeechBubble _speechBubble = null!;

        [SerializeField]
        private float _perCharShowDuration = 1f;

        public IObservable<Unit> OnHidden => _onHidden;

        public void ShowBubble()
        {
            var clicheBible = Game.Instance.ClicheBible;
            string text = clicheBible.GetCliche();
            _displayTimer?.Cancel();

            _speechBubble.Show(text);
            _isShown = true;
            float duration = _perCharShowDuration * text.Length;
            _displayTimer = Game.Instance.TimeSystem.CreateTimer(duration, _ => HideBubble());
            _displayTimer.Start();
        }

        public void HideBubble()
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