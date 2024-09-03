using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathGen
{
    public class PathConfig : MonoBehaviour
    {
        public TileTypes tileTypes;
        public Tilemap tilemap;
        public Direction scriptDirection;
        public Tile startingTile;
    }
}