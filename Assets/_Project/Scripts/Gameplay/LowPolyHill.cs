using UnityEngine;

// Colina low-poly: al empezar genera su malla y su colisión.
// Solo se configuran radio, altura y semilla; así luego se podrán generar mapas distintos entre rondas.
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LowPolyHill : MonoBehaviour
{
    [SerializeField] private float _radius = 8f;
    [SerializeField] private float _height = 3f;
    [SerializeField] private int _segments = 9;
    [SerializeField] private int _rings = 4;
    [SerializeField] private float _jitter = 0.07f;
    [SerializeField] private int _seed = 1;

    private void Awake()
    {
        Mesh mesh = LowPolyMesh.CreateDome(_radius, _height, _segments, _rings, _jitter, _seed);
        GetComponent<MeshFilter>().sharedMesh = mesh;
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

    public float GetRadius()
    {
        return _radius;
    }
}
