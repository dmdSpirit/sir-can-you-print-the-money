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

        private void Initialize()
        {
            OnInitialized();
            Game.Instance.MessageBroker.Publish(new BehaviourInitialized(this));
        }

        protected virtual void OnInitialized()
        {
        }
    }
}