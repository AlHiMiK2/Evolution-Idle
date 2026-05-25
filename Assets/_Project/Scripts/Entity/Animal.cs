using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts
{
    public abstract class Animal : Entity
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _stopDistance;
        [SerializeField] private float _eatTime;
        [SerializeField] private int _killRewardMultiplier;
        [SerializeField] private int _minKillCount = 1;

        private int _killCount;
        private Entity _target;
        private Vector2 _direction;
        private bool _isAttacking = false;
        private Coroutine _eatCoroutine;
        
        public Vector2 Direction => _direction;
        public bool IsMoving {get; private set;}
        public bool IsAttacking => _isAttacking;
        public bool IsPrey => _minKillCount <= _killCount;
        public int KillCount => _killCount;
        
        public event UnityAction Died;
        public event UnityAction Respawned;
        
        private void Update()
        {
            IsMoving = false;
            if (IsAttacked || IsDead) return;
            if (_target == null || !_target.gameObject.activeSelf)
            {
                _target = GetTarget();
            }
            else if (!_target.IsAttacked)
            {
                if (Vector2.Distance(transform.position, _target.transform.position) > _stopDistance)
                {
                    IsMoving = true;
                    Move();
                }
                else
                {
                    _eatCoroutine = StartCoroutine(EatCoroutine());
                }
            }
            else if (_target.IsAttacked && !_isAttacking)
            {
                _target = GetTarget();
            }
        }

        protected abstract Entity GetTarget();

        private IEnumerator EatCoroutine()
        {
            _isAttacking = true;
            _target.OnAttacked();
            yield return new WaitForSeconds(_eatTime);
            _killCount++;
            Reward += _target.Reward * _killRewardMultiplier;
            _target?.Die();
            _target = null;
            _isAttacking = false;
        }

        private void Move()
        {
            _direction = (_target.transform.position - transform.position).normalized;
            transform.Translate(_direction * (_speed * Time.deltaTime), Space.World);
        }

        public override void OnRespawned()
        {
            IsDead = false;
            OnReleased();
            Respawned?.Invoke();
        }

        public override void OnAttacked()
        {
            IsAttacked = true;
            _isAttacking = false;
            _target?.OnReleased();
            if(_eatCoroutine != null)
                StopCoroutine(_eatCoroutine);
        }

        public override void OnReleased()
        {
            IsAttacked = false;
        }

        public override void Die()
        {
            if (TryDie())
            {
                G.Instance.Wallet.AddMoneyWithEffect(Reward, transform.position);
                Reward = 0;
                _killCount = 0;
                Died?.Invoke();
            }
        }
        
        private void OnMouseDown()
        {
            Die();
        }
    }
}