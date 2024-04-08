using UnityEngine;

namespace Game
{
    public static class SlotUtils
    {
        public static void DrawGridGizmos(Transform originObject, int width, int height, float cellSize, Vector3 offset)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    float size = cellSize * originObject.root.transform.localScale.x;
                    Vector3 cellPosition = originObject.position + new Vector3(i * size, j * size, 0) + offset;
                    Gizmos.DrawWireCube(cellPosition, new Vector3(size, size, 0));
                }
            }
        }

        public static void DrawGridGizmos(RectTransform originObject, int width, int height, float cellSize, bool[,] usedGrid, Color usedColor, Vector3 offset)
        {
            float size = cellSize;
            Quaternion rotation = originObject.rotation;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Vector3 localCellPosition = new Vector3(i * size, j * size, 0) + offset;
                    Vector3 worldCellPosition = originObject.TransformPoint(localCellPosition);
                    Gizmos.matrix = Matrix4x4.TRS(worldCellPosition, rotation, Vector3.one * originObject.root.localScale.x);
                    Gizmos.color = Color.white;
                    Gizmos.DrawWireCube(Vector3.zero, new Vector3(size, size, 0));

                    if (usedGrid[i, j])
                    {
                        Gizmos.color = usedColor;
                        Gizmos.DrawCube(Vector3.zero, new Vector3(size, size, 0));
                    }
                }
            }

            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}