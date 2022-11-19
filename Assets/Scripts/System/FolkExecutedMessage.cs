#nullable enable
namespace NovemberProject.System
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