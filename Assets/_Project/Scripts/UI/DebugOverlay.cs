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
            "Turnos jugados: " + controller.GetTurnsPlayed() + "\n\n" +
            "Espacio: avanzar   R: reiniciar (al terminar)";
        GUI.Label(new Rect(20, 20, 700, 200), text, style);
    }
}
