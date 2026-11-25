using UnityEngine;

// El proyectil está en el aire; la cámara lo sigue.
public class FlightState : IGameState
{
    public void Enter()
    {
        Debug.Log("Entramos en FlightState");
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}
