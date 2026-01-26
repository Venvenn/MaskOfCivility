using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

public static class FillUtility
{
    public static void FloodFillTexture(Texture2D texture, int2 startPoint, Color targetColor, Color replacementColor)
    {
        Stack<int2> pixels = new Stack<int2>();
        pixels.Push(startPoint);
        
        while (pixels.Count != 0)
        {
            int2 temp = pixels.Pop();
            int y1 = temp.y;
            while (y1 >= 0 && texture.GetPixel(temp.x, y1) == targetColor)
            {
                y1--;
            }
            y1++;
            bool spanLeft = false;
            bool spanRight = false;
            while (y1 < texture.height && texture.GetPixel(temp.x, y1) == targetColor)
            {
                texture.SetPixel(temp.x, y1, replacementColor);
 
                if (!spanLeft && temp.x > 0 && texture.GetPixel(temp.x - 1, y1) == targetColor)
                {
                    pixels.Push(new int2(temp.x - 1, y1));
                    spanLeft = true;
                }
                else if(spanLeft && temp.x - 1 == 0 && texture.GetPixel(temp.x - 1, y1) != targetColor)
                {
                    spanLeft = false;
                }
                if (!spanRight && temp.x < texture.width - 1 && texture.GetPixel(temp.x + 1, y1) == targetColor)
                {
                    pixels.Push(new int2(temp.x + 1, y1));
                    spanRight = true;
                }
                else if (spanRight && temp.x < texture.width - 1 && texture.GetPixel(temp.x + 1, y1) != targetColor)
                {
                    spanRight = false;
                } 
                y1++;
            }
 
        }
        
        texture.Apply();
    }
    
    public static async Task<Color[]> Flood(Color[] colours,  int2 point, int2 size, Color oldColor, Color newColor)
    {
        if (point.x >= 0 && point.x < size.x && point.y >= 0 && point.y < size.y)
        {
            //await Task.Yield();
            int id = (point.y * size.x) + point.x;
            if (colours[id] == oldColor)
            {
                colours[id] = newColor;
                colours = await Flood(colours,new int2(point.x + 1, point.y), size, oldColor, newColor);
                colours = await Flood(colours,new int2(point.x - 1, point.y), size, oldColor, newColor);
                colours = await Flood(colours, new int2(point.x, point.y + 1), size, oldColor, newColor);
                colours = await Flood(colours, new int2(point.x, point.y - 1), size, oldColor, newColor);
            }
        }

        return colours;
    }
    
    public static void DrawLine(this Texture2D tex, Vector2 p1, Vector2 p2, Color col)
    {
        Vector2 t = p1;
        float frac = 1/Mathf.Sqrt (Mathf.Pow (p2.x - p1.x, 2) + Mathf.Pow (p2.y - p1.y, 2));
        float ctr = 0;
	
        while ((int)t.x != (int)p2.x || (int)t.y != (int)p2.y) {
            t = Vector2.Lerp(p1, p2, ctr);
            ctr += frac;
            tex.SetPixel((int)t.x, (int)t.y, col);
        }
    }

    public static Vector2 GetCenterPointFromPoints(Vector2[] points)
    {
        float totalX = 0, totalY = 0;
        foreach (Vector2 p in points)
        {
            totalX += p.x;
            totalY += p.x;
        }
        float centerX = totalX / points.Length;
        float centerY = totalY / points.Length;

        return new Vector2(centerX, centerY);
    }
}
