#nullable enable
using NovemberProject.System;
using NovemberProject.Time;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.CommonUIStuff
{
    public sealed class TimerProgressBar : UIElement<IReadOnlyTimer>
    {
        private TimeSystem _timeSystem = null!;
        private readonly CompositeDisposable _timerSubs = new();

        private IReadOnlyTimer _timer = null!;

        [SerializeField]
        private Image _barImage = null!;

        [Inject]
        private void Construct(TimeSystem timeSystem)
        {
            _timeSystem = timeSystem;
        }

        protected override void OnShow(IReadOnlyTimer timer)
        {
            _timerSubs.Clear();
            _timer = timer;
            _timeSystem.OnUpdate.Subscribe(OnUpdate).AddTo(_timerSubs);
            _timer.OnTimerCanceled.Subscribe(OnTimerCanceled).AddTo(_timerSubs);
            _timer.OnTimerFinished.Subscribe(OnTimerFinished).AddTo(_timerSubs);
            OnUpdate(0);
        }

        protected override void OnHide()
        {
            _timerSubs.Clear();
        }

        private void OnUpdate(float deltaTime)
        {
            _barImage.fillAmount = _timer.ProgressRate;
        }

        private void OnTimerCanceled(IReadOnlyTimer timer)
        {
            Assert.IsTrue(timer == _timer);
            _timerSubs.Clear();
        }

        private void OnTimerFinished(IReadOnlyTimer timer)
        {
            Assert.IsTrue(timer == _timer);
            _timerSubs.Clear();
        }
    }
}