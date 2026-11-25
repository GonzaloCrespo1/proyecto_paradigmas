using UnityEngine;

// El proyectil está en el aire.
// Al entrar se lanza el proyectil con el apuntado del jugador actual.
// De momento, el vuelo dura un tiempo fijo.
public class FlightState : IGameState
{
    private const float FlightSeconds = 3f;

    private float _startTime;

    public void Enter()
    {
        Debug.Log("Entramos en FlightState");
        _startTime = Time.time;

        GameController controller = GameController.GetInstance();
        AimSettings aim = controller.GetCurrentAim();
        Vector3 velocity = ShotPhysics.GetVelocity(aim.GetAngle(), aim.GetYaw(), aim.GetForce(), controller.GetCurrentPlayerIndex());
        new ProjectileFactory().Create(controller.GetLaunchPosition(), velocity);
        aim.RememberShot();
    }

    public void Update()
    {
        if (Time.time - _startTime >= FlightSeconds)
        {
            GameController.GetInstance().ChangeState(new ImpactState());
        }
    }

    public void Exit()
    {
    }
}
