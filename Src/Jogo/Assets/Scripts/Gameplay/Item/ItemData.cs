using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Item_", menuName = "Itens/Item Data")]
    public partial class ItemData : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite itemSprite;
        [SerializeField] private int girdSizeInPixels = 128;
        [SerializeField] private AudioClip[] pickSound;
        [SerializeField, HideInInspector] private List<ElementSlot<bool>> serializable;
        [SerializeField, HideInInspector] private Vector2Int editorGridSize = new Vector2Int(5, 5);

        public string ItemName => itemName;
        public int GirdSizeInPixels => girdSizeInPixels;
        public bool[,] OriginalItemSize { get; set; }
        public Vector2Int GridSize => new Vector2Int(OriginalItemSize.GetLength(0), OriginalItemSize.GetLength(1));
        public Sprite ItemSprite => itemSprite;

        public void PlayPickSound()
        {
            int index = Random.Range(0, pickSound.Length);
            //AudioManager.Instance.PlaySound(pickSound[index]);
        }
        public void OnBeforeSerialize()
        {
            serializable = new List<ElementSlot<bool>>();
            for (int i = 0; i < editorGridSize.x; i++)
            {
                for (int j = 0; j < editorGridSize.y; j++)
                {
                    if (i < OriginalItemSize.GetLength(0) && j < OriginalItemSize.GetLength(1))
                    {
                        serializable.Add(new ElementSlot<bool>(i, j, OriginalItemSize[i, j]));
                    }
                }
            }
        }

        public void OnAfterDeserialize()
        {
            OriginalItemSize = new bool[editorGridSize.x, editorGridSize.y];
            foreach (var package in serializable)
            {
                if (package.index0 >= 0 && package.index0 < editorGridSize.x && package.index1 >= 0 && package.index1 < editorGridSize.y)
                {
                    OriginalItemSize[package.index0, package.index1] = package.element;
                }
                else
                {
                    continue;
                }
            }
        }
    }
}