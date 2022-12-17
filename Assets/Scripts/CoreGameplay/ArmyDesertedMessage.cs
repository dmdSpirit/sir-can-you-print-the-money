#nullable enable
using NovemberProject.System.Messages;

namespace NovemberProject.CoreGameplay
{
    public sealed class ArmyDesertedMessage : IMessage
    {
        public readonly int Count;

        public ArmyDesertedMessage(int count)
        {
            Count = count;
        }
    }
}