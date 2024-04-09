using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public partial class Item : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;
        [SerializeField] private Image itemImage;

        public bool[,] ItemSize { get; private set; }
        public Vector2Int SelectedGrid { get; set; }
        public ItemSlot MainSlot { get; private set; }
        public ItemSlot[] UsedSlots { get; private set; } = new ItemSlot[0];
        public ItemData ItemData => itemData;
        public Image ItemImage => itemImage;

        private void Start()
        {
            ItemSize = itemData.OriginalItemSize;
            itemImage.sprite = itemData.ItemSprite;
            name = $"Item [{itemData.ItemName}]";
            itemImage.alphaHitTestMinimumThreshold = 0.5f;
        }

        public void RotateToRight()
        {
            ItemSize = RotateMatrixCounterClockwise(ItemSize);
            transform.Rotate(Vector3.forward, -90f);
        }

        public void RotateToLeft()
        {
            ItemSize = RotateMatrix(ItemSize);
            transform.Rotate(Vector3.forward, 90f);
        }

        bool[,] RotateMatrix(bool[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            bool[,] rotatedMatrix = new bool[cols, rows];

            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    rotatedMatrix[cols - 1 - j, i] = matrix[i, j];
                }
            }

            return rotatedMatrix;
        }

        bool[,] RotateMatrixCounterClockwise(bool[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            bool[,] rotatedMatrix = new bool[cols, rows];

            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    rotatedMatrix[j, rows - 1 - i] = matrix[i, j];
                }
            }

            return rotatedMatrix;
        }

        public void SetSlot(ItemSlot slot, ItemSlot[] slots)
        {
            MainSlot = slot;
            UsedSlots = slots;
        }

        public void RemoveFromSlots()
        {
            foreach (ItemSlot slot in UsedSlots)
            {
                slot.ClearItemInSlot();
            }
            MainSlot = null;
            UsedSlots = new ItemSlot[0];
        }
    }

    public partial class Item
    {
#if UNITY_EDITOR

        private void OnValidate()
        {
            if (itemData == null) return;
            ItemSize = itemData.OriginalItemSize;
            itemImage.rectTransform.sizeDelta = new Vector2(itemData.GirdSizeInPixels,itemData.GirdSizeInPixels) * itemData.GridSize;
            if (itemImage != null)
            {
                itemImage.sprite = itemData.ItemSprite;
            }
            name = $"Item [{itemData.ItemName}]";
        }

        private void OnDrawGizmosSelected()
        {
            if (itemData == null) return;
            Color usedGridColor = Color.green;
            usedGridColor.a = 0.5f;

            float rectWidth = itemImage.rectTransform.rect.width;
            float rectHeight = itemImage.rectTransform.rect.height;

            float gridCenter = itemData.GirdSizeInPixels / 2;
            float xOffset = -rectWidth * itemImage.rectTransform.pivot.x + gridCenter;
            float yOffset = -rectHeight * itemImage.rectTransform.pivot.y + gridCenter;

            Vector2 offset = new Vector2(xOffset, yOffset);
            SlotUtils.DrawGridGizmos(itemImage.rectTransform, itemData.GridSize.x, itemData.GridSize.y, itemData.GirdSizeInPixels, itemData.OriginalItemSize, usedGridColor, offset);

            Vector3 gridOffset = new Vector3(SelectedGrid.x * itemData.GirdSizeInPixels + offset.x, SelectedGrid.y * itemData.GirdSizeInPixels + offset.y) * transform.root.localScale.x;
            Vector3 selectedCellCenter = itemImage.rectTransform.position + gridOffset;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(selectedCellCenter, 5);
        }
#endif
    }
}