using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts
{
    public class EntityContainer : MonoBehaviour
    {
        private List<Plant> _plants = new List<Plant>();
        private List<Bunny> _bunnies = new List<Bunny>();
        private List<Fox> _foxies = new List<Fox>();
        private List<Bear> _bears = new List<Bear>();

        public void AddEntity(Entity entity)
        {
            if (entity is Plant plant)
            {
                _plants.Add(plant);
            }
            else if (entity is Bunny animal1)
            {
                _bunnies.Add(animal1);
            }
            else if (entity is Fox animal2)
            {
                _foxies.Add(animal2);
            }
            else if (entity is Bear animal3)
            {
                _bears.Add(animal3);
            }
        }

        public void RemoveEntity(Entity entity)
        {
            if (entity is Plant plant)
            {
                _plants.Remove(plant);
            }
            else if (entity is Bunny animal1)
            {
                _bunnies.Remove(animal1);
            }
            else if (entity is Fox animal2)
            {
                _foxies.Remove(animal2);
            }
            else if (entity is Bear animal3)
            {
                _bears.Remove(animal3);
            }
        }

        public Entity GetNearestPlant(Vector2 position)
        {
            Plant nearest = null;
            float distance = Mathf.Infinity;
            
            foreach (var plant in _plants)
            {
                if (plant.Owner || plant.IsDead) continue;
                float newDistance = Vector2.Distance(position, plant.transform.position);
                if (newDistance < distance)
                {
                    nearest = plant;
                    distance = newDistance;
                }
            }
            
            return nearest;
        }
        
        public Bunny GetNearestBunny(Vector2 position, float maxDistance)
        {
            Bunny nearest = null;
            float distance = Mathf.Infinity;
            
            foreach (var bunny in _bunnies)
            {
                if (bunny.Owner || bunny.IsDead) continue;
                float newDistance = Vector2.Distance(position, bunny.transform.position);
                if (newDistance < distance && newDistance <= maxDistance)
                {
                    nearest = bunny;
                    distance = newDistance;
                }
            }
            
            return nearest;
        }
        
        public Fox GetNearestFox(Vector2 position, float maxDistance)
        {
            Fox nearest = null;
            float distance = Mathf.Infinity;
            
            foreach (var fox in _foxies)
            {
                if (fox.Owner || fox.IsDead) continue;
                float newDistance = Vector2.Distance(position, fox.transform.position);
                if (newDistance < distance && newDistance <= maxDistance)
                {
                    nearest = fox;
                    distance = newDistance;
                }
            }
            
            return nearest;
        }
        
        public Bear GetNearestBear(Vector2 position, float maxDistance)
        {
            Bear nearest = null;
            float distance = Mathf.Infinity;
            
            foreach (var bear in _bears)
            {
                if (bear.Owner || bear.IsDead) continue;
                float newDistance = Vector2.Distance(position, bear.transform.position);
                if (newDistance < distance && newDistance <= maxDistance)
                {
                    nearest = bear;
                    distance = newDistance;
                }
            }
            
            return nearest;
        }
    }
}