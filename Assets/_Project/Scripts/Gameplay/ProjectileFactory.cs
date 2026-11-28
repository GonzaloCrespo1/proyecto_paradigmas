using UnityEngine;

// Fábrica de proyectiles: crea el objeto con su física y lo lanza.
// Quien la usa no necesita saber cómo se construye un proyectil.
public class ProjectileFactory
{
    private const float Radius = 0.4f;
    private const float LifeSeconds = 6f;

    public GameObject Create(Vector3 origin, Vector3 velocity)
    {
        GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        projectile.name = "Proyectil";
        projectile.transform.position = origin;
        projectile.transform.localScale = Vector3.one * Radius;
        projectile.GetComponent<Renderer>().material.color = new Color(0.15f, 0.15f, 0.15f);

        Rigidbody body = projectile.AddComponent<Rigidbody>();
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        body.linearVelocity = velocity;

        projectile.AddComponent<ProjectileImpact>();

        Object.Destroy(projectile, LifeSeconds);
        return projectile;
    }
}
