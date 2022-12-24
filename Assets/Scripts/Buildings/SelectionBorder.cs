#nullable enable
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class SelectionBorder : MonoBehaviour, ISelectable
    {
        private readonly ReactiveProperty<bool> _isSelected = new();

        [SerializeField]
        private GameObject _borderObject = null!;

        public IReadOnlyReactiveProperty<bool> IsSelected => _isSelected;

        private void Start()
        {
            _borderObject.SetActive(false);
        }

        public void Select()
        {
            _isSelected.Value = true;
            _borderObject.SetActive(true);
        }

        public void Unselect()
        {
            _isSelected.Value = false;
            _borderObject.SetActive(false);
        }
    }
}