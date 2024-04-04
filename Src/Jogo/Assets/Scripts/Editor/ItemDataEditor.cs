using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(ItemData))]
    public class ItemDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Label("ItemSize");
            ItemData itemData = (ItemData)target;  

            for (int i = 0; i < itemData.OriginalItemSize.GetLength(0); i++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int y = 0; y < itemData.OriginalItemSize.GetLength(1); y++)
                {
                    itemData.OriginalItemSize[i, y] = EditorGUILayout.Toggle(itemData.OriginalItemSize[i, y]);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}