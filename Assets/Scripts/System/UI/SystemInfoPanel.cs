#nullable enable
using System.Globalization;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.System.UI
{
    public class SystemInfoPanel : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _timeScale = null!;

        private void OnEnable()
        {
            Game.Instance.OnInitialized.TakeUntilDisable(this).Subscribe(_ => Initialize());
        }

        private void Initialize()
        {
            Game.Instance.TimeSystem.TimeScale.TakeUntilDisable(this).Subscribe(UpdateScale);
        }

        private void UpdateScale(float timeScale)
        {
            _timeScale.text = timeScale.ToString(CultureInfo.InvariantCulture);
        }
    }
}