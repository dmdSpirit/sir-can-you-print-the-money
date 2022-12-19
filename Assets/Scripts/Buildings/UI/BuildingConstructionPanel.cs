#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.CoreGameplay;
using NovemberProject.System;
using NovemberProject.TechTree;
using NovemberProject.Time;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Buildings.UI
{
    public sealed class BuildingConstructionPanel : UIElement<IConstructableBuilding>
    {
        private readonly CompositeDisposable _sub = new();

        private TimeSystem _timeSystem = null!;
        private TechController _techController = null!;
        private StoneController _stoneController = null!;
        private IConstructableBuilding _building = null!;

        [SerializeField]
        private TMP_Text _constructionTimerText = null!;

        [SerializeField]
        private GameObject _constructionTimerPanel = null!;

        [SerializeField]
        private Button _startConstructionButton = null!;

        [SerializeField]
        private Image _resourceImage = null!;

        [SerializeField]
        private TMP_Text _stoneCountText = null!;

        [SerializeField]
        private TMP_Text _constructButtonText = null!;

        [SerializeField]
        private string _constructText = "Build";

        [SerializeField]
        private string _lockedText = "Not learned";

        [Inject]
        private void Construct(TimeSystem timeSystem, TechController techController, StoneController stoneController)
        {
            _timeSystem = timeSystem;
            _techController = techController;
            _stoneController = stoneController;
        }

        private void Start()
        {
            _startConstructionButton.OnClickAsObservable()
                .Subscribe(OnStartConstructionClicked);
        }

        protected override void OnShow(IConstructableBuilding building)
        {
            _sub.Clear();
            _building = building;
            _building.ConstructableState.Subscribe(UpdateState).AddTo(_sub);
            _stoneController.Stone.Subscribe(OnStoneCountChanged).AddTo(_sub);
            _techController.CanBuildArena.Subscribe(OnCanBuildArenaChanged).AddTo(_sub);
        }

        protected override void OnHide()
        {
            _building = null!;
            _sub.Clear();
        }

        private void Update()
        {
            if (!IsConstructing())
            {
                return;
            }

            _constructionTimerText.text =
                _timeSystem.EstimateSecondsLeftUnscaled(_building.ConstructionTimer) + "s";

            bool IsConstructing() => IsShown && _building.ConstructableState.Value == ConstructableState.IsConstructing;
        }

        private void UpdateState(ConstructableState constructableState)
        {
            switch (constructableState)
            {
                case ConstructableState.NotConstructed:
                    ShowNotConstructedState();
                    break;
                case ConstructableState.IsConstructing:
                    ShowConstructingState();
                    break;
                case ConstructableState.Constructed:
                    Hide();
                    return;
            }
        }

        private void ShowNotConstructedState()
        {
            _constructionTimerPanel.SetActive(false);
            _stoneCountText.text =
                $"{_stoneController.Stone.Value}/{_building.ConstructionCost.ToString()}";
            _resourceImage.sprite = _building.ResourceImage;
        }

        private void ShowConstructingState()
        {
            _constructionTimerPanel.SetActive(true);
        }

        private void OnStartConstructionClicked(Unit _)
        {
            _building.StartConstruction();
        }

        private void OnStoneCountChanged(int stone)
        {
            _stoneCountText.text =
                $"{_stoneController.Stone.Value}/{_building.ConstructionCost.ToString()}";
            UpdateConstructionButton();
        }

        private void OnCanBuildArenaChanged(bool canBuild)
        {
            UpdateConstructionButton();
        }

        private void UpdateConstructionButton()
        {
            bool constructLearned = _techController.CanBuildArena.Value;
            _constructButtonText.text = constructLearned ? _constructText : _lockedText;
            _startConstructionButton.interactable =
                _stoneController.Stone.Value >= _building.ConstructionCost && constructLearned;
        }
    }
}