#nullable enable
using NovemberProject.CommonUIStuff;

namespace NovemberProject.System
{
    public class BehaviourInitialized : IMessage
    {
        public readonly InitializableBehaviour InitializableBehaviour;

        public BehaviourInitialized(InitializableBehaviour initializableBehaviour)
        {
            InitializableBehaviour = initializableBehaviour;
        }
    }
}