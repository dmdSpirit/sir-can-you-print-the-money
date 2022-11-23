#nullable enable
using NovemberProject.CommonUIStuff;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Buildings.UI
{
    public sealed class BuyUnitPanel : UIElement<IBuyUnit>
    {
        private IBuyUnit _buyUnit = null!;

        [SerializeField]
        private TMP_Text _title = null!;

        [SerializeField]
        private Button _buyButton = null!;

        [SerializeField]
        private TMP_Text _buyButtonText = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _buyButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnBuyButtonClicked);
        }

        protected override void OnShow(IBuyUnit buyUnit)
        {
            _buyUnit = buyUnit;
            _title.text = _buyUnit.BuyUnitTitle;
            _buyButtonText.text = _buyUnit.BuyUnitButtonText;
        }

        protected override void OnHide()
        {
        }

        private void OnBuyButtonClicked(Unit _)
        {
            _buyUnit.BuyUnit();
        }
    }
}