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
            int width, int height)
        {
            var pathCount = 0;

            currentPath.Add(startingTile);

            while (currentPath[pathCount].position != Goal)
            {
                var currentTile = currentPath[pathCount];

                var tile = WeightedRandomChoice(currentTile);

                if (tile.turnTile) //in this case a turnTile was place
                {
                    pathCount += 3;
                    continue;
                }

                pathCount++;
                currentPath.Add(tile);
            }

            endPos = currentPath[pathCount].position;
            return currentPath;
        }


        private Tile WeightedRandomChoice(Tile currentTile)
        {
            float randomValue = Random.Range(0.0f, 10.0f);

            Tile newTile = randomValue switch
            {
                // <= 6.0f =>
            };

            return newTile;
        }
        
        
        private Tile CreateTile(TileBase type, Direction direction, Tile currentTile) => new Tile { type = type, direction = direction, position = CalculateNewPosition(currentTile)};
        private Tile CreateTurnTile(TileBase type, Direction direction, Tile currentTile)
        {
            List<Tile> newTurnTiles = new List<Tile>();
            PathExecution paintCornerTile;
            Vector2Int tilePosition = currentTile.position;

            var turnTilePositions = new { up = tilePosition + Vector2Int.up, 
                                                    down = tilePosition + Vector2Int.down,
                                                    left = tilePosition + Vector2Int.left,
                                                    right = tilePosition + Vector2Int.right
                                                    }; 
            Action<Vector2Int> reset = position => tilePosition = currentTile.position;
            
            var tileActions = new Dictionary<TileBase, (Action, Vector2Int, Vector2Int, Vector2Int, Action)>
            {
                { tileTypes.upRight, (reset(), turnTilePositions.up, turnTilePositions.up + Vector2Int.up, turnTilePositions.up + Vector2Int.up + Vector2Int.right, paintCornerTile.PaintSingleTile(tilemap, tileTypes.upRight, ))},
                { tileTypes.upLeft, (tilePosition + Vector2Int.up * 2 + Vector2Int.left)},
                { tileTypes.downRight, (tilePosition + Vector2Int.down * 2 + Vector2Int.right)},
                { tileTypes.downLeft, (tilePosition + Vector2Int.down * 2 + Vector2Int.left)},
                { tileTypes.rightUp, (tilePosition + Vector2Int.right * 2 + Vector2Int.up)},
                { tileTypes.rightDown, (tilePosition + Vector2Int.right * 2 + Vector2Int.down)},
                { tileTypes.leftUp, (tilePosition + Vector2Int.left * 2 + Vector2Int.up)},
                { tileTypes.leftDown, (tilePosition + Vector2Int.left * 2 + Vector2Int.down)}
            };

            if (tileActions.TryGetValue(type, out var newTilePosition))
            {
                
            }
        }

        private Tile DetermineTileType(Tile currentTile, Direction newDirection) =>
            currentTile.direction switch
            {
                Direction.Up when newDirection == Direction.Up => CreateTile(tileTypes.vertical, Direction.Up, currentTile),
                Direction.Up when newDirection == Direction.Right => CreateTurnTile(tileTypes.upRight, Direction.Right, currentTile),
                Direction.Up when newDirection == Direction.Left => CreateTurnTile(tileTypes.upLeft, Direction.Left, currentTile),
                Direction.Down when newDirection == Direction.Down => CreateTile(tileTypes.vertical, Direction.Down, currentTile),
                Direction.Down when newDirection == Direction.Right => CreateTurnTile(tileTypes.downRight, Direction.Right, currentTile),
                Direction.Down when newDirection == Direction.Left => CreateTurnTile(tileTypes.downLeft, Direction.Left, currentTile),
                Direction.Left when newDirection == Direction.Left => CreateTile(tileTypes.horizontal, Direction.Left, currentTile),
                Direction.Left when newDirection == Direction.Up => CreateTurnTile(tileTypes.leftUp, Direction.Up, currentTile),
                Direction.Left when newDirection == Direction.Down => CreateTurnTile(tileTypes.leftDown, Direction.Down, currentTile),
                Direction.Right when newDirection == Direction.Right => CreateTile(tileTypes.horizontal, Direction.Right, currentTile),
                Direction.Right when newDirection == Direction.Up => CreateTurnTile(tileTypes.rightUp, Direction.Up, currentTile),
                Direction.Right when newDirection == Direction.Down => CreateTurnTile(tileTypes.rightDown, Direction.Down, currentTile)
            };

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
