﻿#nullable enable
using NovemberProject.CommonUIStuff;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace NovemberProject.System.UI
{
    public class MainMenu : UIElement
    {
        [SerializeField]
        private Button _newGame = null!;

        [SerializeField]
        private Button _exitGame = null!;

        [SerializeField]
        private Button _continue = null!;

        protected override void Initialize()
        {
            _newGame.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnNewGame);
            _exitGame.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnExitGame);
            _continue.OnClickAsObservable()
                .TakeUntilDisable(this)
                .Subscribe(OnExitGame);
        }

        private void OnNewGame(Unit _)
        {
            Game.Instance.NewGame();
        }

        private void OnExitGame(Unit _)
        {
            Game.Instance.ExitGame();
        }

        protected override void OnShow()
        {
            gameObject.SetActive(true);
        }

        protected override void OnHide()
        {
            gameObject.SetActive(false);
        }
    }
}