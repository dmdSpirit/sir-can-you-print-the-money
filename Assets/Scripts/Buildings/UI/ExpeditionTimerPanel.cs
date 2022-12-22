#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.Time;
using TMPro;
using UnityEngine;
using Zenject;

namespace NovemberProject.Buildings.UI
{
    public sealed class ExpeditionTimerPanel : UIElement<IExpeditionSender>
    {
        private TimeSystem _timeSystem = null!;
        private IReadOnlyTimer _timer = null!;

        [SerializeField]
        private TMP_Text _timeLeft = null!;

        [SerializeField]
        private TMP_Text _explorersCountText = null!;

        [Inject]
        private void Construct(TimeSystem timeSystem)
        {
            _timeSystem = timeSystem;
        }

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

            _timeLeft.text = _timeSystem.EstimateSecondsLeftUnscaled(_timer) + "s";
        }
    }
}