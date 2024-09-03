using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathGen
{
    public class Path : MonoBehaviour
    {
        [SerializeField] private PathConfig pathConfig;
        
        protected TileTypes TileTypes => pathConfig.tileTypes;
        protected Direction ScriptDirection => pathConfig.scriptDirection;
        protected Tilemap Tilemap => pathConfig.tilemap;
        protected Tile StartingTile => pathConfig.startingTile;
        
        protected Vector2Int Goal;
        [HideInInspector] public List<Tile> currentPath = new List<Tile>();
    }
}