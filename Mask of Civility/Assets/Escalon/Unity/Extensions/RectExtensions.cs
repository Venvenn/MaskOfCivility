using UnityEngine;

namespace Escalon
{
    public static class RectExtensions
    {
        public static Vector2 RandomPointInside(this Rect rect)
        {
            return new Vector2(
                Random.Range(rect.xMin, rect.xMax),
                Random.Range(rect.yMin, rect.yMax));
        }

        public static Vector2 RandomPointInside(this Rect rect, float margin)
        {
            return new Vector2(
                Random.Range(rect.xMin + margin, rect.xMax - margin),
                Random.Range(rect.yMin + margin, rect.yMax - margin));
        } 
    }
}

