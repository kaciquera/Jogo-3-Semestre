using TMPro;
using UnityEngine;

namespace Game.DebugSystem
{
    public class FPSText : MonoBehaviour
    {
        private TextMeshProUGUI text;
        private float deltaTime;

        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            text.text = $"{fps:0.}<size=-15>FPS</size>";
        }
    }
}