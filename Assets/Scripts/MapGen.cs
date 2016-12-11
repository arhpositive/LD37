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

    public int Team0Score { get; private set; }
    public int Team1Score { get; private set; }

    private Map _activeMap;
    private EndGamePanelScript _endGamePanelScript;

    // Use this for initialization
    private void Start ()
    {
        GameObject endGamePanel = GameObject.FindGameObjectWithTag("EndGamePanel");
        _endGamePanelScript = endGamePanel.GetComponent<EndGamePanelScript>();
        endGamePanel.SetActive(false);
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

        _activeMap = new Map(map1, playerCoords);
    }

    private void PlayNextMap()
    {
        Team0Score = 0;
        Team1Score = 0;

        int yDim = _activeMap.MapDesign.GetLength(0);
        for (int y = 0; y < yDim; ++y)
        {
            int xDim = _activeMap.MapDesign.GetLength(1);
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

        for (int i = 0; i < _activeMap.PlayerCoords.GetLength(0); ++i)
        {
            Vector2 coords = new Vector2(_activeMap.PlayerCoords[i, 0], _activeMap.PlayerCoords[i, 1]);
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
        int yDim = _activeMap.MapDesign.GetLength(0);
        int xDim = _activeMap.MapDesign.GetLength(1);
        bool useMirror = xCoord >= xDim;
        int mapX = useMirror ? 2 * xDim - 1 - xCoord : xCoord;
        int mapY = yDim - (yCoord + 1);

        return _activeMap.MapDesign[mapY, mapX];
    }

    public void ReplenishArtifact(Vector2 artifactPosition)
    {
        GameObject go = Instantiate(sceneObjectPrefabs[5], artifactPosition, Quaternion.identity);
    }

    public void ArtifactScored(int teamNo)
    {
        if (teamNo == 0)
        {
            Team0Score += 1;
        }
        else
        {
            Assert.IsTrue(teamNo == 1);
            Team1Score += 1;
        }

        if (Team0Score == 1 || Team1Score == 1) //TODO fix end game condition
        {
            //end game trigger
            //TODO stop the game, display a frame with buttons restart and return to menu
            _endGamePanelScript.ShowEndGamePanel(Team0Score, Team1Score);
        }
    }
}
