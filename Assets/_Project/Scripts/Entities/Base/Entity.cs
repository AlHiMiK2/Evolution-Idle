using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts
{
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField] private float _dieDuration;

        public int Price {get; private set;}
        public bool IsDead { get; protected set; }
        public float DieDuration => _dieDuration;
        public Entity Owner { get; protected set; }

        public event UnityAction Died;
        
        public void OnSpawned(int price)
        {
            Price = price;
            IsDead = false;
            OnReleased();
        }
        
        public abstract int GetReward();

        public void Die()
        {
            if (IsDead) return;
            IsDead = true;
            G.Instance.Wallet.AddMoney(Price);
            G.Instance.Wallet.AddMoneyWithEffect(GetReward(), transform.position);
            G.Instance.EntityContainer.RemoveEntity(this);
            Died?.Invoke();
            Destroy(gameObject, _dieDuration);
        }

        public abstract void OnAttacked(Entity owner);

        public abstract void OnReleased();
    }
}