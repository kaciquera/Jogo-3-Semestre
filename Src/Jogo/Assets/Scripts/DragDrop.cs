using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Canvas canvas;

        private Item item;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private Vector3 initialPosition;
        private bool isDragin;

        private void Awake()
        {
            item = GetComponent<Item>();
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            initialPosition = rectTransform.anchoredPosition; // Armazena a posição inicial do item
        }

        private void Update()
        {
            if (isDragin && Input.GetMouseButtonDown(1))
            {
                RotateRight();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("OnBeginDrag");
            canvasGroup.alpha = .6f;
            isDragin = true;
            canvasGroup.blocksRaycasts = false;

            if(item.Slot != null)
            {
                item.Slot.ClearItem();
                item.SetSlot(null);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            //Debug.Log("OnDrag");
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            isDragin = false;
            Debug.Log("OnEndDrag");
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            if (eventData.hovered.Count == 0) return;
            // Verifica se o item está sobre um slot válido
            Transform hoveredSlot = eventData.pointerCurrentRaycast.gameObject.transform;
            if (hoveredSlot != null && hoveredSlot.TryGetComponent(out ItemSlot slot))
            {

                if (!slot.IsOccupied && slot.CheckPosition(item))
                {
                    transform.position = hoveredSlot.position;
                    slot.SetItem(item);
                    item.SetSlot(slot);
                }
                else
                {
                    rectTransform.anchoredPosition = initialPosition;
                }
            }
            else
            {
                // Se o item não estiver sobre um slot válido, retorna o item à sua posição inicial
                rectTransform.anchoredPosition = initialPosition;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // Verifica se o botão direito do mouse foi clicado
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                // Rotaciona a imagem em 90 graus no eixo Z (para rotação bidimensional)
                RotateRight();
            }
        }

        private void RotateRight()
        {
            rectTransform.Rotate(Vector3.forward, -90f);
            item.RotateToRight();
        }
    }
}