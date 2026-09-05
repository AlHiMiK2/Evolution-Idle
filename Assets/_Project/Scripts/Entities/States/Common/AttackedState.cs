using UnityEngine;

namespace _Project.Scripts.States
{
    public class AttackedState : State
    {
        [SerializeField] private EntityData _data;
        
        private void OnEnable()
        {
            _data.IsAttacking = false;
            _data.Target?.OnReleased();
        }
    }
}