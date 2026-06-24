using UnityEngine;

namespace _Project.Scripts
{
    public class Bear : Animal
    {
        [SerializeField] private float _viewDistance;
        [SerializeField] private int _rewardMultiplier;
        
        protected override Entity GetTarget()
        {
            return G.Instance.EntityContainer.GetNearestFox(transform.position, _viewDistance);
        }

        protected override float GetSpeedMultiplier()
        {
            int level = G.Instance.UpgradesData.BearSpeedLevel;
            return 1f + level * 0.05f;
        }
        
        public override int GetReward()
        {
            return (int)(KillReward * (_rewardMultiplier + G.Instance.UpgradesData.BearRewardLevel * 0.25f));
        }
    }
}