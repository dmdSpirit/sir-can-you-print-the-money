#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.Time;
using TMPro;
using UnityEngine;

namespace NovemberProject.Buildings.UI
{
    public sealed class ExpeditionTimerPanel : UIElement<IExpeditionSender>
    {
        private IReadOnlyTimer _timer = null!;

        [SerializeField]
        private TMP_Text _timeLeft = null!;
        [SerializeField]
        private TMP_Text _explorersCountText = null!;

        protected override void OnShow(IExpeditionSender expeditionSender)
        {
            _timer = expeditionSender.ExpeditionTimer;
            _explorersCountText.text = expeditionSender.WorkerCount.Value.ToString();
        }

        protected override void OnHide()
        {
            _timer = null!;
        }

        private void Update()
        {
            if (!IsShown)
            {
                return;
            }

            _timeLeft.text = Game.Instance.TimeSystem.EstimateSecondsLeftUnscaled(_timer)+"s";
        }
    }
}