using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts
{
    public class Plant : Entity
    {
        [SerializeField] private int _reward;
        [SerializeField] private int _dieReward;
        [SerializeField] private float _rewardRate;

        private float _lastRewardTime;

        public event UnityAction Died;
        public event UnityAction Respawned;

        private void Start()
        {
            Reward = _reward;
        }

        private void Update()
        {
            if(IsDead) return;
            if (_lastRewardTime + _rewardRate < Time.time)
            {
                G.Instance.Wallet.AddMoneyWithEffect(_reward, transform.position);
                _lastRewardTime = Time.time;
            }
        }

        public override void OnRespawned()
        {
            _lastRewardTime = Time.time;
            IsDead = false;
            OnReleased();
            Respawned?.Invoke();
        }

        public override void Die()
        {
            if (TryDie())
            {
                G.Instance.Wallet.AddMoneyWithEffect(_dieReward, transform.position);
                Died?.Invoke();
            }
        }

        public override void OnAttacked()
        {
            IsAttacked = true;
        }

        public override void OnReleased()
        {
            IsAttacked = false;
        }

        private void OnMouseDown()
        {
            Die();
        }
    }
}