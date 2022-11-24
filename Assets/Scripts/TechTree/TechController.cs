#nullable enable
using System;
using System.Collections.Generic;
using NovemberProject.Buildings;
using NovemberProject.CommonUIStuff;
using NovemberProject.System;
using UnityEngine;

namespace NovemberProject.TechTree
{
    public class TechController : InitializableBehaviour
    {
        private readonly Dictionary<TechType, TechTreeNode> _techTreeNodes = new();
        private readonly Dictionary<TechType, TechTreeNodePanel> _techTreeNodePanels = new();

        [SerializeField]
        private int _unlockSalaryCost = 1;

        public void InitializeGameData()
        {
            InitializeTechNodes();
        }

        public void RegisterTechNodePanel(TechTreeNodePanel techTreeNodePanel)
        {
            if (techTreeNodePanel.TechType == TechType.None ||
                _techTreeNodePanels.ContainsKey(techTreeNodePanel.TechType))
            {
                return;
            }

            _techTreeNodePanels.Add(techTreeNodePanel.TechType, techTreeNodePanel);
            techTreeNodePanel.SetTechNode(_techTreeNodes[techTreeNodePanel.TechType]);
        }

        private void InitializeTechNodes()
        {
            _techTreeNodes.Add(TechType.Salary, SalaryNode());
        }

        private TechTreeNode SalaryNode()
        {
            var treasuryBuilding = Game.Instance.BuildingsController.GetBuilding<GovernmentTreasuryBuilding>();
            var salaryNode = new TechTreeNode(Array.Empty<TechTreeNode>(),
                treasuryBuilding.ChangeSalaryAbility, _unlockSalaryCost, TechType.Salary);
            return salaryNode;
        }
    }
}