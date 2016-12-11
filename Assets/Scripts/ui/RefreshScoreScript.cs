using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ui
{
    public class RefreshScoreScript : MonoBehaviour
    {
        public int TeamNo;
        private Text _teamScoreText;
        private MapGen _mapGenScript;

        private void Start()
        {
            _teamScoreText = gameObject.GetComponent<Text>();
            _mapGenScript = Camera.main.GetComponent<MapGen>();
        }

        private void Update()
        {
            if (TeamNo == 0)
            {
                _teamScoreText.text = _mapGenScript.Team0Score.ToString();
            }
            else
            {
                Assert.IsTrue(TeamNo == 1);
                _teamScoreText.text = _mapGenScript.Team1Score.ToString();
            }
        }
    }
}

