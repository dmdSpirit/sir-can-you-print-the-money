#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Buildings.UI
{
    public sealed class ResourceStoragePanel : UIElement<IResourceStorage>
    {
        private IResourceStorage _resourceStorage = null!;
        private IDisposable? _resourceCountSub;

        [SerializeField]
        private Image _resourceIcon = null!;

        [SerializeField]
        private TMP_Text _resourceCountText = null!;

        [SerializeField]
        private TMP_Text _title = null!;

        protected override void OnShow(IResourceStorage resourceStorage)
        {
            _resourceCountSub?.Dispose();
            _resourceStorage = resourceStorage;
            _title.text = _resourceStorage.ResourceTitle;
            _resourceIcon.sprite = _resourceStorage.SpriteIcon;
            _resourceCountSub = _resourceStorage.ResourceCount.Subscribe(OnResourceCountChanged);
        }

        protected override void OnHide()
        {
            _resourceCountSub?.Dispose();
        }

        private void OnResourceCountChanged(int resourceCount)
        {
            _resourceCountText.text = resourceCount.ToString();
        }
    }
}