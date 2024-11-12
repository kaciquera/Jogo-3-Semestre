using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    namespace Game
    {
        public static class UIOverlapChecker
        {
            private static readonly Vector3[] rect1Corners = new Vector3[4];
            private static readonly Vector3[] rect2Corners = new Vector3[4];

            public static bool IsOverlappingOtherUIElements(RectTransform rectTransform, Canvas[] canvases, string[] checkedTags)
            {
                IEnumerable<RectTransform> allRects = canvases
                    .SelectMany(canvas => canvas.GetComponentsInChildren<RectTransform>(true))
                    .Where(rect => checkedTags.Contains(rect.tag) && rect != rectTransform);

                foreach (RectTransform otherRect in allRects)
                {
                    if (AreRectTransformsOverlapping(rectTransform, otherRect))
                    {
                        return true;
                    }
                }

                return false;
            }

            private static bool AreRectTransformsOverlapping(RectTransform rect1, RectTransform rect2)
            {
                rect1.GetWorldCorners(rect1Corners);
                rect2.GetWorldCorners(rect2Corners);

                for (int i = 0; i < 4; i++)
                {
                    if (IsPointInsidePolygon(rect1Corners, rect2Corners[i]) || IsPointInsidePolygon(rect2Corners, rect1Corners[i]))
                    {
                        return true;
                    }
                }

                return false;
            }

            private static bool IsPointInsidePolygon(Vector3[] polygonCorners, Vector3 point)
            {
                int intersectCount = 0;
                for (int i = 0; i < polygonCorners.Length; i++)
                {
                    Vector3 corner1 = polygonCorners[i];
                    Vector3 corner2 = polygonCorners[(i + 1) % polygonCorners.Length];

                    if (IsLineIntersectingVerticalRay(point, corner1, corner2))
                    {
                        intersectCount++;
                    }
                }

                return intersectCount % 2 == 1;
            }

            private static bool IsLineIntersectingVerticalRay(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
            {
                if (lineStart.y > lineEnd.y)
                {
                    Vector3 temp = lineStart;
                    lineStart = lineEnd;
                    lineEnd = temp;
                }

                return (point.y > lineStart.y && point.y <= lineEnd.y) &&
                       (point.x < (lineEnd.x - lineStart.x) * (point.y - lineStart.y) / (lineEnd.y - lineStart.y) + lineStart.x);
            }
        }
    }

}