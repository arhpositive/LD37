using UnityEngine;
using UnityEngine.UI;

public class EndGamePanelScript : MonoBehaviour
{
    private Text _endGameText;
    private Image _endGamePanelImage;

    public Color Team0Color;
    public Color Team1Color;

    // Use this for initialization
    void Start ()
    {
        _endGameText = gameObject.GetComponentInChildren<Text>();
        //TODO use a different way to get both texts!!
        //gameObject.GetComponent<Text>();
        _endGamePanelImage = gameObject.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    public void ShowEndGamePanel(int team0Score, int team1Score)
    {
        _endGameText.text = "Congratulations to ";
        if (team0Score > team1Score)
        {
            _endGamePanelImage.color = Team0Color;
            _endGameText.text += " Team Yellow!";
        }
        else
        {
            _endGamePanelImage.color = Team1Color;
            _endGameText.text += " Team Cyan!";
        }
        gameObject.SetActive(true);
    }
}
