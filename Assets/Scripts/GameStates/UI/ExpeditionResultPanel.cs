#nullable enable
using System;
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
        [SerializeField]
        private TMP_Text _explorersCount = null!;

        [SerializeField]
        private TMP_Text? _rewardsCount;

        [SerializeField]
        private Button _closeButton = null!;

        public IObservable<Unit> OnClose => _closeButton.OnClickAsObservable();

        protected override void OnShow(ExpeditionResult expeditionResult)
        {
            _explorersCount.text = expeditionResult.Explorers.ToString();
            if (_rewardsCount != null)
            {
                _rewardsCount.text = expeditionResult.Rewards.ToString();
            }
        }

        protected override void OnHide()
        {
        }
    }
}