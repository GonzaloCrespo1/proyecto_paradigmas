using System.Collections.Generic;
using UnityEngine;

// Fábrica de mallas low-poly con caras planas (cada triángulo con su propia normal).
// Es lógica pura: devuelve mallas, no crea objetos. La usan las colinas, los pinos, las rocas y los jugadores.
public static class LowPolyMesh
{
    // Cúpula (colina): ápice arriba, base en y = 0. El jitter deforma un poco los vértices para que no parezca perfecta.
    public static Mesh CreateDome(float radius, float height, int segments, int rings, float jitter, int seed)
    {
        System.Random random = new System.Random(seed);
        List<Vector3> corners = new List<Vector3> { new Vector3(0f, height, 0f) };
        for (int r = 1; r <= rings; r++)
        {
            float phi = (float)r / rings * Mathf.PI * 0.5f;
            for (int s = 0; s < segments; s++)
            {
                float angle = ((float)s + (r % 2) * 0.5f) / segments * Mathf.PI * 2f;
                float ring = radius * Mathf.Sin(phi) * (1f + Jitter(random, jitter));
                float y = height * Mathf.Cos(phi) + (r < rings ? Jitter(random, jitter) * height * 0.25f : 0f);
                corners.Add(new Vector3(ring * Mathf.Cos(angle), y, ring * Mathf.Sin(angle)));
            }
        }

        List<int> triangles = new List<int>();
        for (int s = 0; s < segments; s++)
        {
            triangles.AddRange(new[] { 0, 1 + s, 1 + (s + 1) % segments });
        }
        for (int r = 1; r < rings; r++)
        {
            int upper = 1 + (r - 1) * segments;
            int lower = 1 + r * segments;
            for (int s = 0; s < segments; s++)
            {
                int next = (s + 1) % segments;
                triangles.AddRange(new[] { upper + s, upper + next, lower + s });
                triangles.AddRange(new[] { upper + next, lower + next, lower + s });
            }
        }
        return Build(corners, triangles, new Vector3(0f, 0f, 0f));
    }

    // Cono (copa de un pino), base en y = 0.
    public static Mesh CreateCone(float radius, float height, int segments)
    {
        List<Vector3> corners = new List<Vector3> { new Vector3(0f, height, 0f), Vector3.zero };
        for (int s = 0; s < segments; s++)
        {
            float angle = (float)s / segments * Mathf.PI * 2f;
            corners.Add(new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle)));
        }

        List<int> triangles = new List<int>();
        for (int s = 0; s < segments; s++)
        {
            int next = 2 + (s + 1) % segments;
            triangles.AddRange(new[] { 0, 2 + s, next });
            triangles.AddRange(new[] { 1, 2 + s, next });
        }
        return Build(corners, triangles, new Vector3(0f, height * 0.3f, 0f));
    }

    // Cilindro (tronco, cuerpo del jugador), base en y = 0.
    public static Mesh CreateCylinder(float radius, float height, int segments)
    {
        List<Vector3> corners = new List<Vector3> { Vector3.zero, new Vector3(0f, height, 0f) };
        for (int s = 0; s < segments; s++)
        {
            float angle = (float)s / segments * Mathf.PI * 2f;
            corners.Add(new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle)));
        }
        for (int s = 0; s < segments; s++)
        {
            float angle = (float)s / segments * Mathf.PI * 2f;
            corners.Add(new Vector3(radius * Mathf.Cos(angle), height, radius * Mathf.Sin(angle)));
        }

        List<int> triangles = new List<int>();
        for (int s = 0; s < segments; s++)
        {
            int next = (s + 1) % segments;
            int bottom = 2 + s;
            int bottomNext = 2 + next;
            int top = 2 + segments + s;
            int topNext = 2 + segments + next;
            triangles.AddRange(new[] { bottom, top, bottomNext });
            triangles.AddRange(new[] { top, topNext, bottomNext });
            triangles.AddRange(new[] { 0, bottom, bottomNext });
            triangles.AddRange(new[] { 1, top, topNext });
        }
        return Build(corners, triangles, new Vector3(0f, height * 0.5f, 0f));
    }

    // Esfera irregular de pocas caras (rocas, cabeza del jugador), centrada en el origen.
    public static Mesh CreateRock(float radius, float jitter, int seed)
    {
        const int Segments = 7;
        float[] latitudes = { -0.9f, -0.4f, 0.25f, 0.8f };
        System.Random random = new System.Random(seed);

        List<Vector3> corners = new List<Vector3> { new Vector3(0f, -radius, 0f), new Vector3(0f, radius, 0f) };
        foreach (float latitude in latitudes)
        {
            float ring = Mathf.Sqrt(1f - latitude * latitude);
            for (int s = 0; s < Segments; s++)
            {
                float angle = ((float)s / Segments + latitude * 0.2f) * Mathf.PI * 2f;
                Vector3 direction = new Vector3(ring * Mathf.Cos(angle), latitude, ring * Mathf.Sin(angle));
                corners.Add(direction * radius * (1f + Jitter(random, jitter)));
            }
        }

        List<int> triangles = new List<int>();
        for (int s = 0; s < Segments; s++)
        {
            int next = (s + 1) % Segments;
            triangles.AddRange(new[] { 0, 2 + s, 2 + next });
            triangles.AddRange(new[] { 1, 2 + (latitudes.Length - 1) * Segments + s, 2 + (latitudes.Length - 1) * Segments + next });
        }
        for (int r = 0; r < latitudes.Length - 1; r++)
        {
            int lower = 2 + r * Segments;
            int upper = 2 + (r + 1) * Segments;
            for (int s = 0; s < Segments; s++)
            {
                int next = (s + 1) % Segments;
                triangles.AddRange(new[] { lower + s, upper + s, lower + next });
                triangles.AddRange(new[] { upper + s, upper + next, lower + next });
            }
        }
        return Build(corners, triangles, Vector3.zero);
    }

    private static float Jitter(System.Random random, float amount)
    {
        return ((float)random.NextDouble() * 2f - 1f) * amount;
    }

    // Duplica los vértices por triángulo (caras planas) y orienta cada cara hacia fuera respecto al centro.
    private static Mesh Build(List<Vector3> corners, List<int> triangles, Vector3 center)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        for (int i = 0; i < triangles.Count; i += 3)
        {
            Vector3 a = corners[triangles[i]];
            Vector3 b = corners[triangles[i + 1]];
            Vector3 c = corners[triangles[i + 2]];
            Vector3 normal = Vector3.Cross(b - a, c - a);
            bool outward = Vector3.Dot(normal, (a + b + c) / 3f - center) >= 0f;
            // Unity dibuja la cara si su normal (producto vectorial) mira hacia quien la ve.
            if (!outward)
            {
                Vector3 swap = b;
                b = c;
                c = swap;
            }
            int start = vertices.Count;
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(c);
            indices.AddRange(new[] { start, start + 1, start + 2 });
        }

        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(indices, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }
}
