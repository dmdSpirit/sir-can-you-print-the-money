#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.CameraSystem
{
    public sealed class InfoPanel : UIElement<IInfo>
    {
        private IInfo _info = null!;
        private IDisposable _updateSub = null!;

        [SerializeField]
        private Image _image = null!;

        [SerializeField]
        private Text _text = null!;

        protected override void OnShow(IInfo info)
        {
            if (IsShown)
            {
                _updateSub.Dispose();
            }

            _info = info;
            _updateSub = _info.OnUpdated.Subscribe(OnUpdateInfo);
        }

        protected override void OnHide()
        {
            if (!IsShown)
            {
                return;
            }

            _updateSub.Dispose();
        }

        private void OnUpdateInfo(Unit _)
        {
            _image.sprite = _info.Image;
            _text.text = _info.Text;
        }
    }
}