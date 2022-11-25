#nullable enable
using UniRx;

namespace NovemberProject.TechTree
{
    public class TechTreeNode
    {
        private readonly TechTreeNode[] _requiredNodes;
        private readonly IUnlockableAbility _unlockableAbility;

        public IReadOnlyReactiveProperty<bool> IsUnlocked => _unlockableAbility.IsUnlocked;
        public int UnlockCost { get; }
        public TechType TechType { get; }

        public TechTreeNode(TechTreeNode[] requiredNodes, IUnlockableAbility unlockableAbility, int unlockCost, TechType techType)
        {
            _requiredNodes = requiredNodes;
            _unlockableAbility = unlockableAbility;
            TechType = techType;
            UnlockCost = unlockCost;
        }

        public bool CanBeUnlocked()
        {
            if (_requiredNodes.Length == 0)
            {
                return true;
            }

            foreach (TechTreeNode techTreeNode in _requiredNodes)
            {
                if (!techTreeNode.IsUnlocked.Value)
                {
                    return false;
                }
            }

            return true;
        }
    }
}