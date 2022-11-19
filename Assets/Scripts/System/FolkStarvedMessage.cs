#nullable enable
namespace NovemberProject.System
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