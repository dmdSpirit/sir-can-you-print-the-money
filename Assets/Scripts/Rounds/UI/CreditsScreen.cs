#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Rounds.UI
{
    public sealed class CreditsScreen : UIElement<object?>
    {
        [SerializeField]
        private Button _backButton=null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _backButton.OnClickAsObservable()
                .TakeUntilDisable(this)
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
            Game.Instance.GameStateMachine.MainMenu();
        }
    }
}