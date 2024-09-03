using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace PathGen
{
    public class RandomPathGeneration : Path
    {
        // protected override void Start()
        // {
        //     base.Start();
        //     Debug.Log(tileTypes.horizontal);
        // }
        
        public List<Tile> RandomPathGen(out Vector2Int endPos, out Tile endTile)
        {
            Debug.Log(StartingTile.type);
            Debug.Log(TileTypes.horizontal);
            var pathCount = 0;
            currentPath.Add(StartingTile);
            GoalSetting();
            Func<int, bool> loopCondition = DirectionConditions();

            while (loopCondition!(pathCount))
            {
                var currentTile = currentPath[pathCount];

                Tile tile = WeightedRandomChoice(currentTile);

                if (tile.turnTile) // in this case a turnTile was placed as opposed to a normal tile
                {
                    pathCount += 3;
                    continue;
                }

                pathCount++;
                currentPath.Add(tile);
            }
            
            endPos = currentPath[pathCount].position;
            endTile = currentPath[pathCount];
            currentPath.RemoveAt(0);
            return currentPath;
        }


        private Tile WeightedRandomChoice(Tile currentTile)
        {
            float randomValue = Random.Range(0.0f, 10.0f);
            List<Direction> directionsList = OptimalDirections(currentTile).ToList();


            Tile newTile = randomValue switch
            {
                <= 6.5f => DetermineTileType(currentTile, directionsList[0]),
                _ => DetermineTileType(currentTile, directionsList[1])
            };

            return newTile;
        }
        
        
        private Tile CreateTile(TileBase type, Direction direction, Tile currentTile) => new Tile { type = type, direction = direction, position = CalculateNewPosition(currentTile)};
        private Tile CreateTurnTile(TileBase type, Direction direction, Tile currentTile)
        {
            PathExecution paintCornerTile = GetComponent<PathExecution>();
            Vector2Int tilePosition = currentTile.position;

            var turnTilePositions = new { up = new Func<Vector2Int>(() => tilePosition += Vector2Int.up), 
                                                    down = new Func<Vector2Int>(() => tilePosition += Vector2Int.down),
                                                    left = new Func<Vector2Int>(() => tilePosition += Vector2Int.left),
                                                    right = new Func<Vector2Int>(() => tilePosition += Vector2Int.right)
                                                    };

            
            var tileActions = new Dictionary<TileBase, (Func<Vector2Int>, Func<Vector2Int>, Func<Vector2Int>, Func<Vector2Int>)>
            {
                { TileTypes.upRight, (turnTilePositions.up, turnTilePositions.up, turnTilePositions.right, turnTilePositions.down)},
                { TileTypes.upLeft, (turnTilePositions.up, turnTilePositions.up, turnTilePositions.left, turnTilePositions.down)},
                { TileTypes.downRight, (turnTilePositions.down, turnTilePositions.down, turnTilePositions.right, turnTilePositions.up)},
                { TileTypes.downLeft, (turnTilePositions.down, turnTilePositions.down, turnTilePositions.left, turnTilePositions.up)},
                { TileTypes.rightUp, (turnTilePositions.right, turnTilePositions.right, turnTilePositions.up, turnTilePositions.left)},
                { TileTypes.rightDown, (turnTilePositions.right, turnTilePositions.right, turnTilePositions.down, turnTilePositions.left)},
                { TileTypes.leftUp, (turnTilePositions.left, turnTilePositions.left, turnTilePositions.up, turnTilePositions.right)},
                { TileTypes.leftDown, (turnTilePositions.left, turnTilePositions.left, turnTilePositions.down, turnTilePositions.right)}
            };

            if (tileActions.TryGetValue(type, out var newTilePosition))
            {
                var tupleItems = new [] { newTilePosition.Item1, newTilePosition.Item2, newTilePosition.Item3 };
                foreach (var position in tupleItems)
                {
                    currentPath.Add( new Tile
                    {
                        type = type,
                        direction = direction,
                        position = position()
                    
                    });
                }

                paintCornerTile.PaintSingleTile(Tilemap, type, newTilePosition.Item4());
            }

            return new Tile() // dummy tile
            {
                turnTile = true
            };
        }

        private Tile DetermineTileType(Tile currentTile, Direction newDirection) =>
            currentTile.direction switch
            { 
                Direction.Up when newDirection == Direction.Up => CreateTile(TileTypes.vertical, Direction.Up, currentTile),
                Direction.Up when newDirection == Direction.Right => CreateTurnTile(TileTypes.upRight,Direction.Right, currentTile),
                Direction.Up when newDirection == Direction.Left => CreateTurnTile(TileTypes.upLeft,Direction.Left, currentTile),
                Direction.Down when newDirection == Direction.Down => CreateTile(TileTypes.vertical,Direction.Down, currentTile),
                Direction.Down when newDirection == Direction.Right => CreateTurnTile(TileTypes.downRight,Direction.Right, currentTile),
                Direction.Down when newDirection == Direction.Left => CreateTurnTile(TileTypes.downLeft,Direction.Left, currentTile),
                Direction.Left when newDirection == Direction.Left => CreateTile(TileTypes.horizontal,Direction.Left, currentTile),
                Direction.Left when newDirection == Direction.Up => CreateTurnTile(TileTypes.leftUp,Direction.Up, currentTile),
                Direction.Left when newDirection == Direction.Down => CreateTurnTile(TileTypes.leftDown,Direction.Down, currentTile),
                Direction.Right when newDirection == Direction.Right => CreateTile(TileTypes.horizontal,Direction.Right, currentTile),
                Direction.Right when newDirection == Direction.Up => CreateTurnTile(TileTypes.rightUp,Direction.Up, currentTile),
                Direction.Right when newDirection == Direction.Down => CreateTurnTile(TileTypes.rightDown,Direction.Down, currentTile),
                _ => throw new Exception("balls")
            };

        private IEnumerable<Direction> OptimalDirections(Tile currentTile)
        {
            Dictionary<float,Direction> distances = new Dictionary<float,Direction>();
            float rand = Random.Range(-0.1f, 0.1f);
                
            var cases = new (Func<bool> condition, Action action)[]
            {
                (() => currentTile.direction !=Direction.Down, () => distances.Add(Vector2Int.Distance(currentTile.position + Vector2Int.up, Goal) + rand,Direction.Up)),
                (() => currentTile.direction !=Direction.Up, () => distances.Add(Vector2Int.Distance(currentTile.position + Vector2Int.down, Goal) + (rand + 0.01f),Direction.Down)),
                (() => currentTile.direction !=Direction.Right, () => distances.Add(Vector2Int.Distance(currentTile.position + Vector2Int.left, Goal) + (rand + 0.02f),Direction.Left)),
                (() => currentTile.direction !=Direction.Left, () => distances.Add(Vector2Int.Distance(currentTile.position + Vector2Int.right, Goal) + (rand + 0.03f),Direction.Right))
            };

            foreach (var (condition, action) in cases)
                if (condition())
                    action();
            

            return from dist in distances
                   orderby dist.Key
                   select dist.Value;
        }

        private Vector2Int CalculateNewPosition(Tile currentTile)
        {
            Vector2Int currentPosition = currentTile.position;
            
            return currentTile.direction switch
            {
                Direction.Up => currentPosition + Vector2Int.up,
                Direction.Down => currentPosition + Vector2Int.down,
                Direction.Left => currentPosition + Vector2Int.left,
                Direction.Right => currentPosition + Vector2Int.right,
                _ => Vector2Int.zero
            };
            
        }

        private void GoalSetting()
        {

            Vector2Int randVector = ScriptDirection switch
            {
                Direction.Up or Direction.Down => new((int)Random.Range(-6.9f, 6.9f), (int)Random.Range(10.0f, 20.0f)),
                Direction.Right or Direction.Left => new((int)Random.Range(10.0f, 20.0f), (int)Random.Range(-6.9f, 6.9f)),
                _ => Vector2Int.zero
            };
            
            Vector2Int goalDirection = ScriptDirection switch
            {
                Direction.Down => new Vector2Int(1, -1),
                Direction.Left => new Vector2Int(-1, 1),
                _ => new Vector2Int(1,1)
            };

            Goal = Vector2Int.Scale(randVector, goalDirection) + StartingTile.position;
        }

        private Func<int, bool> DirectionConditions() => ScriptDirection switch
        {
            Direction.Right => x => currentPath[x].position.x < Goal.x,
            Direction.Up => x => currentPath[x].position.y < Goal.y,
            Direction.Left => x => currentPath[x].position.x > Goal.x,
            Direction.Down => x => currentPath[x].position.y > Goal.y,
            _ => null
        };
    }
}
