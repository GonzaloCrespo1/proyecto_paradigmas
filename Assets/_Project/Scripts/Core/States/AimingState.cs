using UnityEngine;

// Turno del jugador: apunta y decide ángulo y fuerza.
public class AimingState : IGameState
{
    public void Enter()
    {
        Debug.Log("Entramos en AimingState");
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}
