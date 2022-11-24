#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using NovemberProject.System.Messages;
using UniRx;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace NovemberProject.Buildings
{
    public interface ISelectable
    {
        public IReadOnlyReactiveProperty<bool> IsSelected { get; }
        public void Select();
        public void Unselect();
    }

    public class Building : InitializableBehaviour, ISelectable
    {
        private readonly ReactiveProperty<bool> _isSelected = new();

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

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _selectionBorder.SetActive(false);
            if (Game.Instance.BuildingsController != null)
            {
                Game.Instance.BuildingsController.RegisterBuilding(this);
            }
            else
            {
                Game.Instance.MessageBroker.Receive<BehaviourIsInitializedMessage>()
                    .TakeUntilDisable(this)
                    .Where(message => message.InitializableBehaviour is BuildingsController)
                    .Subscribe(OnBuildingControllerInitialized);
            }
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

        private void OnBuildingControllerInitialized(BehaviourIsInitializedMessage message)
        {
            var controller = (BuildingsController)message.InitializableBehaviour;
            controller.RegisterBuilding(this);
        }
    }
}