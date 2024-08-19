using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathGen
{
    public class test : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Vector2Int startPosition;
        [SerializeField] private TileBase tileFill;

        private void Start()
        {
            Vector3Int tilePosition = tilemap.WorldToCell((Vector3Int)startPosition);
            tilemap.SetTile(tilePosition, tileFill);
        }
    }
}