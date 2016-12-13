using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace ui
{
    public class HighlightOnPressScript : MonoBehaviour
    {
        private Button[] _keyHelpButtons;

        // Use this for initialization
        void Start ()
        {
            _keyHelpButtons = gameObject.GetComponentsInChildren<Button>();
        }
	
        // Update is called once per frame
        void Update () {
		
        }

        public void HighlightButtonByMoveDir(Vector2 moveDir)
        {
            ClearHighlights();
            int index;
            if (moveDir == Vector2.up)
            {
                index = 0;
            }
            else if (moveDir == Vector2.down)
            {
                index = 1;
            }
            else if (moveDir == Vector2.right)
            {
                index = 2;
            }
            else
            {
                index = 3;
                Assert.IsTrue(moveDir == Vector2.left);
            }
            _keyHelpButtons[index].GetComponent<Image>().color = _keyHelpButtons[index].colors.pressedColor;
        }

        private void ClearHighlights()
        {
            foreach (Button t in _keyHelpButtons)
            {
                t.GetComponent<Image>().color = t.colors.normalColor;
            }
        }
    }
}
