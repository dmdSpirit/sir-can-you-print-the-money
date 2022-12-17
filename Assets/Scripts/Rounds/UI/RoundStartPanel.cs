#nullable enable
using NovemberProject.CommonUIStuff;
using NovemberProject.CoreGameplay;
using NovemberProject.GameStates;
using NovemberProject.System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace NovemberProject.Rounds.UI
{
    public sealed class RoundStartPanel : UIElement<object?>
    {
        private GameStateMachine _gameStateMachine = null!;

        [SerializeField]
        private TMP_Text _title = null!;

        [SerializeField]
        private TMP_Text _description = null!;

        [SerializeField]
        private Image _image = null!;

        [SerializeField]
        private Button _nextRound = null!;

        [SerializeField]
        private RoundStartConfigs _roundStartConfigs = null!;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Start()
        {
            _nextRound.OnClickAsObservable()
                .Subscribe(OnNextRound);
        }

        protected override void OnShow(object? _)
        {
            int round = Game.Instance.RoundSystem.Round.Value;
            RoundStartConfig config = _roundStartConfigs.GetRoundStartConfig(round);
            _title.text = config.Title;
            _description.text = config.Description;
            _image.sprite = config.Image;
        }

        protected override void OnHide()
        {
        }

        private void OnNextRound(Unit _)
        {
            _gameStateMachine.Round();
        }
    }
}