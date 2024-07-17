using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathGen
{
    public class RandomPathGenerationRight : MonoBehaviour
    {
        public enum Direction
        {
            Right,
            Left, 
            Up, 
            Down
        }

        public struct Tile
        {
            public Direction Direction;
            public Vector2Int Position;
            public TileBase Type;
        }

        public List<Tile> RandomPathGen(Vector2Int startPos, out Vector2Int endPos, Direction startingTileDirection,
            TileBase startingTileType, TileBase vertical, TileBase horizontal, TileBase upRight, TileBase upLeft,
            TileBase downRight, TileBase downLeft, int width, int height, Vector2Int startPosition)
        {
            List<Tile> currentPath = new List<Tile>();
            int pathCount = 0;

            Tile startingTile = new Tile();
            startingTile.Position = startPos;
            startingTile.Type = startingTileType;
            startingTile.Direction = startingTileDirection;
            currentPath[pathCount] = startingTile;

            while (currentPath[pathCount].Position.x < width + startPosition.x &&
                   currentPath[pathCount].Position.y < height + startPosition.y)
            {
                Tile tile = new Tile();

                switch (currentPath[pathCount].Direction)
                {
                    case Direction.Right: // if your moving right
                        if (Random.Range(0.0f, 10.0f) <= 6.0f)
                        {
                            tile.Type = horizontal;
                            tile.Direction = Direction.Right;
                            
                            if (currentPath[pathCount].Type == horizontal)
                                tile.Position = new Vector2Int(currentPath[pathCount].Position.x + 1,
                                currentPath[pathCount].Position.y);
                            else
                                tile.Position = new Vector2Int(currentPath[pathCount].Position.x + 2,
                                    currentPath[pathCount].Position.y);
                        }
                        else if (Random.Range(0.0f, 10.0f) <= 8.0f)
                        {
                            tile.Type = upLeft;
                            tile.Direction = Direction.Down;
                            
                            if (currentPath[pathCount].Type == horizontal)
                                tile.Position = new Vector2Int(currentPath[pathCount].Position.x + 1,
                                    currentPath[pathCount].Position.y - 1);
                            else if (currentPath[pathCount].Type == upRight)
                                tile.Position = new Vector2Int(currentPath[pathCount].Position.x + 2,
                                    currentPath[pathCount].Position.y);
                            else if (currentPath[pathCount].Type == downRight)
                                tile.Position = new Vector2Int(currentPath[pathCount].Position.x + 2,
                                    currentPath[pathCount].Position.y - 1);
                        }
                        else
                        {
                            tile.Type = downLeft;
                            tile.Direction = Direction.Up;
                            
                            if (currentPath[pathCount].Type == horizontal)
                                tile.Position = new Vector2Int(currentPath[pathCount].Position.x + 1,
                                    currentPath[pathCount].Position.y);
                            else if (currentPath[pathCount].Type == upRight)
                                tile.Position = new Vector2Int(currentPath[pathCount].Position.x + 2,
                                    currentPath[pathCount].Position.y + 1);
                            else if (currentPath[pathCount].Type == downRight)
                                tile.Position = new Vector2Int(currentPath[pathCount].Position.x + 2,
                                    currentPath[pathCount].Position.y);
                        }

                        break;

                    case Direction.Up: // if your moving up
                        if (Random.Range(0.0f, 10.0f) <= 7.0f)
                        {
                            tile.Type = upRight;
                            tile.Direction = Direction.Right;
                        }
                        else if (Random.Range(0.0f, 10.0f) <= 9.0f) // continue to check the past path types to determine where the next path should be placed
                        {
                            tile.Type = vertical;
                            tile.Direction = Direction.Up;
                        }
                        else
                        {
                            tile.Type = upLeft;
                            tile.Direction = Direction.Left;
                        }

                        break;
                    
                    case Direction.Left: // if your moving left
                        if (Random.Range(0.0f, 10.0f) <= 7.5f)
                        {
                            tile.Type = downRight;
                            tile.Direction = Direction.Up;
                        }
                        else if (Random.Range(0.0f, 10.0f) <= 9.0f)
                        {
                            tile.Type = horizontal;
                            tile.Direction = Direction.Left;
                        }
                        else
                        {
                            tile.Type = upRight;
                            tile.Direction = Direction.Down;
                        }

                        break;
                    
                    case Direction.Down: // if your moving down
                        if (Random.Range(0.0f, 10.0f) <= 7.0f)
                        {
                            tile.Type = downRight;
                            tile.Direction = Direction.Right;
                        }
                        else if (Random.Range(0.0f, 10.0f) <= 9.0f)
                        {
                            tile.Type = vertical;
                            tile.Direction = Direction.Down;
                        }
                        else
                        {
                            tile.Type = downLeft;
                            tile.Direction = Direction.Left;
                        }

                        break;
                }
            }
        }
    }
}
