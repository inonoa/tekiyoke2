
using UnityEngine;
using UnityEngine.UI;

public static class ColorExtensions
{
    public static void SetAlpha(this Image image, float a)
    {
        Color color = image.color;
        color.a = a;
        image.color = color;
    }
    public static void SetAlpha(this SpriteRenderer spriteRenderer, float a)
    {
        Color color = spriteRenderer.color;
        color.a = a;
        spriteRenderer.color = color;
    }
}