#nullable enable
using System.Globalization;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.System.UI
{
    public class SystemInfoPanel : InitializableBehaviour
    {
        [SerializeField]
        private TMP_Text _timeScale = null!;

        protected override void Initialize()
        {
            Game.Instance.TimeSystem.TimeScale.TakeUntilDisable(this).Subscribe(UpdateScale);
        }

        private void UpdateScale(float timeScale)
        {
            _timeScale.text = timeScale.ToString(CultureInfo.InvariantCulture);
        }
    }
}