using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public abstract class ToggleBehaviour : MonoBehaviour
    {
        protected Toggle toggle;

        protected virtual void Awake()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnToggle);
        }

        protected abstract void OnToggle(bool isOn);
    }
}
