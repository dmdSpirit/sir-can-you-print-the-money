#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.Rounds.UI
{
    public sealed class RoundTimer : UIElement<object?>
    {
        private IDisposable? _sub;

        [SerializeField]
        private TMP_Text _roundNumber = null!;

        [SerializeField]
        private TimerProgressBar _timerProgressBar = null!;

        protected override void OnShow(object? value)
        {
            _sub?.Dispose();
            _sub= Game.Instance.RoundSystem.OnRoundTimerStarted.Subscribe(_ => OnRoundStarted());
            OnRoundStarted();
        }

        protected override void OnHide()
        {
            _sub?.Dispose();
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