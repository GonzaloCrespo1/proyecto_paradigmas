using UnityEngine;

// Controlador de la partida: el único Singleton del proyecto.
// Guarda el estado actual y cambia de estado y de turno.
public class GameController : MonoBehaviour
{
    private static GameController _instance;

    private const int MaxTurns = 4;

    private IGameState _currentState;
    private int _currentPlayerIndex;
    private int _turnsPlayed;

    public static GameController GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        ChangeState(new MenuState());
    }

    private void Update()
    {
        _currentState?.Update();
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    public void ChangeState(IGameState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void NextTurn()
    {
        _currentPlayerIndex = 1 - _currentPlayerIndex;
        _turnsPlayed++;
        GameEvents.RaiseTurnChanged();
    }

    public bool IsMatchOver()
    {
        return _turnsPlayed >= MaxTurns;
    }

    public void RestartMatch()
    {
        _currentPlayerIndex = 0;
        _turnsPlayed = 0;
        ChangeState(new MenuState());
    }

    public string GetCurrentStateName()
    {
        return _currentState == null ? "-" : _currentState.GetType().Name;
    }

    public int GetCurrentPlayerIndex()
    {
        return _currentPlayerIndex;
    }

    public int GetTurnsPlayed()
    {
        return _turnsPlayed;
    }
}
