using System.Collections.Generic;
using UnityEngine;

// Reparte pinos y rocas low-poly por el escenario con una semilla fija.
// Deja libres las colinas y el pasillo por donde vuelan los proyectiles.
public class Scenery : MonoBehaviour
{
    private const float AreaX = 70f;
    private const float AreaZ = 45f;
    private const float CorridorHalfWidth = 5f;
    private const float CorridorHalfLength = 32f;

    [SerializeField] private int _seed = 7;
    [SerializeField] private int _pines = 80;
    [SerializeField] private int _rocas = 30;
    [SerializeField] private Material[] _pineMaterials;
    [SerializeField] private Material _trunkMaterial;
    [SerializeField] private Material _rockMaterial;

    private LowPolyHill[] _hills;

    private void Start()
    {
        _hills = FindObjectsByType<LowPolyHill>(FindObjectsSortMode.None);
        System.Random random = new System.Random(_seed);
        Mesh trunk = LowPolyMesh.CreateCylinder(0.16f, 0.8f, 5);
        Mesh[] crowns =
        {
            LowPolyMesh.CreateCone(1.25f, 1.5f, 6),
            LowPolyMesh.CreateCone(0.98f, 1.3f, 6),
            LowPolyMesh.CreateCone(0.7f, 1.1f, 6)
        };

        CreateHorizonGround();
        CreateMounds(random);
        CreateBushes(random);
        CreateMotes();
        for (int i = 0; i < _pines; i++)
        {
            if (TryPickSpot(random, out Vector3 spot))
            {
                CreatePine(spot, 0.8f + (float)random.NextDouble() * 1.1f, random, trunk, crowns);
            }
        }
        for (int i = 0; i < _rocas; i++)
        {
            if (TryPickSpot(random, out Vector3 spot))
            {
                CreateRock(spot, 0.5f + (float)random.NextDouble() * 1.3f, random.Next());
            }
        }
    }

    // Lomas bajas y anchas que rompen la planicie del suelo; solo decoran, fuera del pasillo y de las colinas.
    private void CreateMounds(System.Random random)
    {
        Material[] materials =
        {
            MakeMoundMaterial(new Color(0.58f, 0.52f, 0.72f)),
            MakeMoundMaterial(new Color(0.52f, 0.58f, 0.70f)),
            MakeMoundMaterial(new Color(0.66f, 0.54f, 0.70f))
        };
        for (int i = 0; i < 14; i++)
        {
            if (!TryPickSpot(random, out Vector3 spot))
            {
                continue;
            }
            float radius = 5f + (float)random.NextDouble() * 8f;
            float height = 0.8f + (float)random.NextDouble() * 1.6f;
            GameObject mound = new GameObject("Loma");
            mound.transform.SetParent(transform);
            mound.transform.position = spot + Vector3.down * 0.15f;
            mound.AddComponent<MeshFilter>().sharedMesh = LowPolyMesh.CreateDome(radius, height, 12, 3, 0.18f, random.Next());
            mound.AddComponent<MeshRenderer>().sharedMaterial = materials[random.Next(materials.Length)];
        }
    }

    // Suelo enorme bajo el escenario: tapa el borde plano del terreno y se funde con la niebla.
    private void CreateHorizonGround()
    {
        GameObject ground = new GameObject("Horizonte");
        ground.transform.SetParent(transform);
        ground.transform.position = new Vector3(0f, -0.06f, 0f);
        Mesh disc = LowPolyMesh.CreateCylinder(700f, 0.1f, 24);
        ground.AddComponent<MeshFilter>().sharedMesh = disc;
        ground.AddComponent<MeshRenderer>().sharedMaterial = MakeMoundMaterial(new Color(0.60f, 0.55f, 0.70f));
    }

    // Arbustos pequeños de caras planas, en dos verdes, repartidos entre los pinos.
    private void CreateBushes(System.Random random)
    {
        Material[] greens =
        {
            MakeMoundMaterial(new Color(0.30f, 0.62f, 0.45f)),
            MakeMoundMaterial(new Color(0.40f, 0.68f, 0.42f))
        };
        for (int i = 0; i < 70; i++)
        {
            if (!TryPickSpot(random, out Vector3 spot))
            {
                continue;
            }
            float size = 0.4f + (float)random.NextDouble() * 0.6f;
            GameObject bush = new GameObject("Arbusto");
            bush.transform.SetParent(transform);
            bush.transform.position = spot + Vector3.down * 0.05f;
            bush.AddComponent<MeshFilter>().sharedMesh = LowPolyMesh.CreateDome(size, size * 0.7f, 6, 2, 0.12f, random.Next());
            bush.AddComponent<MeshRenderer>().sharedMaterial = greens[random.Next(greens.Length)];
        }
    }

