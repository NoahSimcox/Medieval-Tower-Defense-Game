using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace PathGen
{
    public class PathExecution : Path
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private TileBase tileFill;
        [SerializeField] private Tile initialTile;
        [SerializeField] private TileTypes tileTypes;
        [SerializeField] private Vector2Int startPosition;
        private Vector2Int _endPosition;
        private RandomPathGeneration _randomPath;
        

        private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap map, TileBase tileType)
        {
            foreach (var position in positions)
            {
                PaintSingleTile(map, tileType, position);
            }
        }

        private void PaintSingleTile(Tilemap map, TileBase tileType, Vector2Int position)
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

        void Start()
        {
            
        }


        void Update()
        {
            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");

            if (Input.GetMouseButton(0) && (int)mouseX == startPosition.x && (int)mouseY == startPosition.y)
            {
            //when the start position is clicked
            }
        }
    }
}
