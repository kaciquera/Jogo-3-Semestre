using Game;
using UnityEngine;
using UnityEngine.UI;
public class EnableOnVictory : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private bool isEnabledOnVictory;
    [SerializeField] private GameObject[] objectsToEnable;
    public Button nextLevelButton;
    

    private void Awake()
    {
        GridInventory.OnVictory += OnVictory;
        
        if(PlayerPrefs.GetInt("CurrentLevel") > level)
        {
            nextLevelButton.interactable = true;
        }
        else
        {
            nextLevelButton.interactable = false;
        }
    }

    private void OnDestroy()
    {
        GridInventory.OnVictory -= OnVictory;
    }

    private void OnVictory()
    {
        PersistentData.Instance.DebugTest();
        PlayerPrefs.SetInt("CurrentLevel", level);
        foreach (var item in objectsToEnable)
        {
            item.SetActive(isEnabledOnVictory);
        }
    }
}
