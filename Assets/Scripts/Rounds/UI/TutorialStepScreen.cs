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
        private Button? _nextButton;

        [SerializeField]
        private Button? _previousButton;

        public IObservable<Unit>? OnNext => _nextButton?.OnClickAsObservable();
        public IObservable<Unit>? OnPrevious => _previousButton?.OnClickAsObservable();

        protected override void OnShow(object? _)
        {
            
        }

        protected override void OnHide()
        {
        }
    }
}