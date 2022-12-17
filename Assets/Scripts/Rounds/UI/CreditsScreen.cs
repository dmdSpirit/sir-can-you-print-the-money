#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Rounds.UI
{
    public sealed class CreditsScreen : UIElement<object?>
    {
        private GameStateMachine _gameStateMachine = null!;

        [SerializeField]
        private Button _backButton = null!;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            _backButton.OnClickAsObservable()
                .Subscribe(OnBackButtonClicked);
        }

        protected override void OnShow(object? value)
        {
        }

        protected override void OnHide()
        {
        }

        private void OnBackButtonClicked(Unit _)
        {
            _gameStateMachine.MainMenu();
        }
    }
}