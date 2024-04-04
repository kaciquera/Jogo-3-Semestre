using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Game
{
    public class Grid : MonoBehaviour
    {
        [Header("Config")]
        [Min(1)]
        [SerializeField] private int width = 1;
        [Min(1)]
        [SerializeField] private int height = 1;
        [Range(100, 500)]
        [SerializeField] private float cellSize = 108;
        [SerializeField] private Vector3 offset;

        [Header("References")]
        [SerializeField] private ItemSlot slotPrefab;

        private readonly Dictionary<Vector2Int, ItemSlot> slotByGrid = new Dictionary<Vector2Int, ItemSlot>();
        private RectTransform rectTransform;

        private void Start()
        {
            rectTransform = transform.root.GetComponent<RectTransform>();
            CreateGrid();
        }

        private void CreateGrid()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 cellPosition = GetWorldPosition(x, y);
                    ItemSlot itemSlot = Instantiate(slotPrefab, cellPosition, Quaternion.identity, transform);
                    Vector2Int gridPosition = new Vector2Int(x, y);
                    itemSlot.name = $"Slot [{gridPosition}]";
                    itemSlot.Initialize(this, gridPosition, cellSize);
                    slotByGrid.Add(gridPosition, itemSlot);
                }
            }
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * (cellSize * rectTransform.localScale.x) + transform.position + offset;
        }

        public void SetSlotItem(Vector2Int gridPosition, Item item)
        {
            slotByGrid[gridPosition].SetItemInSlot(item);
        }

        public void ClearSlotItem(Vector2Int gridPosistion)
        {
            slotByGrid[gridPosistion].ClearItemInSlot();
        }

        public bool ValidateSlot(Vector2Int gridPosition)
        {
            return slotByGrid.ContainsKey(gridPosition) && !slotByGrid[gridPosition].IsOccupied;
        }

        public bool ValidateSlots(Vector2Int targetGridPosition, bool[,] itemSize, Vector2Int itemOrigen)
        {
            for (int x = 0; x < itemSize.GetLength(0); x++)
            {
                for (int y = 0; y < itemSize.GetLength(1); y++)
                {
                    if (itemSize[x, y] && !ValidateSlot(targetGridPosition + new Vector2Int(x, y) - itemOrigen))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}