using UnityEngine;

// Estado inicial: la partida todavía no ha empezado.
public class MenuState : IGameState
{
    public void Enter()
    {
        Debug.Log("Entramos en MenuState");
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}
