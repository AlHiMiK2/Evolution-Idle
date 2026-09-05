using UnityEngine;

namespace _Project.Scripts
{
    public class Bunny : Entity
    {
        [SerializeField] private float _rewardMultiplier;

        protected float GetSpeedMultiplier()
        {
            int level = G.Instance.UpgradesData.BunnySpeedLevel;
            return 1f + level * 0.05f;
        }

        //public double GetReward()
        //{
        //    return KillReward * (_rewardMultiplier + G.Instance.UpgradesData.BunnyRewardLevel * 0.25f);
        //}
    }
}