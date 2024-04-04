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
            SerializedProperty itemSize = serializedObject.FindProperty("originalItemSize");
        }
    }
}