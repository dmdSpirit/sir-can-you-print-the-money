#nullable enable
using System;
using NovemberProject.Core;
using NovemberProject.Treasures;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class ResourceProvider
    {
        private readonly ResourceProviderSettings _settings;
        private readonly FoodController _foodController;
        private readonly MoneyController _moneyController;
        private readonly StoneController _stoneController;
        private readonly TreasureController _treasureController;

        public ResourceProvider(ResourceProviderSettings resourceProviderSettings, FoodController foodController,
            MoneyController moneyController, StoneController stoneController, TreasureController treasureController)
        {
            _settings = resourceProviderSettings;
            _foodController = foodController;
            _moneyController = moneyController;
            _stoneController = stoneController;
            _treasureController = treasureController;
        }

        public IReadOnlyReactiveProperty<int> GetResourceQuantity(ResourceType resourceType)
        {
            return resourceType switch
            {
                ResourceType.ArmyFood => _foodController.ArmyFood,
                ResourceType.FolkFood => _foodController.FolkFood,
                ResourceType.ArmyMoney => _moneyController.ArmyMoney,
                ResourceType.FolkMoney => _moneyController.FolkMoney,
                ResourceType.GovernmentMoney => _moneyController.GovernmentMoney,
                ResourceType.Stone => _stoneController.Stone,
                ResourceType.Treasure => _treasureController.Treasures,
                _ => throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null)
            };
        }

        public Sprite GetResourceIcon(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.ArmyFood:
                case ResourceType.FolkFood:
                    return _settings.FoodIcon;
                case ResourceType.ArmyMoney:
                case ResourceType.FolkMoney:
                case ResourceType.GovernmentMoney:
                    return _settings.MoneyIcon;
                case ResourceType.Stone:
                    return _settings.StoneIcon;
                case ResourceType.Treasure:
                    return _settings.TreasureIcon;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
            }
        }

        public string GetResourceTitle(ResourceType resourceType)
        {
            switch (resourceType)
            {
                case ResourceType.ArmyFood:
                case ResourceType.FolkFood:
                    return _settings.FoodTitle;
                case ResourceType.ArmyMoney:
                case ResourceType.FolkMoney:
                case ResourceType.GovernmentMoney:
                    return _settings.MoneyTitle;
                case ResourceType.Stone:
                    return _settings.StoneTitle;
                case ResourceType.Treasure:
                    return _settings.TreasureTitle;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
            }
        }
    }
}