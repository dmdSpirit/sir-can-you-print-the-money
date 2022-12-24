#nullable enable
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Buildings
{
    public class Building : MonoBehaviour, ISelectable, IBuildingInfo
    {
        private BuildingsController _buildingsController = null!;

        [SerializeField]
        private string _title = null!;

        [SerializeField]
        private string _description = null!;

        [SerializeField]
        private Sprite _image = null!;

        [SerializeField]
        private BuildingInfo _buildingInfo = null!;
        [SerializeField]
        private SelectionBorder _selectionBorder = null!;

        public string Title => _buildingInfo.Title;
        public string Description => _buildingInfo.Description;
        public Sprite Image => _buildingInfo.Image;

        public virtual BuildingType BuildingType => BuildingType.None;
        public IReadOnlyReactiveProperty<bool> IsSelected => _selectionBorder.IsSelected;

        [Inject]
        private void Construct(BuildingsController buildingsController)
        {
            _buildingsController = buildingsController;
            _buildingsController.RegisterBuilding(this);
        }

        public void Select() => _selectionBorder.Select();
        public void Unselect() => _selectionBorder.Unselect();
    }
}