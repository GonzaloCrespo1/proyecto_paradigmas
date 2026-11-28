using UnityEngine;

// Oculta al rival del jugador con turno y lo revela cuando el proyectil impacta.
// Al empezar el siguiente turno vuelve a esconderse. Escucha avisos (Observer), no comprueba nada cada fotograma.
public class RivalReveal : MonoBehaviour
{
    [SerializeField] private Renderer[] _players;

    private void OnEnable()
    {
        GameEvents.OnTurnChanged += HideRival;
        GameEvents.OnImpact += RevealRival;
    }

    private void OnDisable()
    {
        GameEvents.OnTurnChanged -= HideRival;
        GameEvents.OnImpact -= RevealRival;
    }

    private void Start()
    {
        HideRival();
    }

    private void HideRival()
    {
        GameController controller = GameController.GetInstance();
        if (controller == null)
        {
            return;
        }

        for (int i = 0; i < _players.Length; i++)
        {
            _players[i].enabled = i == controller.GetCurrentPlayerIndex();
        }
    }

    private void RevealRival()
    {
        foreach (Renderer player in _players)
        {
            player.enabled = true;
        }
    }
}
