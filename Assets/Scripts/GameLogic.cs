using ui;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

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

public class GameLogic : MonoBehaviour
{
    public GameObject[] sceneObjectPrefabs;

    public int Team0Score { get; private set; }
    public int Team1Score { get; private set; }
    public bool GameStarted { get; private set; }
    public static int GameEndArtifactCount = 5;

    private Map _activeMap;
    private GameObject[] _characters;
    private StartGamePanelScript _startGamePanelScript;
    private EndGamePanelScript _endGamePanelScript;
    private bool _gameOnHold;
    
    private bool _gameEnded;

    private void Awake()
    {
        _startGamePanelScript = GameObject.FindGameObjectWithTag("StartGamePanel").GetComponent<StartGamePanelScript>();
        GameObject endGamePanel = GameObject.FindGameObjectWithTag("EndGamePanel");
        _endGamePanelScript = endGamePanel.GetComponent<EndGamePanelScript>();
    }

    // Use this for initialization
    private void Start ()
    {
        CreateMaps();
        PlayNextMap();
        PauseAction();
        GameStarted = false;
        _gameEnded = false;
    }

    // Update is called once per frame
    private void Update ()
    {
        if (_gameOnHold && !GameStarted)
        {
            bool canStart = true;
            for (int i = 0; i < _characters.Length; ++i)
            {
                if (_characters[i].GetComponent<Character>().CurrentMoveDir == Vector2.zero)
                {
                    canStart = false;
                    break;
                }
            }
            if (canStart)
            {
                ContinueAction();
                GameStarted = true;
                _startGamePanelScript.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Pause) && GameStarted && !_gameEnded)
        {
            if (_gameOnHold)
            {
                ContinueAction();
            }
            else
            {
                PauseAction();
            }
        }
	}

    private void PauseAction()
    {
        Time.timeScale = 0.0f;
        _gameOnHold = true;
    }

    private void ContinueAction()
    {
        Time.timeScale = 1.0f;
        _gameOnHold = false;
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
            {1,1,1,1,1,1,1,8,1,0,1,0,0,7,7,7},
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
        int[,] playerCoords = new int[4,2] { {14,10}, {17,10}, {14,12}, {17,12} };

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
                    case 8:
                        go = Instantiate(sceneObjectPrefabs[code - 1], coords, Quaternion.identity);
                        break;
                    default:
                        Assert.IsTrue(false);
                        break;
                }
            }
        }

        int playerCount = _activeMap.PlayerCoords.GetLength(0);
        _characters = new GameObject[playerCount];
        for (int i = 0; i < _activeMap.PlayerCoords.GetLength(0); ++i)
        {
            Vector2 coords = new Vector2(_activeMap.PlayerCoords[i, 0], _activeMap.PlayerCoords[i, 1]);
            _characters[i] = Instantiate(sceneObjectPrefabs[1+i], coords, Quaternion.identity);
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

    public void ReplenishSword(Vector2 swordPosition)
    {
        GameObject go = Instantiate(sceneObjectPrefabs[7], swordPosition, Quaternion.identity);
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

        if (Team0Score >= GameEndArtifactCount || Team1Score >= GameEndArtifactCount)
        {
            //end game trigger
            GameEnd();
        }
    }

    private void GameEnd()
    {
        //TODO stop the game, display a frame with buttons restart and return to menu
        PauseAction();
        _gameEnded = true;
        _endGamePanelScript.ShowEndGamePanel(Team0Score, Team1Score);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
