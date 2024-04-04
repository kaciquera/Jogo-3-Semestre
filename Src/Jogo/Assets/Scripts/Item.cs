using UnityEngine;

namespace Game
{
    public class Item : MonoBehaviour
    {
        [SerializeField]
        private bool[,] originalItemSize = new bool[3, 3]
        {
            { true , true, false},
            { true , false, false},
            { true , false, false}
        };

        private int rotationAmount;
        private bool[,] currentItemSize;
        public bool[,] ItemSize => currentItemSize;
        public Vector2Int ItemOrigin { get; private set; }
        public ItemSlot Slot { get; private set; }

        private void Start()
        {
            currentItemSize = originalItemSize;
        }

        public void SetSlot(ItemSlot slot)
        {
            Slot = slot;
        }

        public void RotateToRight()
        {
            currentItemSize = RotateMatrix(currentItemSize);
            rotationAmount++;
            ItemOrigin = GetBottomLeftPositionAfterRotation(originalItemSize, rotationAmount);
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