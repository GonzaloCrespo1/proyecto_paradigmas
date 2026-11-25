using UnityEngine;
using UnityEngine.InputSystem;

// Estado inicial: la partida todavía no ha empezado.
// Espacio empieza la partida.
public class MenuState : IGameState
{
    public void Enter()
    {
        Debug.Log("Entramos en MenuState");
    }

    public void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            GameController.GetInstance().ChangeState(new AimingState());
        }
    }

    public void Exit()
    {
    }
}
