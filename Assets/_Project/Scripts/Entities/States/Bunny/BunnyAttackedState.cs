using UnityEngine;

namespace _Project.Scripts.States
{
    public class BunnyAttackedState : State
    {
        [SerializeField] private BunnyData _data;
        
        private void OnEnable()
        {
            _data.IsAttacking = false;
        }
    }
}