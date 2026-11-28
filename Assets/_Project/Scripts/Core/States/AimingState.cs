using UnityEngine;

// Turno de apuntar: la fuente de órdenes es el teclado del jugador o el bot, según de quién sea el turno.
// Las dos producen órdenes (Command) que cambian el apuntado del jugador actual.
public class AimingState : IGameState
{
    private IAimSource _input;

    public void Enter()
    {
        Debug.Log("Entramos en AimingState");
        GameController controller = GameController.GetInstance();
        if (controller.IsBotTurn())
        {
            _input = new BotInput(new RandomAimDecision());
        }
        else
        {
            _input = new AimInput();
        }
        _input.Enable();
    }

    public void Update()
    {
        GameController controller = GameController.GetInstance();

        foreach (ICommand command in _input.ReadCommands(controller.GetCurrentAim()))
        {
            command.Execute();
        }

        if (_input.WasFireRequested())
        {
            controller.ChangeState(new FlightState());
        }
    }

    public void Exit()
    {
        _input.Disable();
    }
}
