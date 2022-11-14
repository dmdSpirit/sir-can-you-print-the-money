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
        private Button _exitGame = null!;

        [SerializeField]
        private Button _continue = null!;

        protected override void Initialize()
        {
            _newGame.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnNewGame);
            _exitGame.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnExitGame);
            _continue.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnExitGame);
        }

        protected override void OnShow(object? _)
        {
        }

        protected override void OnHide()
        {
        }

        private void OnNewGame(Unit _)
        {
            Game.Instance.NewGame();
        }

        private void OnExitGame(Unit _)
        {
            Game.Instance.ExitGame();
        }
    }
}