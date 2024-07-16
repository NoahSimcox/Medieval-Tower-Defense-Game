using System.Collections;
using System.Collections.Generic;
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

                switch (currentPath[pathCount].Orientation) // change all this to switch based on direction not orientation. direction is far more descriptive and only leaves two possible turns and one straight movement from each direction.
                {
                    case Orientation.Horizontal:
                        if (Random.Range(0.0f, 10.0f) <= 6.0f)
                        {
                            tile.Type = horizontal;
                            tile.Orientation = Orientation.Horizontal;
                        }
                        else if (Random.Range(0.0f, 10.0f) <= 8.0f)
                        {
                            tile.Type = upLeft;
                            tile.Orientation = Orientation.Vertical;
                        }
                        else
                        {
                            tile.Type = downLeft;
                            tile.Orientation = Orientation.Vertical;
                        }

                        break;

                    case Orientation.Vertical:
                        if (Random.Range(0.0f, 10.0f) <= 6.0f)
                        {
                            tile.Type = vertical;
                            tile.Orientation = Orientation.Vertical;
                        }
                        else if (Random.Range(0.0f, 10.0f) <= 8.0f)
                        {
                            tile.Type = upLeft;
                            tile.Orientation = Orientation.Horizontal;
                        }
                        else
                        {
                            tile.Type = upRight;
                            tile.Orientation = Orientation.Horizontal;
                        }

                        break;
                }

                currentPath[pathCount]
            }
        }
    }
}
