using UnityEngine;

namespace _Project.Scripts
{
    public class EntityData : MonoBehaviour
    {
        public float Speed;
        public float SpeedMultiplier;
        public float AttackDistance;
        public float AttackDuration;
        public float DieDuration;
        public double KillReward;
        [HideInInspector]
        public bool IsAttacking;
        [HideInInspector]
        public bool IsDead;
        [HideInInspector]
        public Entity Owner;
        [HideInInspector]
        public Entity Target;
        [HideInInspector]
        public Vector2 Direction;
    }
}