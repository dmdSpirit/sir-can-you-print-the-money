#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.GameStates;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.System.UI
{
    public interface IMainMenu : IUIScreen
    {
    }

    public sealed class MainMenu : UIScreen, IMainMenu
    {
        private GameStateMachine _gameStateMachine = null!;

        [SerializeField]
        private Button _newGame = null!;

        [SerializeField]
        private Button _credits = null!;

        [SerializeField]
        private Button _exitGame = null!;

        [SerializeField]
        private Button _continue = null!;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            _newGame.OnClickAsObservable()
                .Subscribe(OnNewGame);
            _exitGame.OnClickAsObservable()
                .Subscribe(OnExitGame);
            _continue.OnClickAsObservable()
                .Subscribe(OnExitGame);
            _credits.OnClickAsObservable()
                .Subscribe(OnCredits);
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        private void OnNewGame(Unit _) => _gameStateMachine.NewGame();
        private void OnExitGame(Unit _) => _gameStateMachine.ExitGame();
        private void OnCredits(Unit _) => _gameStateMachine.Credits();
    }
}