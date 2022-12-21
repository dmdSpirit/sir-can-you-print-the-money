#nullable enable
using NovemberProject.CommonUIStuff;
using UnityEngine;

namespace NovemberProject.System.UI
{
    public interface ISystemInfoPanel : IUIScreen{}
    
    public sealed class SystemInfoPanel : UIScreen, ISystemInfoPanel
    {
        [SerializeField]
        private TimeScalePanel _timeScalePanel = null!;

        [SerializeField]
        private GameStatePanel _gameStatePanel = null!;

        [SerializeField]
        private FPSPanel _fpsPanel = null!;

        protected override void OnShow()
        {
            _timeScalePanel.Show(null);
            _gameStatePanel.Show(null);
            _fpsPanel.Show(null);
        }

        protected override void OnHide()
        {
            _timeScalePanel.Hide();
            _gameStatePanel.Hide();
            _fpsPanel.Hide();
        }
    }
}