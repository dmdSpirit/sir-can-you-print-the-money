#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Rounds.UI
{
    public sealed class VictoryScreen : UIElement<object?>
    {
        [SerializeField]
        private Button _toMainMenuButton = null!;
        
        protected override void OnInitialized()
        {
            base.OnInitialized();
            _toMainMenuButton.OnClickAsObservable()
                .TakeUntilDisable(this)
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
            Game.Instance.GameStateMachine.MainMenu();
        }
    }
}