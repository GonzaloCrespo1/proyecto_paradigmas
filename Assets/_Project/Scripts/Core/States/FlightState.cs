using UnityEngine;

// El proyectil está en el aire.
// Al entrar se lanza el proyectil con el apuntado del jugador actual.
// El vuelo termina cuando el proyectil choca con algo (aviso OnProjectileLanded);
// si no choca con nada, un tiempo máximo evita que la partida se quede colgada.
public class FlightState : IGameState
{
    private const float MaxFlightSeconds = 6f;

    private float _startTime;

    public void Enter()
    {
        Debug.Log("Entramos en FlightState");
        _startTime = Time.time;
        GameEvents.OnProjectileLanded += EndFlight;

        GameController controller = GameController.GetInstance();
        AimSettings aim = controller.GetCurrentAim();
        Vector3 velocity = ShotPhysics.GetVelocity(aim.GetAngle(), aim.GetYaw(), aim.GetForce(), controller.GetCurrentPlayerIndex());
        GameObject projectile = new ProjectileFactory().Create(controller.GetLaunchPosition(), velocity, controller.GetCurrentPlayer());
        controller.SetProjectile(projectile.GetComponent<Rigidbody>());
        aim.RememberShot();
        ImpactEffect.SpawnMuzzle(controller.GetLaunchPosition(), velocity.normalized);
        GameEvents.RaiseShotFired();
    }

    public void Update()
    {
        if (Time.time - _startTime >= MaxFlightSeconds)
        {
            EndFlight();
        }
    }

    private void EndFlight()
    {
        GameController.GetInstance().ChangeState(new ImpactState());
    }

    public void Exit()
    {
        GameEvents.OnProjectileLanded -= EndFlight;
    }
}
