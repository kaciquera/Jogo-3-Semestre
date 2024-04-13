using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.DebugSystem
{
    public class DebugManager : MonoBehaviour
    {
        [SerializeField] private DebugKeys restartCommand;
        [SerializeField] private EventDebugKeys debugMenuCommand;

        private void Update()
        {
            if (restartCommand.CheckInputs())
            {
                int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
                SceneManager.LoadScene(sceneBuildIndex);
            }
            debugMenuCommand.CheckInputs();
        }
    }
}