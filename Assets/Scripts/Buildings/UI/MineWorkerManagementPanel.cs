#nullable enable
using NovemberProject.CommonUIStuff;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.Buildings.UI
{
    public sealed class MineWorkerManagementPanel : UIElement<IMineWorkerManipulator>
    {
        private IMineWorkerManipulator _mineWorkerManipulator = null!;
        private readonly CompositeDisposable _sub = new();

        [SerializeField]
        private Button _addWorkerButton = null!;

        [SerializeField]
        private Button _removeWorkerButton = null!;

        [SerializeField]
        private TMP_Text _numberOfWorkersText = null!;

        [SerializeField]
        private TMP_Text _title = null!;

        [SerializeField]
        private GameObject _notLearnedPanel = null!;

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

        protected override void OnShow(IMineWorkerManipulator mineWorkerManipulator)
        {
            _sub.Clear();
            _mineWorkerManipulator = mineWorkerManipulator;
            _mineWorkerManipulator.WorkerCount.Subscribe(OnWorkerCountChanged).AddTo(_sub);
            _mineWorkerManipulator.CanUseMine.Subscribe(OnCanUseMine).AddTo(_sub);
            _title.text = _mineWorkerManipulator.WorkersTitle;
        }

        protected override void OnHide()
        {
            _sub.Clear();
        }

        private void OnCanUseMine(bool canUseMine)
        {
            _notLearnedPanel.SetActive(!canUseMine);
        }

        private void OnWorkerCountChanged(int workerCount)
        {
            _numberOfWorkersText.text = workerCount.ToString();

            _addWorkerButton.interactable = _mineWorkerManipulator.CanAddWorker();
            _removeWorkerButton.interactable = _mineWorkerManipulator.CanRemoveWorker();
        }

        private void AddWorkerHandler(Unit _)
        {
            _mineWorkerManipulator.AddWorker();
        }

        private void RemoveWorkerHandler(Unit _)
        {
            _mineWorkerManipulator.RemoveWorker();
        }
    }
}