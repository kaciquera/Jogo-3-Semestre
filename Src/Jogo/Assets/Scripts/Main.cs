using UnityEngine;

namespace Game
{
    public class Main : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void CreateMain()
        {
            GameObject main = Instantiate(Resources.Load("Main") as GameObject);
            DontDestroyOnLoad(main);
        }
    }
}
