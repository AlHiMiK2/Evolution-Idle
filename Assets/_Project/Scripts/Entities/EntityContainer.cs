using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public class EntityContainer : MonoBehaviour
    {
        private List<Entity> _plants = new List<Entity>();
        private List<Entity> _bunnies = new List<Entity>();
        private List<Entity> _foxies = new List<Entity>();
        private List<Entity> _wolfies = new List<Entity>();
        private List<Entity> _bears = new List<Entity>();
        private List<Entity> _hunters = new List<Entity>();

        private List<Entity> GetListByType(Entities type)
        {
            switch (type)
            {
                case Entities.Plant:
                    return _plants;
                case Entities.Bunny:
                    return _bunnies;
                case Entities.Fox:
                    return _foxies;
                case Entities.Wolf:
                    return _wolfies;
                case Entities.Bear:
                    return _bears;
                case Entities.Hunter:
                    return _hunters;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        public void Add(Entity entity, Entities type)
        {
            GetListByType(type).Add(entity);
        }

        public void Remove(Entity entity, Entities type)
        {
            GetListByType(type).Remove(entity);
        }

        public Entity GetNearest(Vector2 position, Entities type)
        {
            List<Entity> list = GetListByType(type);
            Entity nearest = null;
            float distance = Mathf.Infinity;
            
            foreach (var entity in list)
            {
                if (entity.Data.Owner || entity.Data.IsDead) continue;
                float newDistance = Vector2.Distance(position, entity.transform.position);
                if (newDistance < distance)
                {
                    nearest = entity;
                    distance = newDistance;
                }
            }
            
            return nearest;
        }

        public int GetLiveCount(Entities type) => GetListByType(type).FindAll(x => !x.CanRespawned).Count;
        public Entity GetDead(Entities type) => GetListByType(type).Find(x => x.CanRespawned);
    }
}