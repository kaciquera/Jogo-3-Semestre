using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public partial class GridInventory : MonoBehaviour
    {
        [Header("Config")]
        [Min(1)]
        [SerializeField] private int width = 1;
        [Min(1)]
        [SerializeField] private int height = 1;
        [Range(16, 512)]
        [SerializeField] private float cellSize = 64;
        [SerializeField] private Vector3 offset;

        [Header("References")]
        [SerializeField] private ItemSlot slotPrefab;

        private readonly Dictionary<Vector2Int, ItemSlot> slotByGrid = new Dictionary<Vector2Int, ItemSlot>();
        private RectTransform rectTransform;

        public static event Action OnVictory;

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

        public void CheckVictoryCondition()
        {
            foreach(ItemSlot slot in slotByGrid.Values)
            {
                if (!slot.IsOccupied) return;
            }
            Debug.Log("Victory");
            OnVictory?.Invoke();
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * (cellSize * rectTransform.localScale.x) + transform.position + offset;
        }

        public ItemSlot GetSlot(Vector2Int gridPosition)
        {
            if (!slotByGrid.ContainsKey(gridPosition)) return null;
            return slotByGrid[gridPosition];
        }

        public bool ValidateSlot(Vector2Int gridPosition)
        {
            return slotByGrid.ContainsKey(gridPosition) && !slotByGrid[gridPosition].IsOccupied;
        }

        public bool ValidateSlots(Vector2Int targetGridPosition, Item item)
        {
            for (int x = 0; x < item.ItemSize.GetLength(0); x++)
            {
                for (int y = 0; y < item.ItemSize.GetLength(1); y++)
                {
                    if (item.ItemSize[x, y] && !ValidateSlot(targetGridPosition + new Vector2Int(x, y) - item.SelectedGrid))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    public partial class GridInventory : MonoBehaviour
    {
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            SlotUtils.DrawGridGizmos(transform, width, height, cellSize, offset);
        }
#endif
    }
}