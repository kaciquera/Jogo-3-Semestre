using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Item_", menuName = "Itens/Item Data")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite itemSprite;
        [SerializeField]
        private bool[,] originalItemSize = new bool[5, 5];

        public string ItemName => itemName; 
        public Sprite ItemSprite  => itemSprite;
        public bool[,] OriginalItemSize => originalItemSize;
    }
}