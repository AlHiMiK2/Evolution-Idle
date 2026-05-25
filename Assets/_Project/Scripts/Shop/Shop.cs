using UnityEngine;

namespace _Project.Scripts.Shop
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private Transform _entityContainer;
        [SerializeField] private Transform _percsContainer;
        [SerializeField] private ShopItemsDB _shopItemsDB;
        [SerializeField] private ShopItem _itemPrefab;

        private void Start()
        {
            foreach (var config in _shopItemsDB.Configs)
            {
                if(config as ShopEntityConfig)
                {
                    ShopItem item = Instantiate(_itemPrefab, _entityContainer);
                    item.Init(config);
                }
                else
                {
                    ShopItem item = Instantiate(_itemPrefab, _percsContainer);
                    item.Init(config);
                }
            }
        }
    }
}