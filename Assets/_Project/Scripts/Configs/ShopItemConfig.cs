using UnityEngine;

namespace _Project.Scripts
{
    public class ShopItemConfig : ScriptableObject
    {
        public string Title;
        public double Price;
        public Sprite Icon;
        public float PriceMultiplier = 1.15f;
        public int Id;
        public int UnlockLevel;
        public bool IsSingle;
    }
}