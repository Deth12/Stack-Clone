using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public static class Painter
{
    private static List<Color> colors = new List<Color>();
    private static int counter = 0;

    private static Color prevColor;
    private static Color currentColor;
    private static Color nextColor;

    public static void InitializePainter(List<Color> colors)
    {
        Painter.colors = colors;
        colors.Shuffle();
        prevColor = colors[counter];
        nextColor = colors[counter + 1];
    }

    private static float colorProgress = 0;

    public static Color GetNextColor(bool increaseCounter)
    {
        colorProgress += increaseCounter ? 0.125f : 0;
        currentColor = Color.Lerp(prevColor, nextColor, colorProgress);
        if (colorProgress >= 1)
        {
            if(counter + 1 >= colors.Count - 1)
                ReshuffleColors(currentColor);
            else
                ColorIteration(currentColor);
        }
        return currentColor;
    }

    public static void ColorIteration(Color currentColor)
    {
        counter++;
        colorProgress = 0;
        prevColor = currentColor;
        nextColor = colors[counter];
    }

    public static void ReshuffleColors(Color currentColor)
    {
        colors.Shuffle();
        counter = 0;
        colorProgress = 0;
        prevColor = currentColor;
        nextColor = colors[counter];
    }

    public static void PaintObject(GameObject obj, bool increaseCounter)
    {
        Mesh mesh = obj.GetComponent< MeshFilter >().mesh;
        Color32[] newColors = new Color32[mesh.vertices.Length];
        Color32 newColor = GetNextColor(increaseCounter);
        for (int vertexIndex = 0; vertexIndex < newColors.Length; vertexIndex++)
        {
            newColors[vertexIndex] = newColor;
        }
        mesh.colors32 = newColors;
        //obj.GetComponent<MeshRenderer>().material.color = GetNextColor(increaseCounter);
    }

    public static void PaintBackground(Image img)
    {
        LeanTween
            .value(img.gameObject, img.material.GetColor("_ColorMid"), currentColor, 0.3f)
            .setOnUpdate((Color c) =>
            {
                img.material.SetColor("_ColorMid", c);
            });
    }
}

public static class ThreadSafeRandom
{
    [ThreadStatic] private static System.Random Local;

    public static System.Random ThisThreadsRandom
    {
        get { return Local ?? (Local = new System.Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
    }
}

static class Extensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}