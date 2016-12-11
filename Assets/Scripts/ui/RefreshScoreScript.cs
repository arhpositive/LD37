using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ui
{
    public class RefreshScoreScript : MonoBehaviour
    {
        public int TeamNo;
        private Text _teamScoreText;
        private GameLogic _gameLogicScript;

        private void Start()
        {
            _teamScoreText = gameObject.GetComponent<Text>();
            _gameLogicScript = Camera.main.GetComponent<GameLogic>();
        }

        private void Update()
        {
            if (TeamNo == 0)
            {
                _teamScoreText.text = _gameLogicScript.Team0Score.ToString();
            }
            else
            {
                Assert.IsTrue(TeamNo == 1);
                _teamScoreText.text = _gameLogicScript.Team1Score.ToString();
            }
        }
    }
}

