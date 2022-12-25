#nullable enable
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;

namespace NovemberProject.Buildings
{
    public class Building : MonoBehaviour, ISelectable, IBuildingInfo, IBuildingFunctionsContainer
    {
        private readonly List<IBuildingFunction> _buildingFunctions = new();
        private BuildingsController _buildingsController = null!;

        [SerializeField]
        private BuildingType _buildingType = BuildingType.None;

        [SerializeField]
        private BuildingInfo _buildingInfo = null!;

        [SerializeField]
        private SelectionBorder _selectionBorder = null!;

        public string Title => _buildingInfo.Title;
        public string Description => _buildingInfo.Description;
        public Sprite Image => _buildingInfo.Image;

        public BuildingType BuildingType => _buildingType;
        public IReadOnlyReactiveProperty<bool> IsSelected => _selectionBorder.IsSelected;
        public IReadOnlyList<IBuildingFunction> BuildingFunctions => _buildingFunctions;

        [Inject]
        private void Construct(BuildingsController buildingsController)
        {
            _buildingsController = buildingsController;
            _buildingFunctions.AddRange(GetComponents<IBuildingFunction>());
            _buildingsController.RegisterBuilding(this);
        }

        public void Select() => _selectionBorder.Select();
        public void Unselect() => _selectionBorder.Unselect();

        public T? GetBuildingFunction<T>() where T : IBuildingFunction
        {
            return (T?)_buildingFunctions.FirstOrDefault(buildingFunction => buildingFunction is T);
        }
    }
}