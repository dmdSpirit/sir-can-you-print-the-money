#nullable enable
using NovemberProject.CommonUIStuff;
using UnityEngine;

namespace NovemberProject.System.UI
{
    public class SystemInfoPanel : InitializableBehaviour
    {
        [SerializeField]
        private TimeScalePanel _timeScalePanel = null!;

        [SerializeField]
        private GameStatePanel _gameStatePanel = null!;

        protected override void Initialize()
        {
            _timeScalePanel.Show();
            _gameStatePanel.Show();
        }
    }
}