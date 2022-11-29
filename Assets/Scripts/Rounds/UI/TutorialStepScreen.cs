#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Rounds.UI
{
    public sealed class TutorialStepScreen : UIElement<object?>
    {
        private readonly Subject<Unit> _onNext = new();

        [SerializeField]
        private Button _nextButton = null!;

        public IObservable<Unit> OnNext => _onNext;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _nextButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnNextButton);
        }

        protected override void OnShow(object? value)
        {
        }

        protected override void OnHide()
        {
        }

        private void OnNextButton(Unit _) => _onNext.OnNext(Unit.Default);
    }
}