using System.Collections;
using _Project.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnZone;
    [SerializeField] private float _respawnDuration;
    
    public void Spawn(ShopEntityConfig config)
    {
        var instance = Instantiate(config.Prefab, GetSpawnPosition(), Quaternion.identity);
        G.Instance.EntityContainer.AddEntity(instance);
    }
    
    private Vector2 GetSpawnPosition()
    {
        float maxX, minX, maxY, minY;
        maxX = _spawnZone.lossyScale.x / 2f + _spawnZone.position.x;
        minX = -_spawnZone.lossyScale.x / 2f + _spawnZone.position.x;
        maxY = _spawnZone.lossyScale.y / 2f + _spawnZone.position.y;
        minY = -_spawnZone.lossyScale.y / 2f + _spawnZone.position.y;
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector2(randomX, randomY);
    }

    public void OnDied(Entity entity)
    {
        StartCoroutine(RespawnCoroutine(entity));
    }

    private IEnumerator RespawnCoroutine(Entity entity)
    {
        yield return new WaitForSeconds(_respawnDuration);
        entity.transform.position = GetSpawnPosition();
        G.Instance.EntityContainer.AddEntity(entity);
        entity.gameObject.SetActive(true);
        entity.OnRespawned();
    }
}
