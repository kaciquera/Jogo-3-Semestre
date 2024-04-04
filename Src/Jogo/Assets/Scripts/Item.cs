using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;
        [SerializeField] private Image itemImage;


        private int rotationAmount;
        private bool[,] currentItemSize;
        public bool[,] ItemSize => currentItemSize;
        public Vector2Int ItemOrigin { get; private set; }
        public ItemSlot Slot { get; private set; }

        private void OnValidate()
        {
            if (itemData == null) return;
            currentItemSize = itemData.OriginalItemSize;
            if (itemImage != null)
            {
                itemImage.sprite = itemData.ItemSprite;
            }
            name = $"Item [{itemData.ItemName}]";
        }

        private void Start()
        {
            currentItemSize = itemData.OriginalItemSize;
            itemImage.sprite = itemData.ItemSprite;
            name = $"Item [{itemData.ItemName}]";
        }

        public void SetSlot(ItemSlot slot)
        {
            Slot = slot;
        }

        public void RotateToRight()
        {
            currentItemSize = RotateMatrix(currentItemSize);
            rotationAmount++;
            ItemOrigin = GetBottomLeftPositionAfterRotation(itemData.OriginalItemSize, rotationAmount);
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

        Vector2Int GetBottomLeftPositionAfterRotation(bool[,] originalMatrix, int rotations)
        {
            int rows = originalMatrix.GetLength(0);
            int cols = originalMatrix.GetLength(1);

            int newRowIndex = (rotations % 4 == 0) ? rows - 1 : (rotations % 4 == 1) ? cols - 1 : 0;
            int newColIndex = (rotations % 4 == 0) ? cols - 1 : (rotations % 4 == 1) ? 0 : rows - 1;

            return new Vector2Int(newRowIndex, newColIndex);
        }
    }
}