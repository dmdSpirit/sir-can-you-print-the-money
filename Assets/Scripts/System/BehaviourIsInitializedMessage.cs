#nullable enable
using NovemberProject.CommonUIStuff;

namespace NovemberProject.System
{
    public class BehaviourIsInitializedMessage : IMessage
    {
        public readonly InitializableBehaviour InitializableBehaviour;

        public BehaviourIsInitializedMessage(InitializableBehaviour initializableBehaviour)
        {
            InitializableBehaviour = initializableBehaviour;
        }
    }
}