#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Rounds.UI
{
    public sealed class VictoryScreen : UIElement<object?>
    {
        private GameStateMachine _gameStateMachine = null!;

        [SerializeField]
        private Button _toMainMenuButton = null!;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            _toMainMenuButton.OnClickAsObservable()
                .Subscribe(OnToMainMenuButtonClicked);
        }

        protected override void OnShow(object? value)
        {
        }

        protected override void OnHide()
        {
        }

        private void OnToMainMenuButtonClicked(Unit _)
        {
            _gameStateMachine.MainMenu();
        }
    }
}