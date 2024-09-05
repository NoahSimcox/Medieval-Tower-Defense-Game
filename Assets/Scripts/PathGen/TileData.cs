using System;
using System.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathGen
{
    
    public enum Direction
    {
        Right,
        Left, 
        Up,
        Down
    }
    
    [Serializable]
    public struct Tile
    {
        public Direction direction;
        public Vector2Int position;
        public TileBase type;
        [HideInInspector] public bool turnTile;
    }
        

    [Serializable]
    public record TileTypes
    {
        public TileBase vertical;
        public TileBase horizontal;
        public RuleTile upRight;
        public RuleTile upLeft;
        public RuleTile downRight;
        public RuleTile downLeft;
        public RuleTile rightUp;
        public RuleTile leftUp;
        public RuleTile rightDown;
        public RuleTile leftDown;

        public TileBase grassTile;
        public RuleTile rockBorder;
    }
}