using UnityEngine;

namespace _Project.Scripts.Transitions
{
    public class BunnyHasTargetTransition : Transition
    {
        [SerializeField] private BunnyData _data;

        private void Update()
        {
            NeedTransit = G.Instance.Map.HasGrassTileAt(_data.TargetPosition);
        }
    }
}