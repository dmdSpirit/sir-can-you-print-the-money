#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Rounds.UI
{
    public interface IRoundTimer : IUIScreen
    {
    }

    public sealed class RoundTimer : UIScreen, IRoundTimer
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

        protected override void OnShow()
        {
            _sub?.Dispose();
            _sub = _roundSystem.OnRoundTimerStarted.Subscribe(_ => OnRoundStarted());
            OnRoundStarted();
        }

        protected override void OnHide()
        {
            _sub?.Dispose();
        }

        private void OnRoundStarted()
        {
            _roundNumber.text = _roundSystem.Round.Value.ToString();
            IReadOnlyTimer? roundTimer = _roundSystem.RoundTimer;
            _timerProgressBar.Show(roundTimer!);
        }
    }
}