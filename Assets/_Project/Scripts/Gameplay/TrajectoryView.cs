using System.Collections.Generic;
using UnityEngine;

// Dibuja las dos guías cortas del apuntado:
// una estela continua con el tiro que saldría ahora y una punteada con el tiro anterior del jugador.
// Son cortas a propósito: el resto del tiro hay que estimarlo.
public class TrajectoryView : MonoBehaviour
{
    private const float LineWidth = 0.12f;
    private const float GuideSeconds = 1f;

    private LineRenderer _current;
    private LineRenderer _previous;

    private void Awake()
    {
        _current = CreateLine("Estela", new Color(1f, 0.85f, 0.2f), false);
        _previous = CreateLine("Tiro anterior", new Color(1f, 1f, 1f, 0.85f), true);
    }

    private void Update()
    {
        GameController controller = GameController.GetInstance();
        if (controller == null || !controller.IsAiming() || controller.IsBotTurn())
        {
            _current.enabled = false;
            _previous.enabled = false;
            return;
        }

        AimSettings aim = controller.GetCurrentAim();
        int player = controller.GetCurrentPlayerIndex();
        Vector3 origin = controller.GetLaunchPosition();

        Draw(_current, origin, ShotPhysics.GetVelocity(aim.GetAngle(), aim.GetYaw(), aim.GetForce(), player));

        if (aim.HasLastShot())
        {
            Draw(_previous, origin, ShotPhysics.GetVelocity(aim.GetLastAngle(), aim.GetLastYaw(), aim.GetLastForce(), player));
        }
        else
        {
            _previous.enabled = false;
        }
    }

    private void Draw(LineRenderer line, Vector3 origin, Vector3 velocity)
    {
        List<Vector3> path = ShotPhysics.GetPath(origin, velocity, GuideSeconds);
        line.positionCount = path.Count;
        line.SetPositions(path.ToArray());
        line.enabled = true;
    }

    private LineRenderer CreateLine(string lineName, Color color, bool dotted)
    {
        GameObject child = new GameObject(lineName);
        child.transform.SetParent(transform);

        LineRenderer line = child.AddComponent<LineRenderer>();
        line.widthMultiplier = LineWidth;
        line.startColor = color;
        line.endColor = color;
        line.numCapVertices = 4;

        Material material = new Material(Shader.Find("Sprites/Default"));
        if (dotted)
        {
            Texture2D dots = new Texture2D(2, 1, TextureFormat.RGBA32, false);
            dots.SetPixel(0, 0, Color.white);
            dots.SetPixel(1, 0, Color.clear);
            dots.filterMode = FilterMode.Point;
            dots.wrapMode = TextureWrapMode.Repeat;
            dots.Apply();
            material.mainTexture = dots;
            line.textureMode = LineTextureMode.Tile;
            line.textureScale = new Vector2(1f / (2f * LineWidth), 1f);
        }
        line.material = material;
        line.enabled = false;
        return line;
    }
}
