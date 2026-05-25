using UnityEngine;

namespace _Project.Scripts
{
    [CreateAssetMenu(fileName = "New Shop Items DB", menuName = "Create Shop Items DB", order = 0)]
    public class ShopItemsDB : ScriptableObject
    {
        [SerializeField] private ShopItemConfig[] _configs;
        
        public ShopItemConfig[] Configs => _configs;
    }
}