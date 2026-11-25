using UnityEngine;

// El proyectil ha impactado: se resuelve el daño.
public class ImpactState : IGameState
{
    public void Enter()
    {
        Debug.Log("Entramos en ImpactState");
    }

    public void Update()
    {
    }

    public void Exit()
    {
    }
}
