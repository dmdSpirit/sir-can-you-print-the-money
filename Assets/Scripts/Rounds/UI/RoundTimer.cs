#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.Rounds.UI
{
    public class RoundTimer : UIElement<object?>
    {
        private readonly CompositeDisposable _subs = new();

        [SerializeField]
        private TMP_Text _roundNumber = null!;

        [SerializeField]
        private TimerProgressBar _timerProgressBar = null!;

        protected override void OnShow(object? value)
        {
            _subs.Clear();
            OnRoundStarted();
            Game.Instance.RoundSystem.OnRoundStarted.Subscribe(_ => OnRoundStarted()).AddTo(_subs);
        }

        protected override void OnHide()
        {
            _subs.Clear();
        }

        private void OnRoundStarted()
        {
            _roundNumber.text = Game.Instance.RoundSystem.Round.Value.ToString();
            RoundSystem roundSystem = Game.Instance.RoundSystem;
            IReadOnlyTimer? roundTimer = roundSystem.RoundTimer;
            Assert.IsTrue(roundTimer != null);
            _timerProgressBar.Show(roundTimer!);
        }
    }
}