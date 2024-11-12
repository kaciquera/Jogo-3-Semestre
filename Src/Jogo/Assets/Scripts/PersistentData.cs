using UnityEngine;

namespace Game
{
    public class PersistentData : MonoBehaviour
    {
        private static PersistentData instance;
        public static PersistentData Instance => instance;

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        public void DebugTest()
        {
            Debug.Log("test");
        }
    }
}