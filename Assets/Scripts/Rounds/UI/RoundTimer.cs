#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace NovemberProject.Rounds.UI
{
    public sealed class RoundTimer : UIElement<object?>
    {
        private IDisposable? _sub;
        private RoundSystem _roundSystem = null!;

        [SerializeField]
        private TMP_Text _roundNumber = null!;

        [SerializeField]
        private TimerProgressBar _timerProgressBar = null!;

        [Inject]
        private void Construct(RoundSystem roundSystem)
        {
            _roundSystem = roundSystem;
        }

        protected override void OnShow(object? value)
        {
            _sub?.Dispose();
            _sub= _roundSystem.OnRoundTimerStarted.Subscribe(_ => OnRoundStarted());
            OnRoundStarted();
        }

        protected override void OnHide()
        {
            _sub?.Dispose();
        }

        private void OnRoundStarted()
        {
            _roundNumber.text = _roundSystem.Round.Value.ToString();
            RoundSystem roundSystem = _roundSystem;
            IReadOnlyTimer? roundTimer = roundSystem.RoundTimer;
            Assert.IsTrue(roundTimer != null);
            _timerProgressBar.Show(roundTimer!);
        }
    }
}