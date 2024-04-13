using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] private Sprite defaultSlotSprite;
        [SerializeField] private Sprite hoveredSlotSprite;
        [SerializeField] private Sprite usedSlotSprite;

        private Image slotImage;
        private GridInventory grid;
        private Vector2Int gridPosition;

        public Item ItemInSlot { get; private set; }
        public bool IsOccupied => ItemInSlot != null;
        public GridInventory Grid => grid;

        public void Initialize(GridInventory grid, Vector2Int gridPosition, float slotSize)
        {
            slotImage = GetComponent<Image>();
            slotImage.sprite = defaultSlotSprite;
            this.grid = grid;
            this.gridPosition = gridPosition;
            RectTransform transform = GetComponent<RectTransform>();
            transform.sizeDelta = new Vector2(slotSize, slotSize);
        }

        public bool ValidateSlotsOnGrid(Item item)
        {
            return grid.ValidateSlots(gridPosition, item);
        }

        public void SetItemInSlot(Item item)
        {
            ItemInSlot = item;
            slotImage.sprite = usedSlotSprite;
        }

        public void ClearItemInSlot()
        {
            ItemInSlot = null;
            slotImage.sprite = defaultSlotSprite;
        }

        public void SetHoverState(bool isHovered)
        {
            if (IsOccupied) return;
            slotImage.sprite = isHovered ? hoveredSlotSprite : defaultSlotSprite;
        }

        public ItemSlot[] GetUsedSlots(Item item)
        {
            List<ItemSlot> slots = new List<ItemSlot>();
            bool[,] itemSize = item.ItemSize;

            for (int x = 0; x < itemSize.GetLength(0); x++)
            {
                for (int y = 0; y < itemSize.GetLength(1); y++)
                {
                    if (!itemSize[x, y]) continue;
                    ItemSlot slot = grid.GetSlot(gridPosition + new Vector2Int(x, y) - item.SelectedGrid);
                    if (slot != null)
                    {
                        slots.Add(slot);
                    }
                }
            }
            return slots.ToArray();
        }
    }
}