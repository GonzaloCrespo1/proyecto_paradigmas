using UnityEngine;

// Controlador de la partida: el único Singleton del proyecto.
// Guarda el estado actual y cambia de estado y de turno.
public class GameController : MonoBehaviour
{
    private static GameController _instance;

    [SerializeField] private Transform[] _players;
    private Rigidbody _projectile;

    private const int MaxTurns = 4;
    private const float LaunchHeight = 1.3f;
    private const int BotPlayerIndex = 1;

    private IGameState _currentState;
    private int _currentPlayerIndex;
    private int _turnsPlayed;
    private readonly AimSettings[] _aims = { new AimSettings(), new AimSettings() };

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
        foreach (AimSettings aim in _aims)
        {
            aim.Reset();
        }
        GameEvents.RaiseTurnChanged();
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

    public AimSettings GetCurrentAim()
    {
        return _aims[_currentPlayerIndex];
    }

    public Transform GetCurrentPlayer()
    {
        return _players[_currentPlayerIndex];
    }

    public Vector3 GetLaunchPosition()
    {
        return _players[_currentPlayerIndex].position + Vector3.up * LaunchHeight;
    }

    // El proyectil que está en el aire (null si no hay ninguno). Lo usa la cámara para seguirlo.
    public void SetProjectile(Rigidbody projectile)
    {
        _projectile = projectile;
    }

    public Rigidbody GetProjectile()
    {
        return _projectile;
    }

    // De momento el jugador 2 es siempre el bot.
    public bool IsBotTurn()
    {
        return _currentPlayerIndex == BotPlayerIndex;
    }

    public bool IsMenu()
    {
        return _currentState is MenuState;
    }

    public bool IsGameOver()
    {
        return _currentState is GameOverState;
    }

    public int GetMaxTurns()
    {
        return MaxTurns;
    }

    public bool IsImpact()
    {
        return _currentState is ImpactState;
    }

    // Posición del rival del jugador con turno (la usa la cámara para el plano de revelado).
    public Vector3 GetRivalPosition()
    {
        return _players[1 - _currentPlayerIndex].position;
    }

    public bool IsAiming()
    {
        return _currentState is AimingState;
    }

    public int GetTurnsPlayed()
    {
        return _turnsPlayed;
    }
}
