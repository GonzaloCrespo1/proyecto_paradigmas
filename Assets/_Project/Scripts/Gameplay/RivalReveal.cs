using System.Collections;
using UnityEngine;

// Oculta al rival del jugador con turno y lo revela cuando el proyectil impacta.
// Al empezar el siguiente turno vuelve a esconderse. Escucha avisos (Observer), no comprueba nada cada fotograma.
public class RivalReveal : MonoBehaviour
{
    [SerializeField] private Transform[] _players;

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
            SetVisible(_players[i], i == controller.GetCurrentPlayerIndex());
        }
    }

    private void RevealRival()
    {
        GameController controller = GameController.GetInstance();
        for (int i = 0; i < _players.Length; i++)
        {
            SetVisible(_players[i], true);
            if (controller != null && i != controller.GetCurrentPlayerIndex())
            {
                StartCoroutine(Pop(_players[i]));
            }
        }
    }

    // Efecto de aparición: el rival "salta" de pequeño a un poco más grande y se asienta.
    private static IEnumerator Pop(Transform player)
    {
        const float Duration = 0.45f;
        Vector3 normal = Vector3.one;
        for (float t = 0f; t < Duration; t += Time.deltaTime)
        {
            float k = t / Duration;
            float scale = k < 0.6f ? Mathf.Lerp(0.4f, 1.25f, k / 0.6f) : Mathf.Lerp(1.25f, 1f, (k - 0.6f) / 0.4f);
            player.localScale = normal * scale;
            yield return null;
        }
        player.localScale = normal;
    }

    private static void SetVisible(Transform player, bool visible)
    {
        foreach (Renderer part in player.GetComponentsInChildren<Renderer>())
        {
            part.enabled = visible;
        }
    }
}
