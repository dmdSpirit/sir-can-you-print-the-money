#nullable enable
using NovemberProject.System;
using UniRx;
using UnityEngine;

namespace NovemberProject.CommonUIStuff
{
    public abstract class InitializableBehaviour : MonoBehaviour
    {
        protected void OnEnable()
        {
            if (Game.Instance.IsInitialized)
            {
                Initialize();
                return;
            }

            Game.Instance.OnInitialized
                .TakeUntilDisable(this)
                .Subscribe(_ => Initialize());
        }

        protected virtual void OnInitialized()
        {
        }

        private void Initialize()
        {
            OnInitialized();
        }
    }
}