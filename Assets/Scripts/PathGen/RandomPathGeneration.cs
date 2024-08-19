using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace PathGen
{
    public class RandomPathGeneration : Path
    {
        private List<Tile> RandomPathGen(Tile startingTile, out Vector2Int endPos,
            int width, int height, TileTypes tileTypes)
        {
            var currentPath = new List<Tile>();
            var pathCount = 0;

            currentPath.Add(startingTile);

            while (currentPath[pathCount].position != Goal)
            {
                var currentTile = currentPath[pathCount];

                var tile = DetermineTileTypeAndDirection(currentTile, tileTypes);

                tile.position += CalculateNewPosition(currentTile);

                pathCount++;
                currentPath.Add(tile);
            }

            endPos = currentPath[pathCount].position;
            return currentPath;
        }


        private Tile DetermineTileTypeAndDirection(Tile currentTile, TileTypes tileTypes)
        {
            var newTile = new Tile();
            float randomValue = Random.Range(0.0f, 10.0f);

            newTile = randomValue switch
            {
                
                <= 6.0f =>
                    
            };

            return newTile;
        }
        
        
        private Tile CreateTile(TileBase type, Direction direction) => new Tile { type = type, direction = direction };
        
        private Tile CreateTurnTile(TileBase type, Direction direction)
        {
            return new Tile { type = type, direction = direction };
        }

        private IEnumerable<Direction> OptimalDirections(Tile currentTile)
        {
            Dictionary<float, Direction> distances = new Dictionary<float, Direction>();
                
            var cases = new (Func<bool> condition, Action action)[]
            {
                (() => currentTile.direction != Direction.Down, () => distances.Add(Vector2Int.Distance(currentTile.position + Vector2Int.up, Goal), Direction.Up)),
                (() => currentTile.direction != Direction.Up, () => distances.Add(Vector2Int.Distance(currentTile.position + Vector2Int.down, Goal), Direction.Down)),
                (() => currentTile.direction != Direction.Right, () => distances.Add(Vector2Int.Distance(currentTile.position + Vector2Int.left, Goal), Direction.Left)),
                (() => currentTile.direction != Direction.Left, () => distances.Add(Vector2Int.Distance(currentTile.position + Vector2Int.right, Goal), Direction.Right))
            };

            foreach (var (condition, action) in cases)
                if (condition())
                    action();
            

            return from dist in distances
                   orderby dist.Key
                   select dist.Value;
        }

        private Vector2Int CalculateNewPosition(Tile currentTile) =>
            currentTile.direction switch
            {
                Direction.Up => Vector2Int.up,
                Direction.Down => Vector2Int.down,
                Direction.Left => Vector2Int.left,
                Direction.Right => Vector2Int.right,
                _ => Vector2Int.zero
            };
    }
}
