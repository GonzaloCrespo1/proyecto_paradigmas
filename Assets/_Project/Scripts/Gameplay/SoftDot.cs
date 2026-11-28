using UnityEngine;

// Material de partícula con un punto redondo y suave generado por código, para que las chispas no sean cuadrados.
public static class SoftDot
{
    private static Material _material;

    public static Material GetMaterial()
    {
        if (_material != null)
        {
            return _material;
        }

        const int Size = 32;
        Texture2D texture = new Texture2D(Size, Size, TextureFormat.RGBA32, false);
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                float distance = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(Size * 0.5f, Size * 0.5f)) / (Size * 0.5f);
                float alpha = Mathf.Clamp01(1f - distance);
                texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha * alpha));
            }
        }
        texture.Apply();
        _material = new Material(Shader.Find("Sprites/Default")) { mainTexture = texture };
        return _material;
    }
}
