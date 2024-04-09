using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class LoadSceneButtonBehaviour : ButtonBehaviour
    {
        [SerializeField] private string sceneName;
        public override void OnClick()
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
