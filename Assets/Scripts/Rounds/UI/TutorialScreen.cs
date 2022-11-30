#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace NovemberProject.Rounds.UI
{
    public sealed class TutorialScreen : UIElement<object?>
    {
        private int _currentStep;

        [SerializeField]
        private TutorialStepScreen[] _tutorialStepScreens = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            foreach (TutorialStepScreen tutorialStepScreen in _tutorialStepScreens)
            {
                tutorialStepScreen.OnNext
                    .TakeUntilDisable(this)
                    .Subscribe(OnNext);
                tutorialStepScreen.OnPrevious?
                    .TakeUntilDisable(this)
                    .Subscribe(OnPrevious);
            }
        }

        protected override void OnShow(object? value)
        {
            _currentStep = 0;
            Assert.IsTrue(_tutorialStepScreens.Length > 0);
            _tutorialStepScreens[_currentStep].Show(null);
        }

        protected override void OnHide()
        {
            HideAll();
        }

        private void HideAll()
        {
            foreach (TutorialStepScreen tutorialStepScreen in _tutorialStepScreens)
            {
                tutorialStepScreen.Hide();
            }
        }

        private void OnNext(Unit _)
        {
            _tutorialStepScreens[_currentStep].Hide();
            _currentStep++;
            if (_tutorialStepScreens.Length <= _currentStep)
            {
                Game.Instance.GameStateMachine.StartRound();
                return;
            }

            _tutorialStepScreens[_currentStep].Show(null);
        }

        private void OnPrevious(Unit _)
        {
            if (_currentStep == 0)
            {
                return;
            }

            _tutorialStepScreens[_currentStep].Hide();
            _currentStep--;

            _tutorialStepScreens[_currentStep].Show(null);
        }
    }
}