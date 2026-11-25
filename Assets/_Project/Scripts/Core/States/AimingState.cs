using UnityEngine;
using UnityEngine.InputSystem;

// Turno del jugador: apunta y decide ángulo y fuerza.
// De momento, Espacio dispara.
public class AimingState : IGameState
{
    public void Enter()
    {
        Debug.Log("Entramos en AimingState");
    }

    public void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            GameController.GetInstance().ChangeState(new FlightState());
        }
    }

    public void Exit()
    {
    }
}
