using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _Project.Scripts
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileBase _sandTile;
        [SerializeField] private TileBase _grassTile;
        [SerializeField] private float _regrowthInterval = 5f;

        private float _regrowthTimer;

        private void Update()
        {
            _regrowthTimer += Time.deltaTime;
            if (_regrowthTimer >= _regrowthInterval)
            {
                _regrowthTimer = 0f;
                RegrowGrass();
            }
        }

        public void EatTile(Vector3 position)
        {
            Vector3Int cellPosition = _tilemap.WorldToCell(position);
            _tilemap.SetTile(cellPosition, _sandTile);
        }

        public bool GetNearestGrassTile(Vector3 position, out Vector3 target)
        {
            BoundsInt bounds = _tilemap.cellBounds;
            float minDist = float.MaxValue;
            Vector3Int nearestCell = Vector3Int.zero;
            bool found = false;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cellPos = new Vector3Int(x, y, 0);
                    TileBase tile = _tilemap.GetTile(cellPos);
                    if (tile == _grassTile)
                    {
                        Vector3 worldPos = _tilemap.CellToWorld(cellPos);
                        if (!G.Instance.Zone.InZone(worldPos))
                            continue;

                        Vector3 center = worldPos + new Vector3(
                            _tilemap.cellSize.x * 0.5f,
                            _tilemap.cellSize.y * 0.5f,
                            0);

                        float dist = Vector3.Distance(position, center);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            nearestCell = cellPos;
                            found = true;
                        }
                    }
                }
            }

            if (!found)
            {
                target = Vector3.zero;
                return false;
            }

            List<Vector3Int> grassNeighbors = new List<Vector3Int>();
            Vector3Int[] directions = new Vector3Int[]
            {
                Vector3Int.up,
                Vector3Int.down,
                Vector3Int.left,
                Vector3Int.right
            };

            foreach (Vector3Int dir in directions)
            {
                Vector3Int neighborPos = nearestCell + dir;
                TileBase neighborTile = _tilemap.GetTile(neighborPos);
                if (neighborTile == _grassTile)
                {
                    Vector3 worldPos = _tilemap.CellToWorld(neighborPos);
                    if (G.Instance.Zone.InZone(worldPos))
                    {
                        grassNeighbors.Add(neighborPos);
                    }
                }
            }

            Vector3Int chosenCell = grassNeighbors.Count > 0
                ? grassNeighbors[Random.Range(0, grassNeighbors.Count)]
                : nearestCell;

            Vector3 chosenWorldPos = _tilemap.CellToWorld(chosenCell);
            target = chosenWorldPos + new Vector3(
                _tilemap.cellSize.x * 0.5f,
                _tilemap.cellSize.y * 0.5f,
                0);

            return true;
        }

        public bool HasGrassTileAt(Vector3 position)
        {
            Vector3Int cellPosition = _tilemap.WorldToCell(position);
            return _tilemap.GetTile(cellPosition) == _grassTile;
        }

        private void RegrowGrass()
        {
            if (_tilemap == null || _sandTile == null || _grassTile == null)
                return;

            BoundsInt bounds = _tilemap.cellBounds;
            List<Vector3Int> candidates = new List<Vector3Int>();

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cellPos = new Vector3Int(x, y, 0);
                    TileBase tile = _tilemap.GetTile(cellPos);
                    if (tile == _sandTile && IsAdjacentToGrass(cellPos))
                    {
                        candidates.Add(cellPos);
                    }
                }
            }

            if (candidates.Count > 0)
            {
                Vector3Int chosen = candidates[Random.Range(0, candidates.Count)];
                _tilemap.SetTile(chosen, _grassTile);
            }
        }

        private bool IsAdjacentToGrass(Vector3Int cellPos)
        {
            Vector3Int[] directions = new Vector3Int[]
            {
                Vector3Int.up,
                Vector3Int.down,
                Vector3Int.left,
                Vector3Int.right
            };

            foreach (Vector3Int dir in directions)
            {
                Vector3Int neighbor = cellPos + dir;
                TileBase tile = _tilemap.GetTile(neighbor);
                if (tile == _grassTile)
                    return true;
            }
            return false;
        }
    }
}