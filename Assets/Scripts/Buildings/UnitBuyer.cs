#nullable enable
using NovemberProject.Core;
using UnityEngine;
using Zenject;

namespace NovemberProject.Buildings
{
    public sealed class UnitBuyer : MonoBehaviour, IBuyUnit
    {
        private IUnitManager _unitManager = null!;

        [SerializeField]
        private UnitType _unitType;

        [SerializeField]
        private string _buyUnitTitle = null!;

        [SerializeField]
        private string _buyUnitButtonText = null!;

        public bool CanBuyUnit => _unitManager.CanBuyUnit();
        public string BuyUnitTitle => _buyUnitTitle;

        public string BuyUnitButtonText => _buyUnitButtonText;

        [Inject]
        private void Construct(UnitManagerProvider unitManagerProvider)
        {
            _unitManager = unitManagerProvider.GetUnitManagerFor(_unitType);
        }

        public void BuyUnit() => _unitManager.BuyUnit();
    }
}