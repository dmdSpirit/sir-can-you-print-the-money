#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.Money;
using NovemberProject.System;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class Building : InitializableBehaviour
    {
        [SerializeField]
        private string _title = null!;

        [SerializeField]
        private string _description = null!;

        [SerializeField]
        private Sprite _image = null!;

        [SerializeField]
        private BuildingType _buildingType;

        public string Title => _title;
        public string Description => _description;
        public Sprite Image => _image;
        public BuildingType BuildingType => _buildingType;

        protected override void OnInitialized()
        {
            base.OnInitialized();
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

        private void OnBuildingControllerInitialized(BehaviourIsInitializedMessage message)
        {
            var controller = (BuildingsController)message.InitializableBehaviour;
            controller.RegisterBuilding(this);
        }
    }
}