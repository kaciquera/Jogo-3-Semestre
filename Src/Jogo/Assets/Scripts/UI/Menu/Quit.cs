using UnityEngine;

public class ConfirmarSaida : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToEnable; // Array de GameObjects a serem ativados

    public void AtivarPaineis()
    {
        // Ativa todos os objetos no array
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }
}
