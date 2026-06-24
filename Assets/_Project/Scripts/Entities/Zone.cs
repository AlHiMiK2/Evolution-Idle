using System;
using _Project.Scripts.Shop;
using _Project.Scripts.UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts
{
    public class Zone : MonoBehaviour
    {
        public Vector2 GetSpawnPosition()
        {
            float maxX, minX, maxY, minY;
            maxX = transform.lossyScale.x / 2f + transform.position.x;
            minX = -transform.lossyScale.x / 2f + transform.position.x;
            maxY = transform.lossyScale.y / 2f + transform.position.y;
            minY = -transform.lossyScale.y / 2f + transform.position.y;
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            return new Vector2(randomX, randomY);
        }
        
        private void OnDrawGizmos()
        {
            Vector2 size = new Vector2(transform.lossyScale.x, transform.lossyScale.y);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}