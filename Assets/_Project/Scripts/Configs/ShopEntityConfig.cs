using UnityEngine;

namespace _Project.Scripts
{
    [CreateAssetMenu(fileName = "New Entity", menuName = "Create Shop Entity Config", order = 0)]
    public class ShopEntityConfig : ShopItemConfig
    {
        public Entity Prefab;
    }
}