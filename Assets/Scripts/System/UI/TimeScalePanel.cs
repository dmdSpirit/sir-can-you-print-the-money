#nullable enable
using System;
using System.Globalization;
using NovemberProject.CommonUIStuff;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.System.UI
{
    public class TimeScalePanel : UIElement<object?>
    {
        private IDisposable? _sub;

        [SerializeField]
        private TMP_Text _timeScale = null!;

        protected override void OnShow(object? _)
        {
            _sub = Game.Instance.TimeSystem.TimeScale.Subscribe(UpdateScale);
        }

        protected override void OnHide()
        {
            _sub?.Dispose();
        }

        private void UpdateScale(float timeScale)
        {
            _timeScale.text = timeScale.ToString(CultureInfo.InvariantCulture);
        }
    }
}