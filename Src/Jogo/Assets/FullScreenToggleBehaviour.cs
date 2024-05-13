using UnityEngine;

namespace Game
{
    public class FullScreenToggleBehaviour : ToggleBehaviour
    {
        private void Start()
        {
            toggle.SetIsOnWithoutNotify(Screen.fullScreen);
        }

        protected override void OnToggle(bool isOn)
        {
            Screen.fullScreen = isOn;
        }
    }
}
