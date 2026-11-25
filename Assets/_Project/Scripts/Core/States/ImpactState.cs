using UnityEngine;

// El proyectil ha impactado: se resuelve el daño.
// De momento, tras una pausa pasa el turno o termina la partida.
public class ImpactState : IGameState
{
    private const float PauseSeconds = 1f;

    private float _startTime;

    public void Enter()
    {
        Debug.Log("Entramos en ImpactState");
        _startTime = Time.time;
        GameEvents.RaiseImpact();
    }

    public void Update()
    {
        if (Time.time - _startTime < PauseSeconds)
        {
            return;
        }

        GameController controller = GameController.GetInstance();
        controller.NextTurn();

        if (controller.IsMatchOver())
        {
            controller.ChangeState(new GameOverState());
        }
        else
        {
            controller.ChangeState(new AimingState());
        }
    }

    public void Exit()
    {
    }
}
