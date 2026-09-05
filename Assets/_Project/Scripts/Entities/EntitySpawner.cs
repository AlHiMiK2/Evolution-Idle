using System;
using _Project.Scripts;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private Entity _prefab;
    [SerializeField] private Entities _entityType;

    private float _timeAfterSpawn;
    private int _spawnCount;
    private int _liveCount;

    public float SpawnProgress => _timeAfterSpawn / GetSpawnRate();
    public int SpawnCount => _spawnCount;
    public int LiveCount => _liveCount;
    public Entities Type => _entityType;

    private void Update()
    {
        _spawnCount = GetSpawnCount();
        _liveCount = GetLiveCount();
        if (_liveCount < _spawnCount)
        {
            _timeAfterSpawn += Time.deltaTime;
            
            if (_timeAfterSpawn >= GetSpawnRate())
            {
                Spawn();
                _timeAfterSpawn = 0;
            }
        }
    }

    private void Spawn()
    {
        Entity deadEntity = G.Instance.EntityContainer.GetDead(_entityType);
        
        if (deadEntity)
        {
            deadEntity.transform.position = G.Instance.Zone.GetSpawnPosition();
            deadEntity.OnSpawned();
            EventBus.OnEntitySpawned(deadEntity, _entityType);
        }
        else
        {
            var instance = Instantiate(_prefab, G.Instance.Zone.GetSpawnPosition(), Quaternion.identity);
            instance.OnSpawned();
            G.Instance.EntityContainer.Add(instance, _entityType);
            EventBus.OnEntitySpawned(instance, _entityType);
        }
    }

    private int GetLiveCount() => G.Instance.EntityContainer.GetLiveCount(_entityType);

    private int GetSpawnCount()
    {
        switch (_entityType)
        {
            case Entities.Plant:
                return G.Instance.UpgradesData.PlantSpawnCount;
            case Entities.Bunny:
                return G.Instance.UpgradesData.BunnySpawnCount;
            case Entities.Fox:
                return G.Instance.UpgradesData.FoxSpawnCount;
            case Entities.Wolf:
                return G.Instance.UpgradesData.WolfSpawnCount;
            case Entities.Bear:
                return G.Instance.UpgradesData.BearSpawnCount;
            case Entities.Hunter:
                return G.Instance.UpgradesData.HunterSpawnCount;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private float GetSpawnRate()
    { 
        switch (_entityType)
        {
            case Entities.Plant:
                return G.Instance.UpgradesData.PlantSpawnRate;
            case Entities.Bunny:
                return G.Instance.UpgradesData.BunnySpawnRate;
            case Entities.Fox:
                return G.Instance.UpgradesData.FoxSpawnRate;
            case Entities.Wolf:
                return G.Instance.UpgradesData.WolfSpawnRate;
            case Entities.Bear:
                return G.Instance.UpgradesData.BearSpawnRate;
            case Entities.Hunter:
                return G.Instance.UpgradesData.HunterSpawnRate;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
