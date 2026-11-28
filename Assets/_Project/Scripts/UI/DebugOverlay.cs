using UnityEngine;
using UnityEngine.InputSystem;

// Texto mínimo en pantalla para ver el flujo de la partida.
// Es provisional: se sustituirá por la interfaz real.
// Se muestra y oculta con F3; el HUD real es AimHud.
public class DebugOverlay : MonoBehaviour
{
    private bool _visible;

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.f3Key.wasPressedThisFrame)
        {
            _visible = !_visible;
        }
    }

    private void OnGUI()
    {
        if (!_visible)
        {
            return;
        }

        GameController controller = GameController.GetInstance();
        if (controller == null)
        {
            return;
        }

        GUIStyle style = new GUIStyle(GUI.skin.label) { fontSize = 22 };
        string text =
            "Estado: " + controller.GetCurrentStateName() + "\n" +
            "Jugador: " + (controller.GetCurrentPlayerIndex() + 1) + "\n" +
            "Turnos jugados: " + controller.GetTurnsPlayed() + "\n" +
            "Elevación: " + controller.GetCurrentAim().GetAngle().ToString("0") + "°   " +
            "Giro: " + controller.GetCurrentAim().GetYaw().ToString("0") + "°   " +
            "Fuerza: " + controller.GetCurrentAim().GetForce().ToString("0") + "\n\n" +
            "W/S: elevación   A/D: giro   Mantener Espacio: fuerza\nSoltar Espacio: disparar   R: reiniciar (al terminar)";
        GUI.Label(new Rect(20, 20, 1100, 220), text, style);
    }
}
