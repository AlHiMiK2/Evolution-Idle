using UnityEngine;

namespace _Project.Scripts.Transitions
{
    public class ReleaseTransition : Transition
    {
        [SerializeField] private EntityData _data;
        
        private void Update()
        {
            NeedTransit = !_data.Owner;
        }
    }
}