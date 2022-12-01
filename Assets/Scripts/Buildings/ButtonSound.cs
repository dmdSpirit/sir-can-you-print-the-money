#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Buildings
{
    [RequireComponent(typeof(Button))]
    public sealed class ButtonSound : InitializableBehaviour
    {
        private Button _button = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _button = GetComponent<Button>();
            _button.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnClick);
        }

        public void PlaySound()
        {
            Game.Instance.AudioManager.PlayRandom();
        }

        private void OnClick(Unit _)
        {
            Game.Instance.AudioManager.PlayRandom();
        }
    }
}