using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false; // Variável para controlar se o objeto está sendo arrastado
    private Vector3 offset; // Deslocamento entre o mouse e o objeto
    private Vector3 initialPosition; // Posição inicial do objeto
    private Vector3 gridSize = new Vector3(1f, 1f, 0f); // Tamanho do grid

    void Start()
    {
        initialPosition = transform.position; // Armazena a posição inicial do objeto
    }

    void OnMouseDown()
    {
        isDragging = true; // Define que o objeto está sendo arrastado ao clicar nele
        offset = transform.position - GetMouseWorldPosition(); // Calcula o deslocamento entre o mouse e o objeto
    }

    void OnMouseUp()
    {
        isDragging = false; // Define que o objeto não está mais sendo arrastado ao soltar o botão do mouse
        CheckAndSnapToSquares(); // Verifica e alinha o objeto aos quadrados do grid
    }

    void Update()
    {
        if (isDragging)
        {
            // Move o objeto enquanto estiver arrastando
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    private void CheckAndSnapToSquares()
    {
        bool snapped = true; // Flag para indicar se o objeto foi encaixado em todos os quadrados do grid
        Vector3 averagePosition = Vector3.zero; // Variável para calcular a posição média dos quadrados filhos

        foreach (Transform childSquare in transform)
        {
            bool childSnapped = false; // Flag para indicar se o quadrado filho foi encaixado em algum quadrado do grid

            // Verifica se há colisão com algum quadrado do grid
            Collider2D[] colliders = Physics2D.OverlapPointAll(childSquare.position);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("GridSquare"))
                {
                    // Se o quadrado filho estiver dentro do quadrado do grid, ajusta sua posição para se alinhar ao grid
                    if (collider.bounds.Contains(childSquare.position))
                    {
                        Vector3 snappedPosition = collider.transform.position;
                        snappedPosition.x += Mathf.Round((childSquare.position.x - snappedPosition.x) / gridSize.x) * gridSize.x;
                        snappedPosition.y += Mathf.Round((childSquare.position.y - snappedPosition.y) / gridSize.y) * gridSize.y;

                        childSquare.position = snappedPosition; // Define a nova posição do quadrado filho
                        childSnapped = true; // Marca que o quadrado filho foi encaixado em um quadrado do grid

                        // Adiciona a posição do quadrado filho à posição média
                        averagePosition += childSquare.position;
                        break;
                    }
                }
            }

            if (!childSnapped)
            {
                snapped = false; // Marca que pelo menos um quadrado filho não foi encaixado em um quadrado do grid
            }
        }

        if (!snapped)
        {
            transform.position = initialPosition; // Volta à posição inicial se pelo menos um dos quadrados filhos não foi encaixado
        }
        else
        {
            // Move o objeto pai para a posição média dos quadrados filhos
            transform.position = averagePosition / transform.childCount;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos); // Converte a posição do mouse de tela para o mundo
    }
}
