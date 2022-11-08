#nullable enable
using NovemberProject.System;
using NovemberProject.Time;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NovemberProject.CommonUIStuff
{
    public class TimerProgressBar : UIElement<IReadOnlyTimer>
    {
        private readonly CompositeDisposable _timerSubs = new();

        private IReadOnlyTimer _timer = null!;

        [SerializeField]
        private Image _barImage = null!;

        protected override void OnShow(IReadOnlyTimer timer)
        {
            _timerSubs.Clear();
            _timer = timer;
            Game.Instance.TimeSystem.OnUpdate.Subscribe(OnUpdate).AddTo(_timerSubs);
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
            _barImage.fillAmount = 1 - _timer.ProgressRate;
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