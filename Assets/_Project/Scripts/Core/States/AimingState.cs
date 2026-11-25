using UnityEngine;

// Turno del jugador: apunta el ángulo, mantiene Espacio para cargar la fuerza y lo suelta para disparar.
// Lee el teclado como órdenes (Command) que cambian el apuntado del jugador actual.
public class AimingState : IGameState
{
    private AimInput _input;

    public void Enter()
    {
        Debug.Log("Entramos en AimingState");
        _input = new AimInput();
        _input.Enable();
    }

    public void Update()
    {
        GameController controller = GameController.GetInstance();

        foreach (ICommand command in _input.ReadCommands(controller.GetCurrentAim()))
        {
            command.Execute();
        }

        if (_input.WasFireReleased())
        {
            controller.ChangeState(new FlightState());
        }
    }

    public void Exit()
    {
        _input.Disable();
    }
}
