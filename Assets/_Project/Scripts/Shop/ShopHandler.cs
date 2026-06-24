using System.Collections.Generic;
using _Project.Scripts;
using _Project.Scripts.Shop;
using _Project.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;

public class ShopHandler : MonoBehaviour
{
    private List<int> _buyHistory = new List<int>();
    
    public bool TryBuy(ShopItemConfig config)
    {
        if (config.IsSingle && GetItemBuyCount(config) > 0) return false;
        int price = GetItemPrice(config);
        if (G.Instance.Wallet.TryTakeMoney(price))
        {
            _buyHistory.Add(config.Id);
            HandleBuy(config);
            return true;
        }
        
        return false;
    }

    public int GetItemPrice(ShopItemConfig config)
    {
        return (int)(config.Price * Mathf.Pow(config.PriceMultiplier, GetItemBuyCount(config)));
    }
    
    public int GetItemBuyCount(ShopItemConfig config)
    {
        int id = config.Id;
        int count = 0;

        foreach (var buy in _buyHistory)
        {
            if (buy == id)
                count++;
        }
        
        return count;
    }
    
    private void HandleBuy(ShopItemConfig config)
    {
        if (config is ShopEntityConfig entityConfig)
        {
            ShopEventHandler.OnEntityBought(entityConfig);
        }
        else if (config is ShopPercConfig percConfig)
        {
            UpgradesData data = G.Instance.UpgradesData;
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
                case PercType.BearSpeed:
                    data.BearSpeedLevel++;
                    break;
            }
        }
    }
}
