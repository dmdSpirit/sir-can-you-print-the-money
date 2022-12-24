#nullable enable
using System;
using NovemberProject.Core;
using UniRx;
using UnityEngine;

namespace NovemberProject.Buildings
{
    public sealed class ResourceProvider
    {
        private readonly ResourceProviderSettings _settings;
        private readonly FoodController _foodController;
        private readonly MoneyController _moneyController;

        public ResourceProvider(ResourceProviderSettings resourceProviderSettings, FoodController foodController,
            MoneyController moneyController)
        {
            _settings = resourceProviderSettings;
            _foodController = foodController;
            _moneyController = moneyController;
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
                _ => throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null)
            };
        }

        public Sprite GetResourceIcon(ResourceType resourceType)
        {
            if (resourceType == ResourceType.ArmyFood || resourceType == ResourceType.FolkFood)
            {
                return _settings.FoodIcon;
            }

            if (resourceType == ResourceType.ArmyMoney || resourceType == ResourceType.FolkMoney ||
                resourceType == ResourceType.GovernmentMoney)
            {
                return _settings.MoneyIcon;
            }

            throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
        }

        public string GetResourceTitle(ResourceType resourceType)
        {
            if (resourceType == ResourceType.ArmyFood || resourceType == ResourceType.FolkFood)
            {
                return _settings.FoodTitle;
            }

            if (resourceType == ResourceType.ArmyMoney || resourceType == ResourceType.FolkMoney ||
                resourceType == ResourceType.GovernmentMoney)
            {
                return _settings.MoneyTitle;
            }

            throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null);
        }
    }
}