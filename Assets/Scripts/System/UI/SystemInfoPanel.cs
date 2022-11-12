﻿#nullable enable
using NovemberProject.CommonUIStuff;
using UnityEngine;

namespace NovemberProject.System.UI
{
    public class SystemInfoPanel : UIElement<object?>
    {
        [SerializeField]
        private TimeScalePanel _timeScalePanel = null!;

        [SerializeField]
        private GameStatePanel _gameStatePanel = null!;

        protected override void OnShow(object? _)
        {
            _timeScalePanel.Show(null);
            _gameStatePanel.Show(null);
        }

        protected override void OnHide()
        {
            _timeScalePanel.Hide();
            _gameStatePanel.Hide();
        }
    }
}