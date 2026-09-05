using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.States
{
    public class DeadState : State
    {
        [SerializeField] private Entity _self;
        [SerializeField] private EntityData _data;
        
        public event UnityAction Died;
        
        private void OnEnable()
        {
            if(_data.IsDead)
                StartCoroutine(DieCoroutine());
        }
        
        private IEnumerator DieCoroutine()
        {
            //G.Instance.Wallet.AddMoneyWithEffect(GetReward(), transform.position);
            Died?.Invoke();
            yield return new WaitForSeconds(_data.DieDuration);
            _self.gameObject.SetActive(false);
            EventBus.OnEntityDied(_self);
        }
    }
}