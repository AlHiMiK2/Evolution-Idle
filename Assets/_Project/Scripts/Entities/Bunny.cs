using UnityEngine;

namespace _Project.Scripts
{
    public class Bunny : Animal
    {
        [SerializeField] private int _rewardMultiplier;
        
        protected override Entity GetTarget()
        {
            return G.Instance.EntityContainer.GetNearestPlant(transform.position);
        }

        protected override float GetSpeedMultiplier()
        {
            int level = G.Instance.UpgradesData.BunnySpeedLevel;
            return 1f + level * 0.05f;
        }

        public override int GetReward()
        {
            return (int)(KillReward * (_rewardMultiplier + G.Instance.UpgradesData.BunnyRewardLevel * 0.25f));
        }
    }
}