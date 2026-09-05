using UnityEngine;

namespace _Project.Scripts.Transitions
{
    public class AttackedTransition : Transition
    {
        [SerializeField] private EntityData _data;
        
        private void Update()
        {
            NeedTransit = _data.Owner;
        }
    }
}