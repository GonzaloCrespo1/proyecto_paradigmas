using UnityEngine;

// Fábrica de proyectiles: crea el objeto con su física y lo lanza.
// Quien la usa no necesita saber cómo se construye un proyectil.
public class ProjectileFactory
{
    private const float Radius = 0.55f;
    private const float LifeSeconds = 6f;

    public GameObject Create(Vector3 origin, Vector3 velocity, Transform shooter)
    {
        GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        projectile.name = "Proyectil";
        projectile.transform.position = origin;
        projectile.transform.localScale = Vector3.one * Radius;
        projectile.GetComponent<Renderer>().material.color = new Color(2.4f, 1.1f, 0.35f);

        Rigidbody body = projectile.AddComponent<Rigidbody>();
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        body.linearVelocity = velocity;

        IgnoreShooter(projectile, shooter);
        projectile.AddComponent<ProjectileImpact>();
        AddTrail(projectile);

        Object.Destroy(projectile, LifeSeconds);
        return projectile;
    }

    // El proyectil no debe chocar con quien lo dispara: si no, explotaría nada más salir.
    private static void IgnoreShooter(GameObject projectile, Transform shooter)
    {
        Collider own = projectile.GetComponent<Collider>();
        foreach (Collider other in shooter.GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(own, other);
        }
    }

    // Estela de fuego tras el proyectil: se ve por dónde va y queda más vistoso.
    private static void AddTrail(GameObject projectile)
    {
        TrailRenderer trail = projectile.AddComponent<TrailRenderer>();
        trail.time = 0.9f;
        trail.startWidth = 0.3f;
        trail.endWidth = 0f;
        trail.startColor = new Color(1f, 0.75f, 0.3f, 1f);
        trail.endColor = new Color(1f, 0.3f, 0.5f, 0f);
        trail.material = new Material(Shader.Find("Sprites/Default"));
    }
}
