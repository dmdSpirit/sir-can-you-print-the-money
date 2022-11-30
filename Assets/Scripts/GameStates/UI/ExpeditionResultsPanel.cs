#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.GameStates.UI
{
    public sealed class ExpeditionResultsPanel : UIElement<ExpeditionResult>
    {
        private ExpeditionResult _expeditionResult;

        [SerializeField]
        private ExpeditionResultPanel _successPanel = null!;

        [SerializeField]
        private ExpeditionResultPanel _failPanel = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _successPanel.OnClose
                .TakeUntilDisable(this)
                .Subscribe(OnCloseClicked);
            _failPanel.OnClose
                .TakeUntilDisable(this)
                .Subscribe(OnCloseClicked);
        }

        protected override void OnShow(ExpeditionResult expeditionResult)
        {
            _expeditionResult = expeditionResult;
            if (_expeditionResult.IsSuccess)
            {
                _successPanel.Show(expeditionResult);
                _failPanel.Hide();
            }
            else
            {
                _failPanel.Show(expeditionResult);
                _successPanel.Hide();
            }
        }

        protected override void OnHide()
        {
            _successPanel.Hide();
            _failPanel.Hide();
        }

        private void OnCloseClicked(Unit _)
        {
            Game.Instance.GameStateMachine.ExpeditionFinishedExit();
        }
    }
}