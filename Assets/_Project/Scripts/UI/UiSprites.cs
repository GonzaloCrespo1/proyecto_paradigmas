using UnityEngine;

// Sprites de interfaz generados por código: un arco semicircular (medidores) y un rectángulo con esquinas redondeadas (paneles).
// Así el HUD no depende de imágenes externas.
public static class UiSprites
{
    private static Sprite _arc;
    private static Sprite _panel;

    // Semicírculo hueco con el centro en el borde inferior; se rellena con Image tipo Filled/Radial180.
    public static Sprite GetArc()
    {
        if (_arc != null)
        {
            return _arc;
        }

        const int Width = 512;
        const int Height = 256;
        const float Outer = 252f;
        const float Inner = 190f;
        Texture2D texture = new Texture2D(Width, Height, TextureFormat.RGBA32, false);
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                float dx = x + 0.5f - Width * 0.5f;
                float dy = y + 0.5f;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                float alpha = Mathf.Clamp01(Outer - distance) * Mathf.Clamp01(distance - Inner);
                texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
        }
        texture.Apply();
        texture.filterMode = FilterMode.Bilinear;
        _arc = Sprite.Create(texture, new Rect(0, 0, Width, Height), new Vector2(0.5f, 0f), 100f);
        return _arc;
    }

    // Rectángulo redondeado de 9 porciones, para fondos de panel.
    public static Sprite GetPanel()
    {
        if (_panel != null)
        {
            return _panel;
        }

        const int Size = 64;
        const float Radius = 22f;
        Texture2D texture = new Texture2D(Size, Size, TextureFormat.RGBA32, false);
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                float cx = Mathf.Clamp(x + 0.5f, Radius, Size - Radius);
                float cy = Mathf.Clamp(y + 0.5f, Radius, Size - Radius);
                float distance = Mathf.Sqrt((x + 0.5f - cx) * (x + 0.5f - cx) + (y + 0.5f - cy) * (y + 0.5f - cy));
                texture.SetPixel(x, y, new Color(1f, 1f, 1f, Mathf.Clamp01(Radius - distance)));
            }
        }
        texture.Apply();
        texture.filterMode = FilterMode.Bilinear;
        _panel = Sprite.Create(texture, new Rect(0, 0, Size, Size), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, new Vector4(24, 24, 24, 24));
        return _panel;
    }
}
