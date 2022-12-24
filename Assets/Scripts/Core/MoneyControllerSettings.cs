#nullable enable
using UnityEngine;

namespace NovemberProject.Core
{
    [CreateAssetMenu(menuName = "Create MoneyControllerSettings", fileName = "MoneyControllerSettings", order = 0)]
    public sealed class MoneyControllerSettings : ScriptableObject
    {
        [SerializeField]
        private int _governmentMoney = 100;

        [SerializeField]
        private int _armyMoney = 20;

        [SerializeField]
        private int _folkMoney = 20;

        public int GovernmentMoney => _governmentMoney;
        public int ArmyMoney => _armyMoney;
        public int FolkMoney => _folkMoney;

    }
}