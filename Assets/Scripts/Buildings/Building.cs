#nullable enable
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Buildings
{
    public class Building : MonoBehaviour, ISelectable
    {
        private readonly ReactiveProperty<bool> _isSelected = new();

        private BuildingsController _buildingsController = null!;

        [SerializeField]
        private string _title = null!;

        [SerializeField]
        private string _description = null!;

        [SerializeField]
        private Sprite _image = null!;

        [SerializeField]
        private GameObject _selectionBorder = null!;

        public string Title => _title;
        public string Description => _description;
        public Sprite Image => _image;
        public virtual BuildingType BuildingType { get; } = BuildingType.None;
        public IReadOnlyReactiveProperty<bool> IsSelected => _isSelected;

        [Inject]
        private void Construct(BuildingsController buildingsController)
        {
            _buildingsController = buildingsController;
            _buildingsController.RegisterBuilding(this);
            _selectionBorder.SetActive(false);
        }

        public void Select()
        {
            _isSelected.Value = true;
            _selectionBorder.SetActive(true);
        }

        public void Unselect()
        {
            _isSelected.Value = false;
            _selectionBorder.SetActive(false);
        }
    }
}