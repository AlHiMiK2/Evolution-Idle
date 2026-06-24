using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Shop
{
    public static class ShopEventHandler
    {
        public static event UnityAction<ShopEntityConfig> EntityBought;
        public static event UnityAction FieldUpgraded;

        public static void OnEntityBought(ShopEntityConfig config) => EntityBought?.Invoke(config);
        public static void OnFieldUpgraded() => FieldUpgraded?.Invoke();
    }
}