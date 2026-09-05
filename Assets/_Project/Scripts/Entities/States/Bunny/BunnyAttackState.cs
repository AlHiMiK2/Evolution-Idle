using UnityEngine;

namespace _Project.Scripts.States
{
    public class BunnyAttackState : State
    {
        [SerializeField] private Entity _self;
        [SerializeField] private BunnyData _data;

        private float _attackTime;

        private void OnEnable()
        {
            _data.IsAttacking = true;
        }

        private void Update()
        {
            _attackTime += Time.deltaTime;
            if (_attackTime >= _data.AttackDuration)
            {
                G.Instance.Map.EatTile(_data.TargetPosition);
                _attackTime = 0;
            }
        }
    }
}