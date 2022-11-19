#nullable enable
using NovemberProject.CommonUIStuff;
using TMPro;
using UniRx;
using UnityEngine;

namespace NovemberProject.System
{
    public sealed class IdleFolkPanel : InitializableBehaviour
    {
        private FolkManager _folkManager = null!;
        
        [SerializeField]
        private TMP_Text _countText=null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _folkManager = Game.Instance.CoreGameplay.FolkManager;
            _folkManager.IdleFolk
                .TakeUntilDisable(this)
                .Subscribe(OnCountChanged);
        }

        private void OnCountChanged(int count)
        {
            _countText.text = count.ToString();
        }
    }
}