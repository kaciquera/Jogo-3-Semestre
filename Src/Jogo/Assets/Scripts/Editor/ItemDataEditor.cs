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

            SerializedProperty itemGridSizeProp = serializedObject.FindProperty("editorGridSize");
            ItemData itemData = (ItemData)target;
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel);
            titleStyle.fontSize = 22;
            EditorGUILayout.LabelField("ItemSize", titleStyle, GUILayout.Height(24));
            EditorGUI.BeginChangeCheck();
            DrawItemSize(itemData, itemGridSizeProp);
            serializedObject.ApplyModifiedProperties();
            DrawToggles(itemGridSizeProp, itemData);
            serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }

        private void DrawItemSize(ItemData itemData, SerializedProperty itemGridSizeProp)
        {
            Vector2Int gridSize = new Vector2Int(Mathf.Clamp(itemGridSizeProp.vector2IntValue.x, 1, 10), Mathf.Clamp(itemGridSizeProp.vector2IntValue.y, 1, 10));
            Vector2Int newValue = EditorGUILayout.Vector2IntField("Grid Size:", gridSize);

            if (newValue != gridSize)
            {
                itemGridSizeProp.vector2IntValue = new Vector2Int(Mathf.Clamp(newValue.x, 1, 10), Mathf.Clamp(newValue.y, 1, 10));
                UpdateItemSizeArray(itemData, newValue);
            }
        }

        private void UpdateItemSizeArray(ItemData itemData, Vector2Int newSize)
        {
            bool[,] newOriginalItemSize = new bool[newSize.x, newSize.y];
            for (int i = 0; i < newSize.x; i++)
            {
                for (int j = 0; j < newSize.y; j++)
                {
                    if (i < itemData.OriginalItemSize.GetLength(0) && j < itemData.OriginalItemSize.GetLength(1))
                    {
                        newOriginalItemSize[i, j] = itemData.OriginalItemSize[i, j];
                    }
                    else
                    {
                        newOriginalItemSize[i, j] = false;
                    }
                }
            }
            itemData.OriginalItemSize = newOriginalItemSize;
        }

        private void DrawToggles(SerializedProperty itemGridSizeProp, ItemData itemData)
        {
            int horizontalSize = itemGridSizeProp.vector2IntValue.x;
            int verticalSize = itemGridSizeProp.vector2IntValue.y;

            for (int y = 0; y < verticalSize; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < horizontalSize; x++)
                {
                    if (x < itemData.OriginalItemSize.GetLength(0) && y < itemData.OriginalItemSize.GetLength(1))
                    {
                        int newX = verticalSize - y - 1;
                        itemData.OriginalItemSize[x, newX] = EditorGUILayout.Toggle(GUIContent.none, itemData.OriginalItemSize[x, newX], GUILayout.Width(16), GUILayout.Height(16));
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}