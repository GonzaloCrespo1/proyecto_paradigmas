using UnityEngine;

// Muestra el halo de luz solo sobre la zona del rival del jugador que tiene el turno.
// Escucha el aviso de cambio de turno (Observer) en lugar de comprobarlo cada fotograma.
public class RivalHalo : MonoBehaviour
{
    [SerializeField] private GameObject[] _halos;

    private void OnEnable()
    {
        GameEvents.OnTurnChanged += Refresh;
    }

    private void OnDisable()
    {
        GameEvents.OnTurnChanged -= Refresh;
    }

    private void Start()
    {
        Refresh();
    }

    private void Refresh()
    {
        GameController controller = GameController.GetInstance();
        if (controller == null)
        {
            return;
        }

        for (int i = 0; i < _halos.Length; i++)
        {
            _halos[i].SetActive(i != controller.GetCurrentPlayerIndex());
        }
    }
}
