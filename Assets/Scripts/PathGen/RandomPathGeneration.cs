using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace PathGen
{
    public class RandomPathGeneration
    {
        private readonly Vector2Int _goal;
        private readonly List<Path.Tile> _currentPath;
        private readonly Path.TileTypes _tileTypes;
        private readonly Tilemap _tilemap;
        private readonly Path.Tile _startingTile;
        private readonly PathExecution _pathExecution;
        public RandomPathGeneration(Tilemap tilemap, List<Path.Tile> currentPath, Path.TileTypes tileTypes, Path.Tile startingTile, PathExecution pathExecution, Path.Direction scriptDirection)
        {
            _tilemap = tilemap;
            _currentPath = currentPath;
            _tileTypes = tileTypes;
            _startingTile = startingTile;
            _pathExecution = pathExecution;
            _goal = GoalSetting(scriptDirection, startingTile);
            Debug.Log(_goal);
            Debug.DrawLine(new Vector3(_goal.x, _goal.y, -10), new Vector3(_goal.x, _goal.y+0.5f, -10), Color.red, 100000f);
        }
        
        public List<Path.Tile> RandomPathGen(out Vector2Int endPos, out Path.Tile endTile, int width, int height)
        {
            
            var pathCount = 0;

            _currentPath.Add(_startingTile);

            while (_currentPath[pathCount].position.x < _goal.x)
            {
                var currentTile = _currentPath[pathCount];

                Path.Tile tile = WeightedRandomChoice(currentTile);

                if (tile.turnTile) // in this case a turnTile was placed as opposed to a normal tile
                {
                    pathCount += 3;
                    continue;
                }

                pathCount++;
                _currentPath.Add(tile);
            }

            endPos = _currentPath[pathCount].position;
            endTile = _currentPath[pathCount];
            return _currentPath;
        }


        private Path.Tile WeightedRandomChoice(Path.Tile currentTile)
        {
            float randomValue = Random.Range(0.0f, 10.0f);
            List<Path.Direction> directionsList = new List<Path.Direction>();
            
            foreach (var direction in OptimalDirections(currentTile))
            {
                directionsList.Add(direction);
            }
            
            
            Path.Tile newTile = randomValue switch
            {
                <= 6.0f => DetermineTileType(currentTile, directionsList[0]),
                // <= 7.0f => DetermineTileType(currentTile, directionsList[1]),
                _ => DetermineTileType(currentTile, directionsList[1])
            };

            return newTile;
        }
        
        
        private Path.Tile CreateTile(TileBase type, Path.Direction direction, Path.Tile currentTile) => new Path.Tile { type = type, direction = direction, position = CalculateNewPosition(currentTile)};
        private Path.Tile CreateTurnTile(TileBase type, Path.Direction direction, Path.Tile currentTile)
        {
            List<Path.Tile> newTurnTiles = new List<Path.Tile>();
            PathExecution paintCornerTile = _pathExecution;
            Vector2Int tilePosition = currentTile.position;

            var turnTilePositions = new { up = new Func<Vector2Int>(() => tilePosition += Vector2Int.up), 
                                                    down = new Func<Vector2Int>(() => tilePosition += Vector2Int.down),
                                                    left = new Func<Vector2Int>(() => tilePosition += Vector2Int.left),
                                                    right = new Func<Vector2Int>(() => tilePosition += Vector2Int.right)
                                                    };

            
            var tileActions = new Dictionary<TileBase, (Func<Vector2Int>, Func<Vector2Int>, Func<Vector2Int>, Func<Vector2Int>)>
            {
                { _tileTypes.upRight, (turnTilePositions.up, turnTilePositions.up, turnTilePositions.right, turnTilePositions.down)},
                { _tileTypes.upLeft, (turnTilePositions.up, turnTilePositions.up, turnTilePositions.left, turnTilePositions.down)},
                { _tileTypes.downRight, (turnTilePositions.down, turnTilePositions.down, turnTilePositions.right, turnTilePositions.up)},
                { _tileTypes.downLeft, (turnTilePositions.down, turnTilePositions.down, turnTilePositions.left, turnTilePositions.up)},
                { _tileTypes.rightUp, (turnTilePositions.right, turnTilePositions.right, turnTilePositions.up, turnTilePositions.left)},
                { _tileTypes.rightDown, (turnTilePositions.right, turnTilePositions.right, turnTilePositions.down, turnTilePositions.left)},
                { _tileTypes.leftUp, (turnTilePositions.left, turnTilePositions.left, turnTilePositions.up, turnTilePositions.right)},
                { _tileTypes.leftDown, (turnTilePositions.left, turnTilePositions.left, turnTilePositions.down, turnTilePositions.right)}
            };

            if (tileActions.TryGetValue(type, out var newTilePosition))
            {
                var tuple = tileActions[type];
                var tupleItems = new [] { tuple.Item1, tuple.Item2, tuple.Item3 };
                foreach (var position in tupleItems)
                {
                    _currentPath.Add( new Path.Tile
                    {
                        type = type,
                        direction = direction,
                        position = position()
                    
                    });
                }

                paintCornerTile.PaintSingleTile(_tilemap, type, tuple.Item4());
            }

            return new Path.Tile() // dummy tile
            {
                turnTile = true
            };
        }

        private Path.Tile DetermineTileType(Path.Tile currentTile, Path.Direction newDirection) =>
            currentTile.direction switch
            { 
                Path.Direction.Up when newDirection == Path.Direction.Up => CreateTile(_tileTypes.vertical, Path.Direction.Up, currentTile),
                Path.Direction.Up when newDirection == Path.Direction.Right => CreateTurnTile(_tileTypes.upRight,Path.Direction.Right, currentTile),
                Path.Direction.Up when newDirection == Path.Direction.Left => CreateTurnTile(_tileTypes.upLeft,Path.Direction.Left, currentTile),
                Path.Direction.Down when newDirection == Path.Direction.Down => CreateTile(_tileTypes.vertical,Path.Direction.Down, currentTile),
                Path.Direction.Down when newDirection == Path.Direction.Right => CreateTurnTile(_tileTypes.downRight,Path.Direction.Right, currentTile),
                Path.Direction.Down when newDirection == Path.Direction.Left => CreateTurnTile(_tileTypes.downLeft,Path.Direction.Left, currentTile),
                Path.Direction.Left when newDirection == Path.Direction.Left => CreateTile(_tileTypes.horizontal,Path.Direction.Left, currentTile),
                Path.Direction.Left when newDirection == Path.Direction.Up => CreateTurnTile(_tileTypes.leftUp,Path.Direction.Up, currentTile),
                Path.Direction.Left when newDirection == Path.Direction.Down => CreateTurnTile(_tileTypes.leftDown,Path.Direction.Down, currentTile),
                Path.Direction.Right when newDirection == Path.Direction.Right => CreateTile(_tileTypes.horizontal,Path.Direction.Right, currentTile),
                Path.Direction.Right when newDirection == Path.Direction.Up => CreateTurnTile(_tileTypes.rightUp,Path.Direction.Up, currentTile),
                Path.Direction.Right when newDirection == Path.Direction.Down => CreateTurnTile(_tileTypes.rightDown,Path.Direction.Down, currentTile)
            };

        private IEnumerable<Path.Direction> OptimalDirections(Path.Tile currentTile)
        {
            Dictionary<float,Path.Direction> distances = new Dictionary<float,Path.Direction>();
            float rand = Random.Range(-0.1f, 0.1f);
                
            var cases = new (Func<bool> condition, Action action)[]
            {
                (() => currentTile.direction !=Path.Direction.Down, () => distances.Add(Vector2Int.Distance(currentTile.position + Vector2Int.up, _goal) + rand,Path.Direction.Up)),
                (() => currentTile.direction !=Path.Direction.Up, () => distances.Add(Vector2Int.Distance(currentTile.position + Vector2Int.down, _goal) + (rand + 0.01f),Path.Direction.Down)),
                (() => currentTile.direction !=Path.Direction.Right, () => distances.Add(Vector2Int.Distance(currentTile.position + Vector2Int.left, _goal) + (rand + 0.02f),Path.Direction.Left)),
                (() => currentTile.direction !=Path.Direction.Left, () => distances.Add(Vector2Int.Distance(currentTile.position + Vector2Int.right, _goal) + (rand + 0.03f),Path.Direction.Right))
            };

            foreach (var (condition, action) in cases)
                if (condition())
                    action();
            

            return from dist in distances
                   orderby dist.Key
                   select dist.Value;
        }

        private Vector2Int CalculateNewPosition(Path.Tile currentTile)
        {
            Vector2Int currentPosition = currentTile.position;
            
            return currentTile.direction switch
            {
                Path.Direction.Up => currentPosition + Vector2Int.up,
                Path.Direction.Down => currentPosition + Vector2Int.down,
                Path.Direction.Left => currentPosition + Vector2Int.left,
                Path.Direction.Right => currentPosition + Vector2Int.right,
                _ => Vector2Int.zero
            };
            
        }

        private Vector2Int GoalSetting(Path.Direction scriptDirection, Path.Tile startingTile)
        {

            Vector2Int randVector = scriptDirection switch
            {
                Path.Direction.Up or Path.Direction.Down => new((int)Random.Range(-6.9f, 6.9f), (int)Random.Range(10.0f, 20.0f)),
                Path.Direction.Right or Path.Direction.Left => new((int)Random.Range(10.0f, 20.0f), (int)Random.Range(-6.9f, 6.9f)),
                _ => Vector2Int.zero
            };
            
            Vector2Int goalDirection = scriptDirection switch
            {
                Path.Direction.Down => new Vector2Int(1, -1),
                Path.Direction.Left => new Vector2Int(-1, 1),
                _ => new Vector2Int(1,1)
            };

            return Vector2Int.Scale(randVector, goalDirection) + startingTile.position;
        }
    }
}
