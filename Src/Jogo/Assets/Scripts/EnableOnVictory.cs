using Game;
using UnityEngine;

public class EnableOnVictory : MonoBehaviour
{
    [SerializeField] private bool isEnabledOnVictory;
    [SerializeField] private GameObject[] objectsToEnable;

    private void Awake()
    {
        GridInventory.OnVictory += OnVictory;
    }

    private void OnDestroy()
    {
        GridInventory.OnVictory -= OnVictory;
    }

    private void OnVictory()
    {
        foreach (var item in objectsToEnable)
        {
            item.SetActive(isEnabledOnVictory);
        }
    }
}
