#nullable enable
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.System.UI
{
    public sealed class MainMenu : UIElement<object?>
    {
        [SerializeField]
        private Button _newGame = null!;

        [SerializeField]
        private Button _credits = null!;

        [SerializeField]
        private Button _exitGame = null!;

        [SerializeField]
        private Button _continue = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _newGame.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnNewGame);
            _exitGame.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnExitGame);
            _continue.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnExitGame);
            _credits.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnCredits);
        }

        protected override void OnShow(object? _)
        {
        }

        protected override void OnHide()
        {
        }

        private static void OnNewGame(Unit _)
        {
            Game.Instance.GameStateMachine.NewGame();
        }

        private static void OnExitGame(Unit _)
        {
            Game.Instance.GameStateMachine.ExitGame();
        }

        private static void OnCredits(Unit _)
        {
            Game.Instance.GameStateMachine.Credits();
        }
    }
}