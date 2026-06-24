using UnityEngine;

namespace _Project.Scripts
{
    public abstract class Animal : Entity
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _stopDistance;
        [SerializeField] private float _attackDuration;

        private float _attackTime;
        private Entity _target;
        private Vector2 _direction;
        private bool _isAttacking = false;
        
        public Vector2 Direction => _direction;
        public bool IsAttacking => _isAttacking;
        public int KillReward {get; private set;}
        
        private void Update()
        {
            if (Owner || IsDead) return;
            if (_target && !_target.IsDead && (!_target.Owner || _target.Owner == this))
            {
                if (Vector2.Distance(transform.position, _target.transform.position) > _stopDistance)
                {
                    Move();
                }
                else if (!IsAttacking)
                {
                    _isAttacking = true;
                    _target.OnAttacked(this);
                }
                if (_isAttacking)
                {
                    Attack();
                }
            }
            else
            {
                _target = GetTarget(); 
                _direction = Vector2.zero;
                _isAttacking = false;
            }
        }

        protected abstract Entity GetTarget();
        
        private void Attack()
        {
            _attackTime += Time.deltaTime;
            if (_attackTime >= _attackDuration)
            {
                KillReward += _target.GetReward();
                _target.Die();
                _target = null;
                _isAttacking = false;
                _attackTime = 0;
            }
        }

        private void Move()
        {
            _direction = (_target.transform.position - transform.position).normalized;
            float speed = _speed * GetSpeedMultiplier();
            transform.Translate(_direction * (speed * Time.deltaTime), Space.World);
        }

        protected abstract float GetSpeedMultiplier();

        public override void OnAttacked(Entity owner)
        {
            Owner = owner;
            _isAttacking = false;
            _target?.OnReleased();
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