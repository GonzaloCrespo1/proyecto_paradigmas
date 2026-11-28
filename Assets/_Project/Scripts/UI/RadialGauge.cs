using UnityEngine;
using UnityEngine.UI;

// Medidor semicircular del HUD: una pista tenue, un arco que se rellena, el valor grande en el centro y una etiqueta debajo.
public class RadialGauge
{
    private readonly Image _fill;
    private readonly Text _value;

    public RadialGauge(Transform parent, string label, Vector2 position, float width, Font font)
    {
        GameObject root = new GameObject("Medidor " + label, typeof(RectTransform));
        root.transform.SetParent(parent, false);
        RectTransform rect = root.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);
        rect.sizeDelta = new Vector2(width, width * 0.5f);
        rect.anchoredPosition = position;

        AddArc(root.transform, "Pista", new Color(1f, 1f, 1f, 0.16f));
        _fill = AddArc(root.transform, "Relleno", Color.white);
        _fill.type = Image.Type.Filled;
        _fill.fillMethod = Image.FillMethod.Radial180;
        _fill.fillOrigin = (int)Image.Origin180.Bottom;
        _fill.fillClockwise = true;
        _fill.fillAmount = 0f;

        _value = AddText(root.transform, "Valor", font, 46, new Vector2(0.5f, 0f), new Vector2(0f, width * 0.04f));
        Text caption = AddText(root.transform, "Etiqueta", font, 24, new Vector2(0.5f, 0f), new Vector2(0f, -width * 0.1f));
        caption.text = label;
        caption.color = new Color(1f, 1f, 1f, 0.85f);
    }

    public void Set(float amount, string text, Color color)
    {
        _fill.fillAmount = Mathf.Clamp01(amount);
        _fill.color = color;
        _value.text = text;
    }

    private static Image AddArc(Transform parent, string objectName, Color color)
    {
        GameObject go = new GameObject(objectName, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        Image image = go.GetComponent<Image>();
        image.sprite = UiSprites.GetArc();
        image.color = color;
        image.raycastTarget = false;
        return image;
    }

    private static Text AddText(Transform parent, string objectName, Font font, int size, Vector2 anchor, Vector2 position)
    {
        GameObject go = new GameObject(objectName, typeof(RectTransform), typeof(Text), typeof(Shadow));
        go.transform.SetParent(parent, false);
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = anchor;
        rect.pivot = new Vector2(0.5f, 0f);
        rect.sizeDelta = new Vector2(320f, size * 1.4f);
        rect.anchoredPosition = position;
        Text text = go.GetComponent<Text>();
        text.font = font;
        text.fontSize = size;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.raycastTarget = false;
        go.GetComponent<Shadow>().effectDistance = new Vector2(2f, -2f);
        return text;
    }
}
