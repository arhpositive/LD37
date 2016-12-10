﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MapGen : MonoBehaviour
{
    public GameObject[] sceneObjectPrefabs;

    public int[,] CurrentMap { get; private set; }

    // Use this for initialization
    private void Start ()
    {
        CreateMaps();
        PlayNextMap();
    }

    // Update is called once per frame
    private void Update ()
    {
		
	}

    private void CreateMaps()
    {
        int[,] map1 =
        {
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1},
            {1,1,1,0,1,1,1,1,0,1,0,0,0,0,0,0},
            {1,1,1,0,1,0,0,0,0,0,0,1,0,1,1,1},
            {1,1,1,0,1,0,1,1,1,1,0,1,0,0,0,0},
            {1,0,0,0,0,0,0,0,0,1,0,1,0,1,0,1},
            {1,0,1,1,0,1,1,1,0,1,0,1,0,1,0,1},
            {1,0,1,1,0,1,1,1,0,1,0,1,0,1,0,1},
            {1,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,0,1,1,1,0,1,1,1,1,1,0,1,1,0,1},
            {1,0,0,0,0,0,0,0,0,0,1,0,1,0,2,0},
            {1,1,1,1,1,1,1,0,1,0,1,0,0,0,0,0},
            {1,1,1,1,1,0,0,0,1,0,0,0,1,0,0,0},
            {1,1,1,1,1,0,1,0,1,0,1,0,1,0,4,0},
            {1,0,0,0,0,0,1,0,1,0,1,0,1,1,0,1},
            {1,0,1,1,1,0,1,0,0,0,1,0,0,0,0,1},
            {1,0,0,0,1,0,1,0,1,1,1,0,1,1,0,1},
            {1,0,1,0,1,0,1,0,1,0,0,0,1,1,0,0},
            {1,0,1,0,1,0,1,0,1,0,1,1,1,1,0,1},
            {1,0,1,0,1,0,1,0,1,0,1,1,1,1,0,1},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1},
            {1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
        };

        CurrentMap = map1;
    }

    private void PlayNextMap()
    {
        int yDim = CurrentMap.GetLength(0);
        for (int y = 0; y < yDim; ++y)
        {
            int xDim = CurrentMap.GetLength(1);
            for (int x = 0; x < xDim * 2; ++x)
            {
                bool useMirror = x >= xDim;

                int mapX = useMirror ? 2 * xDim - 1 - x : x;
                int mapY = yDim - (y + 1);

                GameObject go;
                Vector2 coords = new Vector2(x, y);
                int code = CurrentMap[mapY, mapX];
                switch (code)
                {
                    case 0:
                        break;
                    case 1:
                        go = Instantiate(sceneObjectPrefabs[code - 1], coords, Quaternion.identity) as GameObject;
                        break;
                    case 2:
                    case 4:
                        int index = useMirror ? code - 1 : code;
                        go = Instantiate(sceneObjectPrefabs[index], coords, Quaternion.identity) as GameObject;
                        break;
                    default:
                        Assert.IsTrue(false);
                        break;
                }
            }
        }
    }
}
