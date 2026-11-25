using UnityEngine;
using UnityEngine.InputSystem;

// Fin de la partida: hay un ganador.
// R reinicia la partida.
public class GameOverState : IGameState
{
    public void Enter()
    {
        Debug.Log("Entramos en GameOverState");
        GameEvents.RaiseGameOver();
    }

    public void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            GameController.GetInstance().RestartMatch();
        }
    }

    public void Exit()
    {
    }
}