    // Motas de luz que flotan despacio por el atardecer.
    private void CreateMotes()
    {
        GameObject motes = new GameObject("Motas de luz");
        motes.transform.SetParent(transform);
        motes.transform.position = new Vector3(0f, 6f, 0f);
        ParticleSystem particles = motes.AddComponent<ParticleSystem>();
        ParticleSystem.MainModule main = particles.main;
        main.loop = true;
        main.startLifetime = 9f;
        main.startSpeed = 0.25f;
        main.startSize = new ParticleSystem.MinMaxCurve(0.08f, 0.2f);
        main.startColor = new ParticleSystem.MinMaxGradient(new Color(2f, 1.5f, 0.9f, 0.8f), new Color(1.8f, 0.9f, 1.6f, 0.8f));
        main.maxParticles = 180;
        main.prewarm = true;
        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 20f;
        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(100f, 12f, 60f);
        ParticleSystem.ColorOverLifetimeModule fade = particles.colorOverLifetime;
        fade.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new[] { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(1f, 0.3f), new GradientAlphaKey(0f, 1f) });
        fade.color = gradient;
        motes.GetComponent<ParticleSystemRenderer>().material = SoftDot.GetMaterial();
    }

    private static Material MakeMoundMaterial(Color color)
    {
        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        material.SetColor("_BaseColor", color);
        material.SetFloat("_Smoothness", 0.05f);
        return material;
    }

    private bool TryPickSpot(System.Random random, out Vector3 spot)
    {
        for (int attempt = 0; attempt < 20; attempt++)
        {
            float x = ((float)random.NextDouble() * 2f - 1f) * AreaX;
            float z = ((float)random.NextDouble() * 2f - 1f) * AreaZ;
            if (IsFree(x, z))
            {
                spot = new Vector3(x, 0f, z);
                return true;
            }
        }
        spot = Vector3.zero;
        return false;
    }

    private bool IsFree(float x, float z)
    {
        if (Mathf.Abs(z) < CorridorHalfWidth && Mathf.Abs(x) < CorridorHalfLength)
        {
            return false;
        }
        foreach (LowPolyHill hill in _hills)
        {
            Vector3 center = hill.transform.position;
            if (Vector2.Distance(new Vector2(x, z), new Vector2(center.x, center.z)) < hill.GetRadius() + 2f)
            {
                return false;
            }
        }
        return true;
    }

    private void CreatePine(Vector3 position, float scale, System.Random random, Mesh trunk, Mesh[] crowns)
    {
        GameObject pine = new GameObject("Pino");
        pine.transform.SetParent(transform);
        pine.transform.position = position;
        pine.transform.localScale = Vector3.one * scale;
        pine.transform.rotation = Quaternion.Euler(0f, random.Next(0, 360), 0f);

        AddPart(pine.transform, trunk, _trunkMaterial, 0f);
        Material crownMaterial = _pineMaterials[random.Next(_pineMaterials.Length)];
        float[] heights = { 0.55f, 1.35f, 2.05f };
        for (int i = 0; i < crowns.Length; i++)
        {
            AddPart(pine.transform, crowns[i], crownMaterial, heights[i]);
        }
    }

    private void CreateRock(Vector3 position, float scale, int seed)
    {
        GameObject rock = new GameObject("Roca");
        rock.transform.SetParent(transform);
        rock.transform.position = position + Vector3.down * scale * 0.2f;
        rock.transform.localScale = new Vector3(scale, scale * 0.7f, scale);
        AddPart(rock.transform, LowPolyMesh.CreateRock(1f, 0.25f, seed), _rockMaterial, 0f);
    }

    private static void AddPart(Transform parent, Mesh mesh, Material material, float height)
    {
        GameObject part = new GameObject("Parte");
        part.transform.SetParent(parent, false);
        part.transform.localPosition = new Vector3(0f, height, 0f);
        part.AddComponent<MeshFilter>().sharedMesh = mesh;
        part.AddComponent<MeshRenderer>().sharedMaterial = material;
    }
}
