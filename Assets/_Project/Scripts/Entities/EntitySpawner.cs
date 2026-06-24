using System.Collections;
using _Project.Scripts;
using _Project.Scripts.Shop;
using _Project.Scripts.UI;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    [SerializeField] private Zone _zone;

    private void OnEnable()
    {
        ShopEventHandler.EntityBought += Spawn;
    }

    private void OnDisable()
    {
        ShopEventHandler.EntityBought -= Spawn;
    }

    private void Spawn(ShopEntityConfig config)
    {
        var instance = Instantiate(config.Prefab, _zone.GetSpawnPosition(), Quaternion.identity);
        G.Instance.EntityContainer.AddEntity(instance);
        instance.OnSpawned(config.Price);
    }
}
