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
        private IDisposable? _workerCountSub;

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
            _workerCountSub?.Dispose();
            _workerManipulator = workerManipulator;
            _workerCountSub = _workerManipulator.WorkerCount.Subscribe(OnWorkerCountChanged);
            _title.text = _workerManipulator.WorkersTitle;
        }

        protected override void OnHide()
        {
            _workerCountSub?.Dispose();
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