using UnityEngine;

namespace _Project.Scripts.Transitions
{
    public class BunnyAttackDistanceTransition : Transition
    {
        [SerializeField] private BunnyData _data;
        
        private void Update()
        {
            NeedTransit = Vector2.Distance(transform.position, _data.TargetPosition) <= _data.AttackDistance;
        }
    }
}