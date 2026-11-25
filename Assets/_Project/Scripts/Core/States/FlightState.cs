using UnityEngine;

// El proyectil está en el aire; la cámara lo sigue.
// De momento, el vuelo dura un tiempo fijo.
public class FlightState : IGameState
{
    private const float FlightSeconds = 2f;

    private float _startTime;

    public void Enter()
    {
        Debug.Log("Entramos en FlightState");
        _startTime = Time.time;
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
