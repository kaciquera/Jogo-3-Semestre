using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SquareController : MonoBehaviour
{
    public bool isOccupied = false; // Verifica se a água está ocupando este quadrado
    private SpriteRenderer spriteRenderer; // Referência para o componente SpriteRenderer

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtém o componente SpriteRenderer
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            // Verifica se a água ocupa este quadrado e o próximo quadrado
            if (!isOccupied && IsOccupiedByWater(collision.gameObject))
            {
                isOccupied = true; // Marca este quadrado como ocupado pela água
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            isOccupied = false; // Marca este quadrado como não ocupado pela água
        }
    }

    // Método para verificar se este quadrado e o próximo estão ocupados pela água
    private bool IsOccupiedByWater(GameObject water)
    {
        // Verifica se a posição Y da água está alinhada com a posição Y deste quadrado
        if (Mathf.Approximately(water.transform.position.y, transform.position.y))
        {
            // Verifica se a posição X da água está alinhada com a posição X deste quadrado ou o próximo
            if (Mathf.Approximately(water.transform.position.x, transform.position.x) ||
                Mathf.Approximately(water.transform.position.x, transform.position.x + 1))
            {
                return true; // A água ocupa este quadrado e o próximo quadrado
            }
        }
        return false;
    }
}
