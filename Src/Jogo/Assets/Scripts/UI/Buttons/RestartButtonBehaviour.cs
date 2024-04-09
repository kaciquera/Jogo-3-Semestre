using UnityEngine.SceneManagement;

namespace Game
{
    public class RestartButtonBehaviour : ButtonBehaviour
    {
        public override void OnClick()
        {
            int buildId = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(buildId);
        }
    }
}
