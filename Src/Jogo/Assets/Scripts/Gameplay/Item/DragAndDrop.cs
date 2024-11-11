using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

namespace Game
{
    public partial class DragAndDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Canvas canvas;

        private Item item;
        private RectTransform rectTransform;
        private RectTransform canvasRect;
        private CanvasGroup canvasGroup;
        private GraphicRaycaster raycaster;
        private ItemSlot lastHoveredSlot;
        private Vector3 initialPosition;
        private Vector3 lastPosition;
        private bool isDraging;
        private bool hasRotatedBeforeDrag;

        private void Awake()
        {
            item = GetComponent<Item>();
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            raycaster = GetComponent<GraphicRaycaster>();
            canvasRect = transform.parent.GetComponent<RectTransform>();
            raycaster = canvasRect.GetComponent<GraphicRaycaster>();
            initialPosition = rectTransform.anchoredPosition;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        private void Update()
        {
            if (isDraging && Input.GetMouseButtonDown(1))
            {
                RotateToRight();
            }

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            item.ItemData.PlayPickSound();
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                lastPosition = transform.position;
                AllignToGridPoint(eventData.position, eventData.pressEventCamera);
            }
            else
            {
                RotateToRight();
            }
        }


        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left && !isDraging && !hasRotatedBeforeDrag)
            {
                transform.position = lastPosition;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            canvasGroup.alpha = .6f;
            isDraging = true;
            canvasGroup.blocksRaycasts = false;
            transform.SetAsLastSibling();

            RemoveFromSlot();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDraging) return;
            if (eventData.button != PointerEventData.InputButton.Left) return;
            hasRotatedBeforeDrag = false;
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

            if (eventData.hovered.Count != 0)
            {
                Transform hoveredObject = eventData.pointerCurrentRaycast.gameObject.transform;
                if (lastHoveredSlot == null || hoveredObject != lastHoveredSlot.transform)
                {
                    ClearLastSlotHoveredState();
                    SetSlotHoveredStateOn(eventData);
                }
            }
            else
            {
                ClearLastSlotHoveredState();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left) return;

            isDraging = false;
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            ClearLastSlotHoveredState();
            
            if (eventData.hovered.Count != 0)
            {
                TryAddToSlot(eventData);
            }
            else if (!InsideCanvas())
            {
                ReturnToInitialPosition();
            }

        }

        private bool InsideCanvas()
        {
            Vector3[] corners = new Vector3[4];

            rectTransform.GetWorldCorners(corners);

            foreach (Vector3 corner in corners)
            {
                Vector2 localSpacePoint = transform.parent.InverseTransformPoint(corner);
                if (!canvasRect.rect.Contains(localSpacePoint))
                {
                    return false;
                }
            }

            RectTransform[] allUIElements = canvasRect.GetComponentsInChildren<RectTransform>();


            foreach (RectTransform element in allUIElements)
            {
                if (element == rectTransform) continue;
                
                if (element.CompareTag("Item") || element.CompareTag("Slot"))
                {
                    if (IsRectTransformOverlapping(rectTransform, element))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsRectTransformOverlapping(RectTransform rect1, RectTransform rect2)
        {
            Rect rect1World = GetWorldRect(rect1);
            Rect rect2World = GetWorldRect(rect2);

            return rect1World.Overlaps(rect2World); 
        }

        private Rect GetWorldRect(RectTransform rect)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            Vector3 bottomLeft = corners[0];
            Vector3 topRight = corners[2];

            return new Rect(bottomLeft, topRight - bottomLeft);
        }

        private void SetSlotHoveredStateOn(PointerEventData eventData)
        {
            if (TryGetSlot(eventData, out ItemSlot slot))
            {
                lastHoveredSlot = slot;

                ItemSlot[] slots = slot.GetUsedSlots(item);
                foreach (ItemSlot currentSlot in slots)
                {
                    currentSlot.SetHoverState(true);
                }
            }
        }

        private void ClearLastSlotHoveredState()
        {
            if (lastHoveredSlot == null) return;

            ItemSlot[] slots = lastHoveredSlot.GetUsedSlots(item);
            foreach (ItemSlot currentSlot in slots)
            {
                currentSlot.SetHoverState(false);
            }
            lastHoveredSlot = null;
        }

        private void TryAddToSlot(PointerEventData eventData)
        {
            if (TryGetSlot(eventData, out ItemSlot slot) && slot.ValidateSlotsOnGrid(item))
            {
                AllignToGridPoint(slot.transform.position, eventData.pressEventCamera);

                ItemSlot[] slots = slot.GetUsedSlots(item);
                foreach (ItemSlot currentSlot in slots)
                {
                    currentSlot.SetItemInSlot(item);
                }
                item.SetSlot(slot, slots);
                slot.Grid.CheckVictoryCondition();

                return;
            }

            ReturnToInitialPosition();
        }

        private void ReturnToInitialPosition()
        {
            rectTransform.DOAnchorPos(initialPosition, 0.25f);
        }

        private bool TryGetSlot(PointerEventData eventData, out ItemSlot slot)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = eventData.position;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            foreach (RaycastResult result in results)
            {
                GameObject resultObj = result.gameObject;
                if (resultObj.CompareTag("Slot"))
                {
                    slot = resultObj.GetComponent<ItemSlot>();
                    return slot;
                }
            }
            slot = null;
            return false;
        }

        private void RemoveFromSlot()
        {
            item.RemoveFromSlots();
        }

        private void RotateToRight()
        {

            if (item.MainSlot != null) return;
            ClearLastSlotHoveredState();
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            item.RotateToRight(pointerEventData.position, pointerEventData.pressEventCamera);
            SetSlotHoveredStateOn(pointerEventData);
            hasRotatedBeforeDrag = true;
            item.ItemData.PlayPickSound();
        }

        private void AllignToGridPoint(Vector2 position, Camera pressEventCamera)
        {
            Vector2 localClickPosition;
            RectTransform rectTransform = GetComponent<RectTransform>();

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, position, pressEventCamera, out localClickPosition))
            {
                float width = rectTransform.rect.width;
                float height = rectTransform.rect.height;
                float offsetX = width * rectTransform.pivot.x;
                float offsetY = height * rectTransform.pivot.y;
                int girdSizeInPixels = item.ItemData.GirdSizeInPixels;

                Vector2 adjustedClickPosition = GetPositionByRotation(localClickPosition, item.transform.eulerAngles.z);

                adjustedClickPosition.x = Mathf.Clamp(adjustedClickPosition.x, -width / 2, width / 2);
                adjustedClickPosition.y = Mathf.Clamp(adjustedClickPosition.y, -height / 2, height / 2);

                int gridX = Mathf.FloorToInt((adjustedClickPosition.x + offsetX) / girdSizeInPixels);
                int gridY = Mathf.FloorToInt((adjustedClickPosition.y + offsetY) / girdSizeInPixels);

                gridX = Mathf.Clamp(gridX, 0, (int)(width / girdSizeInPixels) - 1);
                gridY = Mathf.Clamp(gridY, 0, (int)(height / girdSizeInPixels) - 1);

                float centeredGridX = (gridX + 0.5f) * girdSizeInPixels - offsetX;
                float centeredGridY = (gridY + 0.5f) * girdSizeInPixels - offsetY;

                Vector2 offsetToCenterOfGrid = new Vector2(centeredGridX - adjustedClickPosition.x, centeredGridY - adjustedClickPosition.y);

                rectTransform.anchoredPosition -= offsetToCenterOfGrid;
                item.SelectedGrid = new Vector2Int(gridX, gridY);
            }
        }

        private Vector2 GetPositionByRotation(Vector2 position, float rotation)
        {
            float radianRotation = rotation * Mathf.Deg2Rad;

            float cos = Mathf.Cos(radianRotation);
            float sin = Mathf.Sin(radianRotation);

            float rotatedX = position.x * cos - position.y * sin;
            float rotatedY = position.x * sin + position.y * cos;

            return new Vector2(rotatedX, rotatedY);
        }
    }
}