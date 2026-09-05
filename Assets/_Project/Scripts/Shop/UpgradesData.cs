using UnityEngine;

namespace _Project.Scripts.Shop
{
    public class UpgradesData : MonoBehaviour
    {
        [Header("Reward")]
        public int PlantRewardLevel;
        public int BunnyRewardLevel;
        public int FoxRewardLevel;
        public int WolfRewardLevel;
        public int BearRewardLevel;
        [Header("Speed")] 
        public int BunnySpeedLevel;
        public int FoxSpeedLevel;
        public int WolfSpeedLevel;
        public int BearSpeedLevel;
        [Header("SpawnCount")] 
        public int PlantSpawnCount;
        public int BunnySpawnCount;
        public int FoxSpawnCount;
        public int WolfSpawnCount;
        public int BearSpawnCount;
        public int HunterSpawnCount;
        [Header("SpawnRate")] 
        public float PlantSpawnRate;
        public float BunnySpawnRate;
        public float FoxSpawnRate;
        public float WolfSpawnRate;
        public float BearSpawnRate;
        public float HunterSpawnRate;
    }
}