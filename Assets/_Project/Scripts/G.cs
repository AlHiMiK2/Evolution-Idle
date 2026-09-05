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
        public Wallet Wallet;
        public UpgradesData UpgradesData; 
        public Zone Zone;
        public Map Map;
        public EntitySpawner[] Spawners;
        public LevelHandler LevelHandler;
        
        public static G Instance { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Instance = FindAnyObjectByType<G>();
        }
    }
}