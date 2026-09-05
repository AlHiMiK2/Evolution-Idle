using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.States
{
    public class FindTargetState : State
    {
        [SerializeField] private EntityData _data;
        [SerializeField] private Entities[] _targetTypes;

        private void Update()
        {
            List<Entity> targets = new List<Entity>();
            
            foreach (var type in _targetTypes)
            {
                Entity entity = G.Instance.EntityContainer.GetNearest(transform.position, type);
                
                if (entity)
                {
                    targets.Add(entity);
                }
            }
    
            if(targets.Count > 0)
                _data.Target = targets[Random.Range(0, targets.Count)];
        }
    }
}