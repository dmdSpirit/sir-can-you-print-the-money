#nullable enable
using NovemberProject.CommonUIStuff;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace NovemberProject.Buildings.UI
{
    public sealed class ExpeditionSenderPanel : UIElement<IExpeditionSender>
    {
        private IExpeditionSender _expeditionSender = null!;
        private readonly CompositeDisposable _panelSub = new();

        [SerializeField]
        private Button _addWorkerButton = null!;

        [SerializeField]
        private Button _removeWorkerButton = null!;

        [SerializeField]
        private TMP_Text _numberOfWorkersText = null!;

        [SerializeField]
        private Button _sendToExpeditionButton = null!;

        [SerializeField]
        private ExpeditionTimerPanel _expeditionTimerPanel = null!;

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
            _sendToExpeditionButton.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(SendExpeditionHandler);
        }

        protected override void OnShow(IExpeditionSender expeditionSender)
        {
            _panelSub.Clear();
            _expeditionSender = expeditionSender;
            _expeditionSender.WorkerCount.Subscribe(OnWorkerCountChanged).AddTo(_panelSub);
            _expeditionSender.PotentialWorkerCount.Subscribe(OnPotentialWorkerCountChanged).AddTo(_panelSub);
            _expeditionSender.IsExpeditionActive.Subscribe(OnExpeditionStatusChanged).AddTo(_panelSub);
            _expeditionSender.CanBeSentToExpedition.Subscribe(OnCanBeSentToExpeditionChanged).AddTo(_panelSub);
            _title.text = _expeditionSender.WorkersTitle;
        }

        protected override void OnHide()
        {
            _panelSub.Clear();
        }

        private void OnWorkerCountChanged(int workerCount)
        {
            if (_expeditionSender.HasMaxWorkerCount)
            {
                _numberOfWorkersText.text = $"{workerCount}/{_expeditionSender.MaxWorkerCount}";
            }
            else
            {
                _numberOfWorkersText.text = workerCount.ToString();
            }

            _addWorkerButton.interactable = _expeditionSender.CanAddWorker();
            _removeWorkerButton.interactable = _expeditionSender.CanRemoveWorker();
        }

        private void OnPotentialWorkerCountChanged(int potentialWorkerCount)
        {
            _addWorkerButton.interactable = _expeditionSender.CanAddWorker();
            _removeWorkerButton.interactable = _expeditionSender.CanRemoveWorker();
        }

        private void AddWorkerHandler(Unit _)
        {
            _expeditionSender.AddWorker();
        }

        private void RemoveWorkerHandler(Unit _)
        {
            _expeditionSender.RemoveWorker();
        }

        private void SendExpeditionHandler(Unit _)
        {
        }

        private void OnExpeditionStatusChanged(bool isExpeditionActive)
        {
            if (isExpeditionActive)
            {
                Assert.IsTrue(_expeditionSender.ExpeditionTimer != null);
                _expeditionTimerPanel.Show(_expeditionSender.ExpeditionTimer);
                return;
            }
            _expeditionTimerPanel.Hide();
        }

        private void OnCanBeSentToExpeditionChanged(bool canBeSentToExpedition)
        {
            _sendToExpeditionButton.interactable = canBeSentToExpedition;
        }
    }
}