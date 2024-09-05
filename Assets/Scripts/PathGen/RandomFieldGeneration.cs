using System;
using System.Collections.Generic;
using UnityEngine;

namespace PathGen
{
    public class RandomFieldGeneration : PathData
    {
        public List<Tile> FillWithGrass(List<Tile> tiles)
        {
            List<Tile> grassTiles = new List<Tile>();
            
            var boundary = FindBoundsOfPath(tiles);

            var position = transform.position;
            float transformPositionX = position.x;
            float transformPositionY = position.y;
            int positionX = Mathf.FloorToInt(transformPositionX);
            int positionY = Mathf.FloorToInt(transformPositionY);

            int startingPosX = StartingTile.position.x;
            int startingPosY = StartingTile.position.y;

            (int, Func<int, bool>, Func<int, int>) determineConditions = ScriptDirection switch
            {
                Direction.Up => (startingPosY, x => x < positionY, x => ++x),
                Direction.Down => (startingPosY, x => x > positionY, x => --x),
                Direction.Left => (startingPosX, x => x > positionX, x => --x),
                Direction.Right => (startingPosX, x => x < positionX, x => ++x),
                _ => throw new Exception("pog day")
            };

            Func<int, int, Vector2Int> determineXY = ScriptDirection switch
            {
                Direction.Up or Direction.Down => (i, j) => new Vector2Int(j, i),
                Direction.Right or Direction.Left => (i, j) => new Vector2Int(i, j),
                _ => throw new Exception("pog day")
            };

            for (int i = determineConditions.Item1; determineConditions.Item2(i); i = determineConditions.Item3(i))
            {
                for (int j = boundary.Item2 + 1; j >= boundary.Item1 - 1; j--)
                {
                    if (Tilemap.HasTile(Tilemap.WorldToCell( new Vector3Int(determineXY(i, j).x, determineXY(i, j).y, 0))))
                        continue;
                    
                    grassTiles.Add(new Tile
                    {
                        position = determineXY(i, j),
                        type = TileTypes.rockBorder
                    });
                }
            }

            return grassTiles;
        }
        
        private (int, int) FindBoundsOfPath(List<Tile> tiles)
        {
            List<Tile> tempTiles = tiles;
            
            (Func<Tile, Tile, int>, Func<(int, int)>) determineComparison = ScriptDirection switch
            {
                Direction.Up or Direction.Down => ((x,y) => x.position.x.CompareTo(y.position.x), () => (tempTiles[0].position.x, tempTiles[^1].position.x)),
                Direction.Right or Direction.Left => ((x,y) => x.position.y.CompareTo(y.position.y), () => (tempTiles[0].position.y, tempTiles[^1].position.y)),
                _ => throw new Exception("pog day")
            };
            
            tempTiles.Sort((tile, tile1) => determineComparison.Item1(tile, tile1));
            
            return determineComparison.Item2();
        }
    }
}