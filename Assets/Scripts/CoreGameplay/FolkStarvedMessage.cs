#nullable enable
using NovemberProject.System.Messages;

namespace NovemberProject.CoreGameplay
{
    public sealed class FolkStarvedMessage : IMessage
    {
        public readonly int Count;

        public FolkStarvedMessage(int count)
        {
            Count = count;
        }
    }
}