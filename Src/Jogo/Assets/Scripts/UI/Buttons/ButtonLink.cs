using UnityEngine;
using UnityEngine.UI;

public class ButtonLink : MonoBehaviour
{
    public string url = "https://www.google.com.br";

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenURL);
    }

    void OpenURL()
    {
        Application.OpenURL(url);
    }
}
