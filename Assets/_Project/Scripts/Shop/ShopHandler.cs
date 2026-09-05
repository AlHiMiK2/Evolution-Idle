using System;
using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.Shop;
using UnityEngine;

public class ShopHandler : MonoBehaviour
{
    [SerializeField] private Shop _shop;
    
    private int[] _buys;

    private void Awake()
    {
        _buys = new int[_shop.ItemsDB.Configs.Length];
    }

    public bool TryBuy(ShopItemConfig config)
    {
        if (config.IsSingle && GetItemBuyCount(config) > 0) return false;
        double price = GetItemPrice(config);
        if (G.Instance.Wallet.TryTakeMoney(price))
        {
            _buys[config.Id]++;
            HandleBuy(config);
            return true;
        }
        
        return false;
    }

    public double GetItemPrice(ShopItemConfig config)
    {
        return config.Price * Mathf.Pow(config.PriceMultiplier, GetItemBuyCount(config));
    }
    
    public int GetItemBuyCount(ShopItemConfig config)
    {
        return _buys[config.Id];
    }
    
    private void HandleBuy(ShopItemConfig config)
    {
        UpgradesData data = G.Instance.UpgradesData;
        if (config is ShopEntityConfig entityConfig)
        {
            switch (entityConfig.Type)
            {
                case Entities.Plant:
                    data.PlantSpawnCount++;
                    break;
                case Entities.Bunny:
                    data.BunnySpawnCount++;
                    break;
                case Entities.Fox:
                    data.FoxSpawnCount++;
                    break;
                case Entities.Wolf:
                    data.WolfSpawnCount++;
                    break;
                case Entities.Bear:
                    data.BearSpawnCount++;
                    break;
                case Entities.Hunter:
                    data.HunterSpawnCount++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else if (config is ShopPercConfig percConfig)
        {
            switch (percConfig.Type)
            {
                case PercType.PlantReward:
                    data.PlantRewardLevel++;
                    break;
                case PercType.BunnyReward:
                    data.BunnyRewardLevel++;
                    break;
                case PercType.FoxReward:
                    data.FoxRewardLevel++;
                    break;
                case PercType.WolfReward:
                    data.WolfRewardLevel++;
                    break;
                case PercType.BearReward:
                    data.BearRewardLevel++;
                    break;
                case PercType.HunterMultiplier:
                    break;
                case PercType.BunnySpeed:
                    data.BunnySpeedLevel++;
                    break;
                case PercType.FoxSpeed:
                    data.FoxSpeedLevel++;
                    break;
                case PercType.WolfSpeed:
                    data.WolfSpeedLevel++;
                    break;
                case PercType.BearSpeed:
                    data.BearSpeedLevel++;
                    break;
            }
        }
    }
}
