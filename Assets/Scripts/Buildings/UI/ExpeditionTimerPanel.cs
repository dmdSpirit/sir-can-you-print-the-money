#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.Time;
using TMPro;
using UnityEngine;

namespace NovemberProject.Buildings.UI
{
    public sealed class ExpeditionTimerPanel : UIElement<IReadOnlyTimer>
    {
        private IReadOnlyTimer _timer = null!;

        [SerializeField]
        private TMP_Text _timeLeft = null!;

        protected override void OnShow(IReadOnlyTimer timer)
        {
            _timer = timer;
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

            _timeLeft.text = Game.Instance.TimeSystem.EstimateSecondsLeft(_timer)+"s";
        }
    }
}