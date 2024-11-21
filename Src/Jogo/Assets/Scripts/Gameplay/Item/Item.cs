using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public partial class Item : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;
        [SerializeField] private Image itemImage;

        private RectTransform rectTransform;
        private Tween currentRotationTween;

        public bool[,] ItemSize { get; private set; }
        public Vector2Int SelectedGrid { get; set; }
        public ItemSlot MainSlot { get; private set; }
        public ItemSlot[] UsedSlots { get; private set; } = new ItemSlot[0];
        public ItemData ItemData => itemData;
        public Image ItemImage => itemImage;

        private void Awake()
        {
            rectTransform = itemImage.rectTransform;
        }

        private void Start()
        {
            ItemSize = itemData.OriginalItemSize;
            itemImage.sprite = itemData.ItemSprite;
            name = $"Item [{itemData.ItemName}]";
            itemImage.alphaHitTestMinimumThreshold = 0.5f;
        }

        public void RotateToRight(Vector2 cursorPosition, Camera eventCamera)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, cursorPosition, eventCamera, out Vector2 localCursor))
            {
                RotateAroundPoint(itemData.Pivot, -90f);
            }

            ItemSize = RotateMatrixCounterClockwise(ItemSize);
        }

        public void RotateToLeft(Vector2 cursorPosition, Camera eventCamera)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, cursorPosition, eventCamera, out Vector2 localCursor))
            {
                RotateAroundPoint(itemData.Pivot, 90f);
            }

            ItemSize = RotateMatrix(ItemSize);
        }


        private void RotateAroundPoint(Vector2 point, float angle)
        {
            if (currentRotationTween != null && currentRotationTween.IsActive() && currentRotationTween.IsPlaying())
            {
                currentRotationTween.Complete();
            }
            StartRotation(point, angle);
        }

        private void StartRotation(Vector2 point, float angle)
        {
            Vector2 pivotOffset = new Vector2(
                rectTransform.rect.width * (point.x - rectTransform.pivot.x),
                rectTransform.rect.height * (point.y - rectTransform.pivot.y)
            );

            Vector3 worldPoint = rectTransform.TransformPoint(pivotOffset);
            Vector3 currentRotation = rectTransform.eulerAngles;

            rectTransform.RotateAround(worldPoint, Vector3.forward, angle);
            Vector3 targetRotation = rectTransform.eulerAngles;

            rectTransform.eulerAngles = currentRotation;

            currentRotationTween = rectTransform.DORotate(targetRotation, 0.25f);
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

            if (itemImage != null && itemData.ItemSprite != null)
            {
                rectTransform = itemImage.rectTransform;
                Vector2Int size = new Vector2Int(itemData.GridSize.x,itemData.GridSize.y);
                rectTransform.sizeDelta = size * itemData.GirdSizeInPixels;
                itemImage.sprite = itemData.ItemSprite;
            }

            name = $"Item [{itemData.ItemName}]";
        }

        private void OnDrawGizmosSelected()
        {
            if (itemData == null) return;
            Color usedGridColor = Color.green;
            usedGridColor.a = 0.5f;

            float rectWidth = rectTransform.rect.width;
            float rectHeight = rectTransform.rect.height;

            float gridCenter = itemData.GirdSizeInPixels / 2;
            float xOffset = -rectWidth * rectTransform.pivot.x + gridCenter;
            float yOffset = -rectHeight * rectTransform.pivot.y + gridCenter;

            Vector2 offset = new Vector2(xOffset, yOffset);
            SlotUtils.DrawGridGizmos(rectTransform, itemData.GridSize.x, itemData.GridSize.y, itemData.GirdSizeInPixels, itemData.OriginalItemSize, usedGridColor, offset);

            Vector3 gridOffset = new Vector3(SelectedGrid.x * itemData.GirdSizeInPixels + offset.x, SelectedGrid.y * itemData.GirdSizeInPixels + offset.y) * transform.root.localScale.x;
            Vector3 selectedCellCenter = rectTransform.position + gridOffset;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(selectedCellCenter, 5);
        }
#endif
    }
}