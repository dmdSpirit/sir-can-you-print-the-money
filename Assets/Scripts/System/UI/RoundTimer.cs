#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.System.UI
{
    public class RoundTimer : UIElement<object?>
    {
        private readonly CompositeDisposable _subs = new();

        [SerializeField]
        private TMP_Text _roundNumber = null!;

        [SerializeField]
        private TimerProgressBar _timerProgressBar = null!;

        protected override void Initialize()
        {
        }

        protected override void OnShow(object? value)
        {
            gameObject.SetActive(true);
            _subs.Clear();
            OnRoundStarted();
            Game.Instance.RoundSystem.OnRoundStarted.Subscribe(_ => OnRoundStarted()).AddTo(_subs);
        }

        protected override void OnHide()
        {
            _subs.Clear();
            gameObject.SetActive(false);
        }

        private void OnRoundStarted()
        {
            _roundNumber.text = Game.Instance.RoundSystem.Round.Value.ToString();
            IReadOnlyTimer? roundTimer = Game.Instance.RoundSystem.RoundTimer;
            Assert.IsTrue(roundTimer != null);
            _timerProgressBar.Show(roundTimer!);
        }
    }
}