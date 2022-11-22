#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Rounds.UI
{
    public sealed class NewGamePanel : UIElement<object?>
    {
        [SerializeField]
        private Button _startGameButton = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _startGameButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnStartGameClicked);
        }

        protected override void OnShow(object? value)
        {
        }

        protected override void OnHide()
        {
        }

        private void OnStartGameClicked(Unit _)
        {
            Game.Instance.GameStateMachine.StartRound();
        }
    }
}