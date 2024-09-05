using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathGen
{
    public class PathExecution : MonoBehaviour
    {
        private void PaintTiles(List<Tile> tiles, Tilemap map)
        {
            foreach (var tile in tiles)
            {
                Debug.Log($"Pos: {tile.position}, Type: {tile.type}");
                PaintSingleTile(map, tile.type, tile.position);
            }
        }

        public void PaintSingleTile(Tilemap map, TileBase tileType, Vector2Int position)
        {
            Vector3Int tilePosition = map.WorldToCell((Vector3Int)position);
            map.SetTile(tilePosition, tileType);
        }
        
        private void OnMouseUp()
        {
            PathData pathData = GetComponent<PathData>();
            PathConfig pathConfig = GetComponent<PathConfig>();
            RandomPathGeneration randomPath = GetComponent<RandomPathGeneration>();
            RandomFieldGeneration randomField = GetComponent<RandomFieldGeneration>();
            
            List<Tile> pathTiles = randomPath.RandomPathGen(out var endPosition, out var endTile);
            PaintTiles(pathTiles, pathConfig.tilemap);
            transform.position = new Vector3(endPosition.x, endPosition.y, 0);

            List<Tile> grassTiles = randomField.FillWithGrass(pathTiles);
            PaintTiles(grassTiles, pathConfig.tilemap);
            
            pathConfig.startingTile = endTile;
            pathData.currentPath.Clear();
        }
    }
}
