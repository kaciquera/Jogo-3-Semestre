using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuShortcut : MonoBehaviour
{
    public KeyCode menuKey = KeyCode.Escape; 

    void Update()
    {
        
        if (Input.GetKeyDown(menuKey))
        {
            
            SceneManager.LoadScene("Sceve_Level_16");
        }
    }
}
