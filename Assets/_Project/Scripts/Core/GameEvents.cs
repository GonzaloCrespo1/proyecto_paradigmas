using System;

// Avisos de la partida (patrón Observer).
// Quien quiera enterarse de algo se suscribe con +=; quien lo provoca llama a un Raise.
// Así los módulos no se conocen entre sí: solo conocen estos avisos.
public static class GameEvents
{
    public static event Action OnTurnChanged;
    public static event Action OnImpact;
    public static event Action OnHealthChanged;
    public static event Action OnGameOver;

    public static void RaiseTurnChanged()
    {
        OnTurnChanged?.Invoke();
    }

    public static void RaiseImpact()
    {
        OnImpact?.Invoke();
    }

    public static void RaiseHealthChanged()
    {
        OnHealthChanged?.Invoke();
    }

    public static void RaiseGameOver()
    {
        OnGameOver?.Invoke();
    }
}
