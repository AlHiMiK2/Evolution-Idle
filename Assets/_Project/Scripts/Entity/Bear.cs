using UnityEngine;

namespace _Project.Scripts
{
    public class Bear : Animal
    {
        protected override Entity GetTarget()
        {
            return G.Instance.EntityContainer.GetNearestFox(transform.position);
        }
    }
}