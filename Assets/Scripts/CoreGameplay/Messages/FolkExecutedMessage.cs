#nullable enable
using NovemberProject.System.Messages;

namespace NovemberProject.CoreGameplay.Messages
{
    public sealed class FolkExecutedMessage : IMessage
    {
        public readonly int NumberOfExecutedFolk;

        public FolkExecutedMessage(int numberOfExecutedFolk)
        {
            NumberOfExecutedFolk = numberOfExecutedFolk;
        }
    }
}