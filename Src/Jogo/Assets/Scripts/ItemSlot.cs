using UnityEngine;
using static UnityEditor.Progress;

namespace Game
{
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] private GameObject occupiedImage;
        private Grid grid;
        private Vector2Int gridPosition;
        public Item ItemInSlot { get; private set; }
        public bool IsOccupied => ItemInSlot != null;

        public void Initialize(Grid grid, Vector2Int gridPosition, float slotSize)
        {
            this.grid = grid;
            this.gridPosition = gridPosition;
            RectTransform transform = GetComponent<RectTransform>();
            transform.sizeDelta = new Vector2(slotSize, slotSize);
        }

        public bool CheckPosition(Item item)
        {
            return grid.ValidateSlots(gridPosition, item.ItemSize , item.ItemOrigin);
        }

        public void SetItemInSlot(Item item)
        {
            ItemInSlot = item;
            occupiedImage.SetActive(true);
        }

        public void SetItem(Item item)
        {
            Debug.Log(ItemInSlot);
            //SetItemInSlot(item);
            item.transform.position = transform.position;
            bool[,] itemSize = item.ItemSize;

            for (int x = 0; x < itemSize.GetLength(0); x++)
            {
                for (int y = 0; y < itemSize.GetLength(1); y++)
                {
                    if (!itemSize[x, y]) continue;
                    Vector2Int slotIndex = gridPosition + new Vector2Int(x, y) - item.ItemOrigin;
                    grid.SetSlotItem(slotIndex, item);
                    Debug.Log(slotIndex);
                }
            }
        }

        public void ClearItemInSlot()
        {
            ItemInSlot = null;
            occupiedImage.SetActive(false);
        }

        public void ClearItem()
        {
            Debug.Log(ItemInSlot);
            bool[,] itemSize = ItemInSlot.ItemSize;

            for (int x = itemSize.GetLength(0) - 1; x >= 0; x--)
            {
                for (int y = itemSize.GetLength(1) - 1; y >= 0; y--)
                {
                    if (!itemSize[x, y]) continue;
                    Vector2Int slotIndex = gridPosition + new Vector2Int(x, y) - ItemInSlot.ItemOrigin;
                    grid.ClearSlotItem(slotIndex);
                    Debug.Log(slotIndex);
                }
            }
            ClearItemInSlot();
        }
    }
}