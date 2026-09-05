using UnityEngine;

namespace _Project.Scripts.States
{
    public class AttackState : State
    {
        [SerializeField] private Entity _self;
        [SerializeField] private EntityData _data;

        private float _attackTime;

        private void OnEnable()
        {
            _data.IsAttacking = true;
            _data.Target.OnAttacked(_self);
        }

        private void Update()
        {
            _attackTime += Time.deltaTime;
            if (_attackTime >= _data.AttackDuration)
            {
                
                _data.Target.Die();
                _data.Target = null;
                _attackTime = 0;
            }
        }
    }
}