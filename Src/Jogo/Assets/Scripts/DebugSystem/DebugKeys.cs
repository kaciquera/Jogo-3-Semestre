using UnityEngine;

namespace Game.DebugSystem
{
    [System.Serializable]
    public struct DebugKeys
    {
        [SerializeField] private KeyCode[] prefixKeys;
        [SerializeField] private KeyCode mainKey;

        public bool CheckInputs()
        {
            if (mainKey == KeyCode.None) return false;

            foreach (KeyCode key in prefixKeys)
            {
                if (!Input.GetKey(key)) return false;
            }
      
            return Input.GetKeyDown(mainKey);
        }
    }
}