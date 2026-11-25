using UnityEngine;

// Texto mínimo en pantalla para ver el flujo de la partida.
// Es provisional: se sustituirá por la interfaz real.
public class DebugOverlay : MonoBehaviour
{
    private void OnGUI()
    {
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
            "Ángulo: " + controller.GetCurrentAim().GetAngle().ToString("0") + "°   " +
            "Fuerza: " + controller.GetCurrentAim().GetForce().ToString("0") + "\n\n" +
            "W/S: ángulo   A/D: fuerza   Espacio: disparar   R: reiniciar (al terminar)";
        GUI.Label(new Rect(20, 20, 1100, 220), text, style);
    }
}
