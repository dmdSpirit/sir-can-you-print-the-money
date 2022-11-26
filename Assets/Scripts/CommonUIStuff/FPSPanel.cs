#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.CommonUIStuff
{
    public sealed class FPSPanel : UIElement<object?>
    {
        private IDisposable? _sub;
        private readonly List<float> _fpsList = new();

        [SerializeField]
        private TMP_Text _current = null!;

        [SerializeField]
        private TMP_Text _average = null!;

        protected override void OnShow(object? value)
        {
            _sub?.Dispose();
            _sub = Observable.Timer(TimeSpan.FromSeconds(.3)).Repeat().Subscribe(UpdateFPS);
        }

        protected override void OnHide()
        {
            _sub?.Dispose();
        }

        private void Update()
        {
            _fpsList.Add(1f / UnityEngine.Time.unscaledDeltaTime);
        }

        private void UpdateFPS(long _)
        {
            _current.text = $"{_fpsList.Min():0.##}/{_fpsList.Average():0.##}";
            _average.text =
                $"{(UnityEngine.Time.frameCount / UnityEngine.Time.unscaledTime):0.##}";
            _fpsList.Clear();
        }
    }
}