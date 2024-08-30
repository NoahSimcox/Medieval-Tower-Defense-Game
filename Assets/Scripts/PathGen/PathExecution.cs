using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace PathGen
{
    public class PathExecution : Path
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private TileBase tileFill;
        [SerializeField] private Tile initialTile;
        [SerializeField] private PathExecution pathExecution;
        [SerializeField] private Direction scriptDirection;
        private RandomPathGeneration _randomPath;
        private Vector2Int _endPosition;
        


        private void PaintTiles(IEnumerable<Tile> tiles, Tilemap map)
        {
            foreach (var tile in tiles)
            {
                Debug.Log(tile.position);
                PaintSingleTile(map, tile.type, tile.position);
            }
        }

        public void PaintSingleTile(Tilemap map, TileBase tileType, Vector2Int position)
        {
            Vector3Int tilePosition = map.WorldToCell((Vector3Int)position);
            map.SetTile(tilePosition, tileType);
        }

        private void Fill(Vector2Int[] pathLocations)
        {

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    foreach (var position in pathLocations)
                    {
                        if (position.x != i || position.y != j)
                            PaintSingleTile(tilemap, tileFill, new Vector2Int(i, j));
                    }
                }
            }
        }
        
        private void OnMouseUp()
        {
            Debug.Log($"Init Pos: {initialTile.position}, Init Direction: {initialTile.direction}, Init Type: {initialTile.type}");
            _randomPath = new RandomPathGeneration(tilemap, currentPath, tileTypes, initialTile, pathExecution, scriptDirection);
            List<Tile> tiles = _randomPath.RandomPathGen(out var endPosition, out var endTile, 0, 0);
            PaintTiles(tiles, tilemap);
            transform.position = new Vector3(endPosition.x, endPosition.y, 0);
            initialTile = endTile;
            Debug.Log($"Init Pos: {initialTile.position}, Init Direction: {initialTile.direction}, Init Type: {initialTile.type}");
        }
    }
}
