using UnityEngine;

// Estrategia provisional: elige elevación, giro y fuerza al azar dentro de un rango razonable.
public class RandomAimDecision : IAimDecision
{
    public AimDecision Decide()
    {
        float angle = Random.Range(35f, 65f);
        float yaw = Random.Range(-8f, 8f);
        float force = Random.Range(50f, 90f);
        return new AimDecision(angle, yaw, force);
    }
}
