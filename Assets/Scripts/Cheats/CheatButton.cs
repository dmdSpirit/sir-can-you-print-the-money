#nullable enable
using NovemberProject.CommonUIStuff;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Cheats
{
    public sealed class CheatButton : UIElement<CheatButtonInfo>
    {
        private CheatButtonInfo _info;

        [SerializeField]
        private Button _button = null!;

        [SerializeField]
        private TMP_Text _title = null!;

        private void Start()
        {
            _button.OnClickAsObservable()
                .Subscribe(OnButtonClick);
        }

        protected override void OnShow(CheatButtonInfo info)
        {
            _info = info;
            _title.text = _info.Title;
        }

        protected override void OnHide()
        {
        }

        private void OnButtonClick(Unit _)
        {
            if (!IsShown)
            {
                return;
            }

            _info.Action.Invoke();
        }
    }
}