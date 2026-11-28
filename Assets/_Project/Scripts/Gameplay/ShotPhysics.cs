using System.Collections.Generic;
using UnityEngine;

// Cálculos del tiro: dirección, velocidad inicial y trayectoria ideal.
// Es lógica pura, sin escena. El jugador 1 mira hacia +X y el jugador 2 hacia -X;
// el giro horizontal (yaw) se mide desde esa dirección y es positivo hacia la derecha.
public static class ShotPhysics
{
    private const float ForceToSpeed = 0.3f;
    private const float StepSeconds = 0.1f;

    // Dirección horizontal del jugador, ya girada por el yaw.
    public static Vector3 GetDirection(float yaw, int playerIndex)
    {
        Vector3 forward = playerIndex == 0 ? Vector3.right : Vector3.left;
        return Quaternion.AngleAxis(yaw, Vector3.up) * forward;
    }

    public static Vector3 GetVelocity(float angle, float yaw, float force, int playerIndex)
    {
        float radians = angle * Mathf.Deg2Rad;
        float speed = force * ForceToSpeed;
        Vector3 horizontal = GetDirection(yaw, playerIndex) * Mathf.Cos(radians);
        return (horizontal + Vector3.up * Mathf.Sin(radians)) * speed;
    }

    // Puntos de la parábola desde el origen durante los segundos indicados.
    public static List<Vector3> GetPath(Vector3 origin, Vector3 velocity, float seconds)
    {
        List<Vector3> points = new List<Vector3>();
        int count = Mathf.CeilToInt(seconds / StepSeconds);
        for (int i = 0; i <= count; i++)
        {
            float t = i * StepSeconds;
            points.Add(origin + velocity * t + 0.5f * Physics.gravity * t * t);
        }
        return points;
    }
}
