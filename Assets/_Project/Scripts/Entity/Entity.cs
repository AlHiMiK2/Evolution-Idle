using System.Collections;
using UnityEngine;

namespace _Project.Scripts
{
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField] private float _dieDelay;

        protected bool IsDead;
        
        public int Reward { get; protected set; }
        public float DieDelay => _dieDelay;
        public bool IsAttacked { get; protected set; }

        public abstract void OnRespawned();

        public abstract void Die();
        
        protected bool TryDie()
        {
            if(IsDead) return false;
            IsDead = true;
            StartCoroutine(DieCoroutine());
            return true;
        }

        private IEnumerator DieCoroutine()
        {
            yield return new WaitForSeconds(DieDelay);
            G.Instance.EntityContainer.RemoveEntity(this);
            gameObject.SetActive(false);
            G.Instance.Spawner.OnDied(this);
        }

        public abstract void OnAttacked();

        public abstract void OnReleased();
    }
}