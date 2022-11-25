#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.TechTree
{
    public class TechTreeNodePanel : UIElement<object?>
    {
        private TechTreeNode _techTreeNode = null!;
        private readonly CompositeDisposable _sub = new();

        [SerializeField]
        private Button _button = null!;

        [SerializeField]
        private GameObject _unlockedState = null!;

        [SerializeField]
        private GameObject _lockedState = null!;

        [SerializeField]
        private TechType _techType;

        public TechType TechType => _techType;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _button.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnButtonClicked);
            Game.Instance.TechController.RegisterTechNodePanel(this);
        }

        protected override void OnShow(object? _)
        {
            _sub.Clear();
            _techTreeNode.IsUnlocked.Subscribe(_ => UpdateButton()).AddTo(_sub);
            Game.Instance.TreasureController.Treasures.Subscribe(_ => UpdateButton()).AddTo(_sub);
        }

        protected override void OnHide()
        {
            _sub.Clear();
        }

        public void SetTechNode(TechTreeNode techTreeNode)
        {
            _techTreeNode = techTreeNode;
        }

        private void UpdateButton()
        {
            bool isUnlocked = _techTreeNode.IsUnlocked.Value;
            _unlockedState.SetActive(isUnlocked);
            _lockedState.SetActive(!isUnlocked);
            if (!isUnlocked)
            {
                _button.interactable = Game.Instance.TreasureController.Treasures.Value >= _techTreeNode.UnlockCost;
            }
        }

        private void OnButtonClicked(Unit unit)
        {
        }
    }
}