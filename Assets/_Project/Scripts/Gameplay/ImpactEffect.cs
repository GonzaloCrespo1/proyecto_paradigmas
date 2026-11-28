using UnityEngine;

// Explosión de chispas en el punto del impacto. Se crea por código y se destruye sola.
public static class ImpactEffect
{
    public static void Spawn(Vector3 position)
    {
        GameObject effect = new GameObject("Impacto");
        effect.transform.position = position;

        ParticleSystem particles = effect.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.duration = 0.5f;
        main.loop = false;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.6f, 1.1f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(4f, 11f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.3f, 0.7f);
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(2.4f, 1.7f, 0.6f), new Color(2.2f, 0.6f, 1.1f));
        main.gravityModifier = 1.3f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.stopAction = ParticleSystemStopAction.Destroy;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 45) });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.3f;

        ParticleSystem.ColorOverLifetimeModule fade = particles.colorOverLifetime;
        fade.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) });
        fade.color = gradient;

        ParticleSystem.SizeOverLifetimeModule shrink = particles.sizeOverLifetime;
        shrink.enabled = true;
        shrink.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0f));

        effect.GetComponent<ParticleSystemRenderer>().material = SoftDot.GetMaterial();
        particles.Play();
    }

    // Destello breve en la boca del arma al disparar: chispas hacia delante.
    public static void SpawnMuzzle(Vector3 position, Vector3 direction)
    {
        GameObject effect = new GameObject("Destello");
        effect.transform.SetPositionAndRotation(position, Quaternion.LookRotation(direction));

        ParticleSystem particles = effect.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.duration = 0.2f;
        main.loop = false;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.15f, 0.35f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(3f, 8f);
        main.startSize = new ParticleSystem.MinMaxCurve(0.15f, 0.4f);
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(2.6f, 2f, 0.9f), new Color(2.4f, 0.9f, 1.2f));
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.stopAction = ParticleSystemStopAction.Destroy;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 18) });

        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 22f;
        shape.radius = 0.1f;

        ParticleSystem.SizeOverLifetimeModule shrink = particles.sizeOverLifetime;
        shrink.enabled = true;
        shrink.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0f));

        effect.GetComponent<ParticleSystemRenderer>().material = SoftDot.GetMaterial();
        particles.Play();
    }
}
