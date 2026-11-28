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
