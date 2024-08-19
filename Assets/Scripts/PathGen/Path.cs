using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace PathGen
{
    public abstract class Path : MonoBehaviour
    {

        [SerializeField] protected TileTypes tileTypes;
        [SerializeField] protected Tilemap tilemap;
        protected List<Tile> currentPath;
        protected Vector2Int Goal => new((int)Random.Range(8.0f, 15.9f), (int)Random.Range(4.0f, 10.9f));

        protected enum Direction
        {
            Right,
            Left, 
            Up, 
            Down
        }

        [Serializable]
        protected struct Tile
        {
            public Direction direction;
            public Vector2Int position;
            public TileBase type;
            [HideInInspector] public bool turnTile;
        }
        

        [Serializable]
        protected record TileTypes
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
        }
    }
}