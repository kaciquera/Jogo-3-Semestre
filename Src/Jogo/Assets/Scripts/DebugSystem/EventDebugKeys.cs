using UnityEngine;
using UnityEngine.Events;

namespace Game.DebugSystem
{
    [System.Serializable]
    public struct EventDebugKeys
    {
        [SerializeField] private KeyCode[] prefixKeys;
        [SerializeField] private KeyCode mainKey;

        [SerializeField] private UnityEvent OnTrigger;

        public bool CheckInputs()
        {
            if (mainKey == KeyCode.None) return false;

            foreach (KeyCode key in prefixKeys)
            {
                if (!Input.GetKey(key)) return false;
            }

            bool isTriggered = Input.GetKeyDown(mainKey);

            if (isTriggered)
            {
                OnTrigger?.Invoke();
            }

            return isTriggered;
        }
    }
}