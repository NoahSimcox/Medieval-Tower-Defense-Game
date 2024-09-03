using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathGen
{
    public class PathExecution : MonoBehaviour
    {
        private void PaintTiles(IEnumerable<Tile> tiles, Tilemap map)
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
        
        // private Func<bool> DirectionConditions(List<Path.Tile> tiles) => scriptDirection switch
        // {
        //     Path.Direction.Right => () => tiles[0].position.x < tiles[^1].position.x,
        //     Path.Direction.Up => () => tiles[0].position.y < tiles[^1].position.y,
        //     Path.Direction.Left => () => tiles[0].position.x > tiles[^1].position.x,
        //     Path.Direction.Down => () => tiles[0].position.y > tiles[^1].position.y,
        //     _ => null
        // };
        
        
        private void OnMouseUp()
        {
            Path path = GetComponent<Path>();
            PathConfig pathConfig = GetComponent<PathConfig>();
            RandomPathGeneration randomPath = GetComponent<RandomPathGeneration>();
            
            List<Tile> tiles = randomPath.RandomPathGen(out var endPosition, out var endTile);
            PaintTiles(tiles, pathConfig.tilemap);
            
            transform.position = new Vector3(endPosition.x, endPosition.y, 0);
            pathConfig.startingTile = endTile;
            path.currentPath.Clear();
        }
    }
}
