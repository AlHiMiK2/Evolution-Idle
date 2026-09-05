using UnityEngine;

namespace _Project.Scripts.Shop
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private Transform _entityContainer;
        [SerializeField] private Transform _percsContainer;
        [SerializeField] private ShopItemsDB _shopItemsDB;
        [SerializeField] private SpawnerItem _spawnerItemPrefab;
        [SerializeField] private PercItem _percItemPrefab;

        public ShopItemsDB ItemsDB => _shopItemsDB;
        
        private void OnEnable()
        {
            G.Instance.LevelHandler.LevelChanged += OnLevelChanged;
        }

        private void OnDisable()
        {
            G.Instance.LevelHandler.LevelChanged -= OnLevelChanged;
        }

        private void OnLevelChanged(int level)
        {
            AddItems(level);
        }

        private void Start()
        {
            AddItems(0);
        }

        private void AddItems(int level)
        {
            foreach (var config in _shopItemsDB.Configs)
            {
                if (config.UnlockLevel != level) continue;
                if (config is ShopEntityConfig entityConfig)
                {
                    EntitySpawner entitySpawner = null;
                    foreach (var spawner in G.Instance.Spawners)
                    {
                        if (spawner.Type == entityConfig.Type)
                        {
                            entitySpawner = spawner;
                            break;
                        }
                    }
                    SpawnerItem item = Instantiate(_spawnerItemPrefab, _entityContainer);
                    item.Init(config, entitySpawner);
                }
                else
                {
                    PercItem item = Instantiate(_percItemPrefab, _percsContainer);
                    item.Init(config);
                }
            }
        }
    }
}