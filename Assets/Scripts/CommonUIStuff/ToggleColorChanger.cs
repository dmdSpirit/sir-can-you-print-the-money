#nullable enable
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.CommonUIStuff
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleColorChanger : MonoBehaviour
    {
        private Toggle _toggle = null!;

        [SerializeField]
        private Color _isOnColor;

        [SerializeField]
        private Color _isOffColor;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void OnEnable()
        {
            _toggle.OnValueChangedAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnValueChanged);
        }

        private void OnValueChanged(bool isOn)
        {
            _toggle.targetGraphic.color = isOn ? _isOnColor : _isOffColor;
        }
    }
}