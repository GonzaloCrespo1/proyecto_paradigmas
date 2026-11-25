using System.Collections.Generic;
using UnityEngine;

// Cálculos del tiro: velocidad inicial y trayectoria ideal.
// Es lógica pura, sin escena: el jugador 1 dispara hacia la derecha y el 2 hacia la izquierda.
public static class ShotPhysics
{
    private const float ForceToSpeed = 0.3f;
    private const float StepSeconds = 0.1f;
    private const int MaxPoints = 80;
    private const float LowestHeight = -1f;

    public static Vector3 GetVelocity(float angle, float force, int playerIndex)
    {
        float direction = playerIndex == 0 ? 1f : -1f;
        float radians = angle * Mathf.Deg2Rad;
        float speed = force * ForceToSpeed;
        return new Vector3(direction * Mathf.Cos(radians) * speed, Mathf.Sin(radians) * speed, 0f);
    }

    // Puntos de la parábola desde el origen, hasta que cae por debajo del suelo.
    public static List<Vector3> GetPath(Vector3 origin, Vector3 velocity)
    {
        List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < MaxPoints; i++)
        {
            float t = i * StepSeconds;
            Vector3 point = origin + velocity * t + 0.5f * Physics.gravity * t * t;
            if (point.y < LowestHeight)
            {
                break;
            }
            points.Add(point);
        }
        return points;
    }
}
