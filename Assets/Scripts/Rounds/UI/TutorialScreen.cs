#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace NovemberProject.Rounds.UI
{
    public sealed class TutorialScreen : UIElement<object?>
    {
        private GameStateMachine _gameStateMachine = null!;
        private int _currentStep;

        [SerializeField]
        private TutorialStepScreen[] _tutorialStepScreens = null!;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            foreach (TutorialStepScreen tutorialStepScreen in _tutorialStepScreens)
            {
                tutorialStepScreen.OnNext
                    .Subscribe(OnNext);
                tutorialStepScreen.OnPrevious?
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
                _gameStateMachine.StartRound();
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