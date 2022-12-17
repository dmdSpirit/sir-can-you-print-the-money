#nullable enable
using NovemberProject.System.Messages;

namespace NovemberProject.CoreGameplay
{
    public sealed class FolkExecutedMessage : IMessage
    {
        public readonly int Count;

        public FolkExecutedMessage(int count)
        {
            Count = count;
        }
    }
}