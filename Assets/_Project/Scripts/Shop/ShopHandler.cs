using System.Collections.Generic;
using _Project.Scripts;
using UnityEngine;

public class ShopHandler : MonoBehaviour
{
    private List<int> _buyHistory = new List<int>();
    
    public bool TryBuy(ShopItemConfig config)
    {
        if (GetItemBuyCount(config) >= config.MaxCount) return false;
        int price = GetItemPrice(config);
        if (G.Instance.Wallet.TryTakeMoney(price))
        {
            _buyHistory.Add(config.Id);
            if(config is ShopEntityConfig entityConfig)
            {
                G.Instance.Spawner.Spawn(entityConfig);
            }
            else if (config is ShopPercConfig percConfig)
            {
                
            }
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
}
