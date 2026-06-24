using _Project.Scripts.Shop;
using _Project.Scripts.UI;
using UnityEngine;

namespace _Project.Scripts
{
    public class G : MonoBehaviour
    {
        public UIHandler UIHandler;
        public EntityContainer EntityContainer;
        public ShopHandler ShopHandler;
        public EntitySpawner Spawner;
        public Wallet Wallet;
        public UpgradesData UpgradesData; 
        
        public static G Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
    }
}