#nullable enable
using NovemberProject.System.Messages;

namespace NovemberProject.CoreGameplay.Messages
{
    public sealed class FolkStarvedMessage : IMessage
    {
        public readonly int NumberOfStarvedFolk;

        public FolkStarvedMessage(int numberOfStarvedFolk)
        {
            NumberOfStarvedFolk = numberOfStarvedFolk;
        }
    }
}