using UnityEngine;

namespace _Project.Scripts
{
    public class Plant : Entity
    {
        [SerializeField] private int _reward;

        public override int GetReward()
        {
            return _reward + G.Instance.UpgradesData.PlantRewardLevel;
        }

        public override void OnAttacked(Entity danger)
        {
            Owner = danger;
        }

        public override void OnReleased()
        {
            Owner = null;
        }

        private void OnMouseDown()
        {
            Die();
        }
    }
}