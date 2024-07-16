using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathGen
{
    public class PathGeneration : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private int width;
        [SerializeField] private int height;
        [SerializeField] private TileBase tileFill;
        [SerializeField] private TileBase vertical;
        [SerializeField] private TileBase horizontal;
        [SerializeField] private TileBase upRight;
        [SerializeField] private TileBase upLeft;
        [SerializeField] private TileBase downRight;
        [SerializeField] private TileBase downLeft;
        [SerializeField] private Vector2Int startPosition;
        private Vector2Int _endPosition;
        private RandomPathGenerationRight _randomPathRight;
        // private RandomPathGenerationLeft _randomPathLeft;
        // private RandomPathGenerationUp _randomPathUp;
        // private RandomPathGenerationDown _randomPathDown;


        private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap map, TileBase tile)
        {
            foreach (var position in positions)
            {
                PaintSingleTile(map, tile, position);
            }
        }

        private void PaintSingleTile(Tilemap map, TileBase tile, Vector2Int position)
        {
            Vector3Int tilePosition = map.WorldToCell((Vector3Int)position);
            map.SetTile(tilePosition, tile);
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
            PaintSingleTile(tilemap, vertical, startPosition);
            
        }


        void Update()
        {
            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");

            if (Input.GetMouseButton(0) && (int)mouseX == startPosition.x && (int)mouseY == startPosition.y)
            {

            }
        }
    }
}
