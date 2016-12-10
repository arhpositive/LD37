using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MapGen : MonoBehaviour
{
    public GameObject WallPrefab;

    public int[,] CurrentMap { get; private set; }

    // Use this for initialization
    private void Start ()
    {
        CreateMaps();
        PlayNextMap();
    }

    // Update is called once per frame
    private void Update () {
		
	}

    private void CreateMaps()
    {
        int[,] map1 =
        {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0},
            {0,0,0,1,0,0,0,0,1,0,1,1,1,1,1,1},
            {0,0,0,1,0,1,1,1,1,1,1,0,1,0,0,0},
            {0,0,0,1,0,1,0,0,0,0,1,0,1,1,1,1},
            {0,1,1,1,1,1,1,1,1,0,1,0,1,0,1,0},
            {0,1,0,0,1,0,0,0,1,0,1,0,1,0,1,0},
            {0,1,0,0,1,0,0,0,1,0,1,0,1,0,1,0},
            {0,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1},
            {0,1,0,0,0,1,0,0,0,0,0,1,0,0,1,0},
            {0,1,1,1,1,1,1,1,1,1,0,1,0,1,1,1},
            {0,0,0,0,0,0,0,1,0,1,0,1,1,1,1,1},
            {0,0,0,0,0,1,1,1,0,1,1,1,0,1,1,1},
            {0,0,0,0,0,1,0,1,0,1,0,1,0,1,1,1},
            {0,1,1,1,1,1,0,1,0,1,0,1,0,0,1,0},
            {0,1,0,0,0,1,0,1,1,1,0,1,1,1,1,0},
            {0,1,1,1,0,1,0,1,0,0,0,1,0,0,1,0},
            {0,1,0,1,0,1,0,1,0,1,1,1,0,0,1,1},
            {0,1,0,1,0,1,0,1,0,1,0,0,0,0,1,0},
            {0,1,0,1,0,1,0,1,0,1,0,0,0,0,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {0,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0},
            {0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}
        };

        CurrentMap = map1;
    }

    private void PlayNextMap()
    {
        print("yDim: " + CurrentMap.GetLength(0) + " xDim: " + CurrentMap.GetLength(1));
        int yDim = CurrentMap.GetLength(0);
        for (int y = 0; y < yDim; ++y)
        {
            int xDim = CurrentMap.GetLength(1);
            for (int x = 0; x < xDim * 2; ++x)
            {
                bool useMirror = x >= xDim;

                int mapX = useMirror ? 2 * xDim - 1 - x : x;
                int mapY = yDim - (y + 1);
                
                Vector2 coords = new Vector2(x, y);
                switch (CurrentMap[mapY, mapX])
                {
                    case 0:
                        GameObject go = Instantiate(WallPrefab, coords, Quaternion.identity) as GameObject;
                        break;
                    case 1:
                        break;
                    default:
                        Assert.IsTrue(false);
                        break;
                }
            }
        }
    }
}
