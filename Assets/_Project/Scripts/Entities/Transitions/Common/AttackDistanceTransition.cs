using UnityEngine;

namespace _Project.Scripts.Transitions
{
    public class AttackDistanceTransition : Transition
    {
        [SerializeField] private EntityData _data;
        
        private void Update()
        {
            NeedTransit = Vector2.Distance(transform.position, _data.Target.transform.position) <= _data.AttackDistance;
        }
    }
}