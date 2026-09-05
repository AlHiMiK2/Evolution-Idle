using UnityEngine;

namespace _Project.Scripts.Transitions
{
    public class NoHasTargetTransition : Transition
    {
        [SerializeField] private EntityData _data;

        private void Update()
        {
            NeedTransit = !_data.Target;
        }
    }
}