using UnityEngine;
using UnityEngine.Events;

namespace Game.DebugSystem
{
    [System.Serializable]
    public struct LogCommand
    {
        [SerializeField] private string text;
        [SerializeField] private string callbackText;
        [SerializeField] private string tooltip;
        [SerializeField] private UnityEvent Callback;

        public string Text => text;
        public string CallbackText => callbackText;
        public bool HasCallbackText => !string.IsNullOrEmpty(callbackText);

        public bool ValidateWithCallback(string message)
        {
            bool isValid = text == message;
            if (isValid)
            {
                Callback?.Invoke();
            }

            return isValid;
        }

        public bool Validate(string message)
        {
            return text == message;
        }
    }
}