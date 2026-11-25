using UnityEngine;

// Fin de la partida: hay un ganador.
public class GameOverState : IGameState
{
    public void Enter()
    {
        Debug.Log("Entramos en GameOverState");
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}
