using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;

public class Map
{
    public int[,] MapDesign;
    public int[,] PlayerCoords;

    public Map(int[,] mapDesign, int[,] playerCoords)
    {
        MapDesign = mapDesign;
        PlayerCoords = playerCoords;
    }
}

public class MapGen : MonoBehaviour
{
    public GameObject[] sceneObjectPrefabs;

    private Map ActiveMap;

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
            {1,1,1,6,0,0,0,0,0,1,1,1,1,1,1,1},
            {1,1,1,0,1,1,1,1,0,1,0,0,0,0,0,0},
            {1,1,1,0,1,0,0,0,0,0,0,1,0,1,1,1},
            {1,1,1,0,1,0,1,1,1,1,0,1,0,0,0,0},
            {1,0,0,0,0,0,0,0,0,1,0,1,0,1,0,1},
            {1,0,1,1,0,1,1,1,0,1,0,1,0,1,0,1},
            {1,0,1,1,0,1,1,1,0,1,0,1,0,1,0,1},
            {1,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,0,1,1,1,0,1,1,1,1,1,0,1,1,0,1},
            {1,0,0,0,0,0,0,0,0,0,1,0,1,7,7,7},
            {1,1,1,1,1,1,1,0,1,0,1,0,0,7,7,7},
            {1,1,1,1,1,0,0,0,1,0,0,0,1,7,7,7},
            {1,1,1,1,1,0,1,0,1,0,1,0,1,7,7,7},
            {1,0,0,0,0,0,1,0,1,0,1,0,1,1,0,1},
            {1,0,1,1,1,0,1,0,0,0,1,0,0,0,0,1},
            {1,0,0,0,1,0,1,0,1,1,1,0,1,1,0,1},
            {1,0,1,0,1,0,1,0,1,0,0,0,1,1,0,0},
            {1,0,1,0,1,0,1,0,1,0,1,1,1,1,0,1},
            {1,0,1,0,1,0,1,0,1,0,1,1,1,1,0,1},
            {1,6,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1},
            {1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
        };
        int[,] playerCoords = new int[4,2] { {14,10}, {17,10}, {14,13}, {17,13} };

        ActiveMap = new Map(map1, playerCoords);
    }

    private void PlayNextMap()
    {

        int yDim = ActiveMap.MapDesign.GetLength(0);
        for (int y = 0; y < yDim; ++y)
        {
            int xDim = ActiveMap.MapDesign.GetLength(1);
            for (int x = 0; x < xDim * 2; ++x)
            {
                GameObject go;
                Vector2 coords = new Vector2(x, y);
                int code = GetMapValueFromCoords(x, y);
                switch (code)
                {
                    case 0:
                        break;
                    case 1:
                    case 6:
                    case 7:
                        go = Instantiate(sceneObjectPrefabs[code - 1], coords, Quaternion.identity);
                        break;
                    default:
                        Assert.IsTrue(false);
                        break;
                }
            }
        }

        for (int i = 0; i < ActiveMap.PlayerCoords.GetLength(0); ++i)
        {
            Vector2 coords = new Vector2(ActiveMap.PlayerCoords[i, 0], ActiveMap.PlayerCoords[i, 1]);
            GameObject player = Instantiate(sceneObjectPrefabs[1+i], coords, Quaternion.identity); //TODO fix this stupid index
        }
        
    }

    public bool IsMovementPossible(int x, int y)
    {
        if (GetMapValueFromCoords(x, y) == 1)
        {
            return false;
        }
        return true;
    }

    private int GetMapValueFromCoords(int xCoord, int yCoord)
    {
        int yDim = ActiveMap.MapDesign.GetLength(0);
        int xDim = ActiveMap.MapDesign.GetLength(1);
        bool useMirror = xCoord >= xDim;
        int mapX = useMirror ? 2 * xDim - 1 - xCoord : xCoord;
        int mapY = yDim - (yCoord + 1);

        return ActiveMap.MapDesign[mapY, mapX];
    }

    public void ReplenishArtifact(Vector2 artifactPosition)
    {
        GameObject go = Instantiate(sceneObjectPrefabs[5], artifactPosition, Quaternion.identity);
    }
}
