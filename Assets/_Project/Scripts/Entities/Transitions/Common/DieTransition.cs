using UnityEngine;

namespace _Project.Scripts.Transitions
{
    public class DieTransition : Transition
    {
        [SerializeField] private EntityData _data;
        
        private void Update()
        {
            NeedTransit = _data.IsDead;
        }
    }
}