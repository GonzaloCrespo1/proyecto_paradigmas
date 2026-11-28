using UnityEngine;
using UnityEngine.UI;

// Interfaz de la partida: medidores semicirculares de ÁNGULO y FUERZA mientras apuntas,
// un cartel central con el momento de la partida y el contador de turnos.
// Se construye entera por código y solo lee el estado de la partida.
public class AimHud : MonoBehaviour
{
    private const float FadeSpeed = 8f;
    private static readonly Color AngleColor = new Color(0.55f, 0.88f, 1f);
    private static readonly Color ForceLow = new Color(1f, 0.85f, 0.35f);
    private static readonly Color ForceHigh = new Color(1f, 0.32f, 0.45f);

    private CanvasGroup _gauges;
    private RadialGauge _angle;
    private RadialGauge _force;
    private Text _yaw;
    private Text _banner;
    private Text _turn;
    private float _bannerAlpha;

    // Se crea solo al iniciar la escena, sin tener que colocarlo a mano.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreateOnLoad()
    {
        if (FindAnyObjectByType<AimHud>() == null)
        {
            new GameObject("AimHud", typeof(AimHud));
        }
    }

    private void Awake()
    {
        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        Canvas canvas = new GameObject("HUD", typeof(Canvas), typeof(CanvasScaler)).GetComponent<Canvas>();
        canvas.transform.SetParent(transform, false);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        BuildGauges(canvas.transform, font);
        _banner = BuildText(canvas.transform, "Cartel", font, 72, new Vector2(0.5f, 1f), new Vector2(0f, -70f), new Vector2(1400f, 110f), TextAnchor.MiddleCenter);
        _turn = BuildText(canvas.transform, "Turno", font, 34, new Vector2(0f, 1f), new Vector2(40f, -30f), new Vector2(420f, 50f), TextAnchor.MiddleLeft);
    }

    private void BuildGauges(Transform canvas, Font font)
    {
        GameObject group = new GameObject("Medidores", typeof(RectTransform), typeof(CanvasGroup));
        group.transform.SetParent(canvas, false);
        RectTransform rect = group.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);
        rect.sizeDelta = new Vector2(1900f, 235f);
        rect.anchoredPosition = new Vector2(0f, 14f);
        _gauges = group.GetComponent<CanvasGroup>();
        _gauges.alpha = 0f;

        AddPanel(group.transform, new Vector2(-560f, 0f));
        AddPanel(group.transform, new Vector2(560f, 0f));

        _angle = new RadialGauge(group.transform, "ÁNGULO", new Vector2(-560f, 62f), 300f, font);
        _force = new RadialGauge(group.transform, "FUERZA", new Vector2(560f, 62f), 300f, font);
        _yaw = BuildText(group.transform, "Giro", font, 22, new Vector2(0.5f, 0f), new Vector2(0f, 4f), new Vector2(1000f, 32f), TextAnchor.MiddleCenter);
        _yaw.color = new Color(1f, 1f, 1f, 0.8f);
    }

    private static void AddPanel(Transform parent, Vector2 position)
    {
        GameObject panel = new GameObject("Panel", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(parent, false);
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);
        rect.sizeDelta = new Vector2(380f, 225f);
        rect.anchoredPosition = position;
        Image background = panel.GetComponent<Image>();
        background.sprite = UiSprites.GetPanel();
        background.type = Image.Type.Sliced;
        background.color = new Color(0.12f, 0.06f, 0.2f, 0.5f);
        background.raycastTarget = false;
    }

    private static Text BuildText(Transform parent, string objectName, Font font, int size, Vector2 anchor, Vector2 position, Vector2 area, TextAnchor alignment)
    {
        GameObject go = new GameObject(objectName, typeof(RectTransform), typeof(Text), typeof(Shadow));
        go.transform.SetParent(parent, false);
        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = anchor;
        rect.pivot = anchor;
        rect.sizeDelta = area;
        rect.anchoredPosition = position;
        Text text = go.GetComponent<Text>();
        text.font = font;
        text.fontSize = size;
        text.fontStyle = FontStyle.Bold;
        text.alignment = alignment;
        text.color = Color.white;
        text.raycastTarget = false;
        go.GetComponent<Shadow>().effectDistance = new Vector2(3f, -3f);
        return text;
    }

    private void Update()
    {
        GameController controller = GameController.GetInstance();
        if (controller == null)
        {
            return;
        }

        bool showGauges = controller.IsAiming() && !controller.IsBotTurn();
        _gauges.alpha = Mathf.MoveTowards(_gauges.alpha, showGauges ? 1f : 0f, FadeSpeed * Time.deltaTime);
        if (showGauges)
        {
            UpdateGauges(controller.GetCurrentAim());
        }

        string banner = GetBanner(controller);
        if (banner.Length > 0)
        {
            _banner.text = banner;
        }
        _bannerAlpha = Mathf.MoveTowards(_bannerAlpha, banner.Length > 0 ? 1f : 0f, FadeSpeed * Time.deltaTime);
        _banner.color = new Color(1f, 1f, 1f, _bannerAlpha);

        _turn.text = "TURNO " + Mathf.Min(controller.GetTurnsPlayed() + 1, controller.GetMaxTurns()) + " / " + controller.GetMaxTurns();
    }

    private void UpdateGauges(AimSettings aim)
    {
        float angleAmount = (aim.GetAngle() - AimSettings.MinAngle) / (AimSettings.MaxAngle - AimSettings.MinAngle);
        _angle.Set(angleAmount, aim.GetAngle().ToString("0") + "°", AngleColor);

        float forceAmount = (aim.GetForce() - AimSettings.MinForce) / (AimSettings.MaxForce - AimSettings.MinForce);
        _force.Set(forceAmount, Mathf.RoundToInt(forceAmount * 100f) + "%", Color.Lerp(ForceLow, ForceHigh, forceAmount));

        _yaw.text = "GIRO " + aim.GetYaw().ToString("+0;-0;0") + "°   ·   W/S elevación · A/D giro · ESPACIO fuerza · ENTER dispara";
    }

    private static string GetBanner(GameController controller)
    {
        if (controller.IsMenu())
        {
            return "PULSA ESPACIO PARA EMPEZAR";
        }
        if (controller.IsAiming())
        {
            return controller.IsBotTurn() ? "TURNO DEL BOT" : "TU TURNO";
        }
        if (controller.IsImpact())
        {
            return "¡IMPACTO!";
        }
        if (controller.IsGameOver())
        {
            return "FIN DE LA PARTIDA · R PARA REINICIAR";
        }
        return "";
    }
}
