using UnityEngine;

// Columna de luz sobre la zona del rival del jugador que tiene el turno.
// Sustituye el cilindro plano por un tubo abierto con degradado vertical (opaco abajo, transparente arriba),
// un resplandor circular en el suelo y un latido suave. Escucha el cambio de turno (Observer).
public class RivalHalo : MonoBehaviour
{
    private const int Segments = 28;
    private const float PulseSpeed = 1.6f;
    private const float PulseAmount = 0.06f;
    private static readonly Color Glow = new Color(1f, 0.82f, 0.95f);

    [SerializeField] private GameObject[] _halos;

    private Material _material;
    private Transform[] _columns;

    private void Awake()
    {
        _material = new Material(Shader.Find("Sprites/Default"));
        _columns = new Transform[_halos.Length];
        for (int i = 0; i < _halos.Length; i++)
        {
            _columns[i] = BuildColumn(_halos[i]);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnTurnChanged += Refresh;
    }

    private void OnDisable()
    {
        GameEvents.OnTurnChanged -= Refresh;
    }

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        float pulse = 1f + Mathf.Sin(Time.time * PulseSpeed) * PulseAmount;
        foreach (Transform column in _columns)
        {
            column.localScale = new Vector3(pulse, 1f, pulse);
        }
    }

    private void Refresh()
    {
        GameController controller = GameController.GetInstance();
        if (controller == null)
        {
            return;
        }

        for (int i = 0; i < _halos.Length; i++)
        {
            _halos[i].SetActive(i != controller.GetCurrentPlayerIndex());
        }
    }

    // Convierte el cilindro original en la columna de luz, conservando su posición, radio y altura.
    private Transform BuildColumn(GameObject halo)
    {
        Renderer original = halo.GetComponent<Renderer>();
        Bounds bounds = original.bounds;
        float radius = bounds.extents.x;
        float height = bounds.size.y;
        Vector3 basePoint = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);

        Destroy(halo.GetComponent<Collider>());
        Destroy(original);
        Destroy(halo.GetComponent<MeshFilter>());
        halo.transform.SetPositionAndRotation(basePoint, Quaternion.identity);
        halo.transform.localScale = Vector3.one;

        GameObject column = new GameObject("Columna de luz", typeof(MeshFilter), typeof(MeshRenderer));
        column.transform.SetParent(halo.transform, false);
        column.GetComponent<MeshFilter>().sharedMesh = BuildTube(radius, height);
        column.GetComponent<MeshRenderer>().sharedMaterial = _material;

        GameObject ground = new GameObject("Resplandor", typeof(MeshFilter), typeof(MeshRenderer));
        ground.transform.SetParent(halo.transform, false);
        ground.transform.localPosition = Vector3.up * 0.08f;
        ground.GetComponent<MeshFilter>().sharedMesh = BuildDisc(radius * 2.2f);
        ground.GetComponent<MeshRenderer>().sharedMaterial = _material;
        return column.transform;
    }

    // Tres anillos: opaco abajo, ya muy tenue a un cuarto de altura y transparente arriba, para que el degradado se note.
    private static Mesh BuildTube(float radius, float height)
    {
        float[] levels = { 0f, 0.25f, 1f };
        float[] alphas = { 0.34f, 0.12f, 0f };
        int rows = levels.Length;
        Vector3[] vertices = new Vector3[(Segments + 1) * rows];
        Color[] colors = new Color[vertices.Length];
        int[] triangles = new int[Segments * (rows - 1) * 6];
        int t = 0;
        for (int r = 0; r < rows; r++)
        {
            for (int i = 0; i <= Segments; i++)
            {
                float angle = i / (float)Segments * Mathf.PI * 2f;
                int index = r * (Segments + 1) + i;
                vertices[index] = new Vector3(Mathf.Cos(angle) * radius, levels[r] * height, Mathf.Sin(angle) * radius);
                colors[index] = new Color(Glow.r, Glow.g, Glow.b, alphas[r]);
                if (r < rows - 1 && i < Segments)
                {
                    int up = index + Segments + 1;
                    triangles[t++] = index; triangles[t++] = up; triangles[t++] = index + 1;
                    triangles[t++] = index + 1; triangles[t++] = up; triangles[t++] = up + 1;
                }
            }
        }
        Mesh mesh = new Mesh { name = "Tubo de luz" };
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        return mesh;
    }

    private static Mesh BuildDisc(float radius)
    {
        Vector3[] vertices = new Vector3[Segments + 1];
        Color[] colors = new Color[vertices.Length];
        int[] triangles = new int[Segments * 3];
        vertices[0] = Vector3.zero;
        colors[0] = new Color(Glow.r, Glow.g, Glow.b, 0.4f);
        for (int i = 0; i < Segments; i++)
        {
            float angle = i / (float)Segments * Mathf.PI * 2f;
            vertices[i + 1] = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
            colors[i + 1] = new Color(Glow.r, Glow.g, Glow.b, 0f);
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = (i + 1) % Segments + 1;
            triangles[i * 3 + 2] = i + 1;
        }
        Mesh mesh = new Mesh { name = "Resplandor" };
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        return mesh;
    }
}
