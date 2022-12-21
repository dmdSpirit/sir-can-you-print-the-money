#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Rounds.UI
{
    public interface ICreditsScreen : IUIScreen
    {
    }

    public sealed class CreditsScreen : UIScreen, ICreditsScreen
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

        protected override void OnShow()
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