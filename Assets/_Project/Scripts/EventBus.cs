using UnityEngine.Events;

namespace _Project.Scripts
{
    public static class EventBus
    {
        public static event UnityAction<Entity, Entities> EntitySpawned;
        public static event UnityAction<Entity> EntityDied;

        public static void OnEntitySpawned(Entity entity, Entities type) => EntitySpawned?.Invoke(entity, type);
        public static void OnEntityDied(Entity entity) => EntityDied?.Invoke(entity);
    }
}