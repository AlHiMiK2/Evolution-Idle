using UnityEngine;

namespace _Project.Scripts.States
{
    public class BunnyFindTargetState : State
    {
        [SerializeField] private Entity _self;
        [SerializeField] private BunnyData _data;

        private void Update()
        {
            if (G.Instance.Map.GetNearestGrassTile(_self.transform.position, out Vector3 target))
            {
                _data.TargetPosition = target;
            }
        }
    }
}