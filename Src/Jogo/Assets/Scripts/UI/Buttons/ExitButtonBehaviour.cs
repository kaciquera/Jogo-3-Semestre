using UnityEngine;

namespace Game
{
    public class ExitButtonBehaviour : ButtonBehaviour
    {
        public override void OnClick()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
