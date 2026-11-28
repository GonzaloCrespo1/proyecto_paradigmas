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
        projectile.GetComponent<Renderer>().material.color = new Color(1f, 0.55f, 0.25f);

        Rigidbody body = projectile.AddComponent<Rigidbody>();
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        body.linearVelocity = velocity;

        projectile.AddComponent<ProjectileImpact>();
        AddTrail(projectile);

        Object.Destroy(projectile, LifeSeconds);
        return projectile;
    }

    // Estela de fuego tras el proyectil: se ve por dónde va y queda más vistoso.
    private static void AddTrail(GameObject projectile)
    {
        TrailRenderer trail = projectile.AddComponent<TrailRenderer>();
        trail.time = 0.7f;
        trail.startWidth = 0.35f;
        trail.endWidth = 0f;
        trail.startColor = new Color(1f, 0.75f, 0.3f, 1f);
        trail.endColor = new Color(1f, 0.3f, 0.5f, 0f);
        trail.material = new Material(Shader.Find("Sprites/Default"));
    }
}
