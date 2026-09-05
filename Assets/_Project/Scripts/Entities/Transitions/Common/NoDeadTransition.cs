using UnityEngine;

namespace _Project.Scripts.Transitions
{
    public class NoDeadTransition : Transition
    {
        [SerializeField] private EntityData _data;
        
        private void Update()
        {
            NeedTransit = !_data.IsDead;
        }
    }
}