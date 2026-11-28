using UnityEngine;

// Personaje low-poly del jugador: cuerpo, cabeza y gorra del color de su equipo.
// Sustituye al cubo provisional y mira hacia donde apunta su jugador (+X o -X).
public class PlayerAvatar : MonoBehaviour
{
    [SerializeField] private Material _bodyMaterial;
    [SerializeField] private Material _skinMaterial;
    [SerializeField] private Material _capMaterial;
    [SerializeField] private float _facing = 1f;

    private void Awake()
    {
        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<MeshFilter>());
        Destroy(GetComponent<BoxCollider>());
        CapsuleCollider capsule = gameObject.AddComponent<CapsuleCollider>();
        capsule.height = 1.8f;
        capsule.radius = 0.4f;
        capsule.center = new Vector3(0f, 0.1f, 0f);

        AddPart("Cuerpo", LowPolyMesh.CreateCylinder(0.38f, 0.95f, 6), _bodyMaterial, new Vector3(0f, -0.5f, 0f), Vector3.one);
        AddPart("Cabeza", LowPolyMesh.CreateRock(0.3f, 0.05f, 3), _skinMaterial, new Vector3(0f, 0.72f, 0f), Vector3.one);
        AddPart("Gorra", LowPolyMesh.CreateDome(0.33f, 0.3f, 7, 2, 0f, 1), _capMaterial, new Vector3(0f, 0.78f, 0f), Vector3.one);
        AddPart("Visera", LowPolyMesh.CreateCylinder(0.26f, 0.05f, 6), _capMaterial, new Vector3(_facing * 0.24f, 0.8f, 0f), new Vector3(1f, 1f, 0.9f));
    }

    private void AddPart(string partName, Mesh mesh, Material material, Vector3 localPosition, Vector3 scale)
    {
        GameObject part = new GameObject(partName);
        part.transform.SetParent(transform, false);
        part.transform.localPosition = localPosition;
        part.transform.localScale = scale;
        part.AddComponent<MeshFilter>().sharedMesh = mesh;
        part.AddComponent<MeshRenderer>().sharedMaterial = material;
    }
}
