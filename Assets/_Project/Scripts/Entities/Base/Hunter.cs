using System.Collections;
using UnityEngine;

namespace _Project.Scripts
{
    public class Hunter : Entity
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _stopDistance;
        [SerializeField] private float _huntTime;
        [SerializeField] private float _cooldown;
        [SerializeField] private float _viewDistance;
        [SerializeField] private float _rewardMultiplier;

        private Animal _target;
        private Vector2 _direction;
        private bool _isAttacking = false;
        private bool _isCooldown = false;
        
        public Vector2 Direction => _direction;
        public bool IsMoving {get; private set;}
        public bool IsAttacking => _isAttacking;
        
        private void Update()
        {
            IsMoving = false;
            if (_isAttacking || _isCooldown) return;
            if (!_target || _target.IsDead)
            {
                _target = GetTarget(); 
            }
            else
            {
                if (Vector2.Distance(transform.position, _target.transform.position) > _stopDistance)
                {
                    IsMoving = true;
                    Move();
                }
                else
                {
                    StartCoroutine(HuntCoroutine());
                }
            }
        }

        private Animal GetTarget()
        {
            Animal target = G.Instance.EntityContainer.GetNearestBear(transform.position, _viewDistance);
            if (target) return target;
            
            target = G.Instance.EntityContainer.GetNearestFox(transform.position, _viewDistance);
            if (target) return target;
            
            target = G.Instance.EntityContainer.GetNearestBunny(transform.position, _viewDistance);
            return target;
        }

        private IEnumerator HuntCoroutine()
        {
            _isAttacking = true;
            _target.OnAttacked(this);
            _direction = (_target.transform.position - transform.position).normalized;
            yield return new WaitForSeconds(_huntTime);
            G.Instance.Wallet.AddMoneyWithEffect((int)(_target.GetReward() * _rewardMultiplier), _target.transform.position);
            _target?.Die();
            _target = null;
            _isAttacking = false;
            _isCooldown = true;
            yield return new WaitForSeconds(_cooldown);
            _isCooldown = false;
        }

        private void Move()
        {
            _direction = (_target.transform.position - transform.position).normalized;
            float speed = _speed + (int)_speed;
            transform.Translate(_direction * (speed * Time.deltaTime), Space.World);
        }

        public override int GetReward()
        {
            return 0;
        }

        public override void OnAttacked(Entity danger)
        {
        }

        public override void OnReleased()
        {
        }
    }
}