using UnityEngine;

namespace _Project.Scripts
{
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField] private EntityData _data;

        public EntityData Data => _data;
        
        public bool CanRespawned => _data.IsDead && !gameObject.activeSelf;
        
        public void OnSpawned()
        {
            _data.IsDead = false;
            gameObject.SetActive(true);
        }
        
        public void Die()
        {
            _data.IsDead = true;
        }

        public void OnAttacked(Entity owner)
        {
            _data.Owner = owner;
        }

        public void OnReleased()
        {
            _data.Owner = null;
        }
    }
}