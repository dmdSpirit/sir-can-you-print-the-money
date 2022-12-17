#nullable enable
using NovemberProject.System.Messages;

namespace NovemberProject.CoreGameplay
{
    public sealed class ArmyStarvedMessage : IMessage
    {
        public readonly int Count;

        public ArmyStarvedMessage(int count)
        {
            Count = count;
        }
    }
}