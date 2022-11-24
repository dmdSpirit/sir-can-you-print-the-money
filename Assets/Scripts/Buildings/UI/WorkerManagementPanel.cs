#nullable enable
using System;
using NovemberProject.CommonUIStuff;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Buildings.UI
{
    public sealed class WorkerManagementPanel : UIElement<IWorkerManipulator>
    {
        private IWorkerManipulator _workerManipulator = null!;
        private readonly CompositeDisposable _workerCountSub = new();

        [SerializeField]
        private Button _addWorkerButton = null!;

        [SerializeField]
        private Button _removeWorkerButton = null!;

        [SerializeField]
        private TMP_Text _numberOfWorkersText = null!;

        [SerializeField]
        private TMP_Text _title = null!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _addWorkerButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(AddWorkerHandler);
            _removeWorkerButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(RemoveWorkerHandler);
        }

        protected override void OnShow(IWorkerManipulator workerManipulator)
        {
            _workerCountSub.Clear();
            _workerManipulator = workerManipulator;
            _workerManipulator.WorkerCount.Subscribe(OnWorkerCountChanged).AddTo(_workerCountSub);
            _workerManipulator.PotentialWorkerCount.Subscribe(OnPotentialWorkerCountChanged).AddTo(_workerCountSub);
            _title.text = _workerManipulator.WorkersTitle;
        }

        protected override void OnHide()
        {
            _workerCountSub.Clear();
        }

        private void OnWorkerCountChanged(int workerCount)
        {
            if (_workerManipulator.HasMaxWorkerCount)
            {
                _numberOfWorkersText.text = $"{workerCount}/{_workerManipulator.MaxWorkerCount}";
            }
            else
            {
                _numberOfWorkersText.text = workerCount.ToString();
            }

            _addWorkerButton.interactable = _workerManipulator.CanAddWorker();
            _removeWorkerButton.interactable = _workerManipulator.CanRemoveWorker();
        }

        private void OnPotentialWorkerCountChanged(int potentialWorkerCount)
        {
            _addWorkerButton.interactable = _workerManipulator.CanAddWorker();
            _removeWorkerButton.interactable = _workerManipulator.CanRemoveWorker();
        }

        private void AddWorkerHandler(Unit _)
        {
            _workerManipulator.AddWorker();
        }

        private void RemoveWorkerHandler(Unit _)
        {
            _workerManipulator.RemoveWorker();
        }
    }
}