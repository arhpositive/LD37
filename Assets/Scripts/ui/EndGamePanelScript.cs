using UnityEngine;
using UnityEngine.UI;

public class EndGamePanelScript : MonoBehaviour
{
    private Image _endGamePanelImage;
    private Text[] _endGameTexts;
    private GameLogic _gameLogicScript;

    public Color Team0Color;
    public Color Team1Color;

    // Use this for initialization
    void Start ()
    {
        _endGameTexts = gameObject.GetComponentsInChildren<Text>();
        _endGamePanelImage = gameObject.GetComponent<Image>();
        _gameLogicScript = Camera.main.GetComponent<GameLogic>();
        gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    public void ShowEndGamePanel(int team0Score, int team1Score)
    {
        _endGameTexts[0].text = "Congratulations to Team";
        if (team0Score > team1Score)
        {
            _endGamePanelImage.color = Team0Color;
            
            _endGameTexts[0].text += " Cyan!";
        }
        else
        {
            _endGamePanelImage.color = Team1Color;
            _endGameTexts[0].text += " Yellow!";
        }
        _endGameTexts[1].text = team0Score.ToString() + " - " + team1Score.ToString();
        gameObject.SetActive(true);
    }

    public void OnRestartButtonClicked()
    {
        _gameLogicScript.RestartGame();
    }
}
