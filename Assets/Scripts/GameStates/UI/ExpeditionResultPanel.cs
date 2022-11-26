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
    public sealed class ExpeditionResultPanel : UIElement<ExpeditionResult>
    {
        private ExpeditionResult _expeditionResult;

        [SerializeField]
        private TMP_Text _explorersCount = null!;

        [SerializeField]
        private TMP_Text _rewardsCount = null!;

        [SerializeField]
        private Button _closeButton = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _closeButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnCloseClicked);
        }

        protected override void OnShow(ExpeditionResult expeditionResult)
        {
            _expeditionResult = expeditionResult;
            _explorersCount.text = expeditionResult.Explorers.ToString();
            _rewardsCount.text = expeditionResult.Rewards.ToString();
        }

        protected override void OnHide()
        {
        }

        private void OnCloseClicked(Unit _)
        {
            Game.Instance.GameStateMachine.ExpeditionFinishedExit();
        }
    }
}