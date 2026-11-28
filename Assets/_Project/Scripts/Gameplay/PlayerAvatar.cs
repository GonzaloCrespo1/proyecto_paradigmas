using UnityEngine;

// Personaje low-poly del jugador: piernas, cuerpo, brazos, cabeza y gorra con visera del color de su equipo.
// Mira hacia donde apunta su jugador (+X o -X), respira con un balanceo suave y retrocede un poco al disparar.
public class PlayerAvatar : MonoBehaviour
{
    private const float BobSpeed = 2.2f;
    private const float BobHeight = 0.03f;
    private const float RecoilDistance = 0.28f;
    private const float RecoilRecovery = 6f;

    [SerializeField] private Material _bodyMaterial;
    [SerializeField] private Material _skinMaterial;
    [SerializeField] private Material _capMaterial;
    [SerializeField] private float _facing = 1f;

    private Transform _model;
    private float _recoil;
    private float _phase;

    private void Awake()
    {
        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<MeshFilter>());
        Destroy(GetComponent<BoxCollider>());
        CapsuleCollider capsule = gameObject.AddComponent<CapsuleCollider>();
        capsule.height = 1.8f;
        capsule.radius = 0.4f;
        capsule.center = new Vector3(0f, 0.1f, 0f);

        _model = new GameObject("Modelo").transform;
        _model.SetParent(transform, false);
        _phase = Random.value * 6f;

        Mesh leg = LowPolyMesh.CreateCylinder(0.13f, 0.5f, 6);
        AddPart("Pierna I", leg, _bodyMaterial, new Vector3(0f, -0.8f, 0.17f), Quaternion.identity);
        AddPart("Pierna D", leg, _bodyMaterial, new Vector3(0f, -0.8f, -0.17f), Quaternion.identity);
        AddPart("Cuerpo", LowPolyMesh.CreateCylinder(0.36f, 0.75f, 6), _bodyMaterial, new Vector3(0f, -0.36f, 0f), Quaternion.identity);

        Mesh arm = LowPolyMesh.CreateCylinder(0.1f, 0.55f, 5);
        Quaternion lean = Quaternion.Euler(0f, 0f, _facing * 14f);
        AddPart("Brazo I", arm, _bodyMaterial, new Vector3(0.02f * _facing, -0.25f, 0.46f), lean);
        AddPart("Brazo D", arm, _bodyMaterial, new Vector3(0.02f * _facing, -0.25f, -0.46f), lean);
        Mesh hand = LowPolyMesh.CreateRock(0.1f, 0.05f, 5);
        AddPart("Mano I", hand, _skinMaterial, new Vector3(0.13f * _facing, -0.27f, 0.47f), Quaternion.identity);
        AddPart("Mano D", hand, _skinMaterial, new Vector3(0.13f * _facing, -0.27f, -0.47f), Quaternion.identity);

        AddPart("Cabeza", LowPolyMesh.CreateRock(0.3f, 0.05f, 3), _skinMaterial, new Vector3(0f, 0.72f, 0f), Quaternion.identity);
        AddPart("Gorra", LowPolyMesh.CreateDome(0.34f, 0.3f, 7, 2, 0f, 1), _capMaterial, new Vector3(0f, 0.78f, 0f), Quaternion.identity);
        AddPart("Visera", LowPolyMesh.CreateCylinder(0.27f, 0.05f, 6), _capMaterial, new Vector3(_facing * 0.25f, 0.8f, 0f), Quaternion.identity, new Vector3(1f, 1f, 0.9f));
    }

    private void OnEnable()
    {
        GameEvents.OnShotFired += OnShotFired;
    }

    private void OnDisable()
    {
        GameEvents.OnShotFired -= OnShotFired;
    }

    private void OnShotFired()
    {
        GameController controller = GameController.GetInstance();
        if (controller != null && controller.GetCurrentPlayer() == transform)
        {
            _recoil = 1f;
        }
    }

    private void Update()
    {
        _recoil = Mathf.MoveTowards(_recoil, 0f, RecoilRecovery * Time.deltaTime);
        float bob = Mathf.Sin(Time.time * BobSpeed + _phase) * BobHeight;
        _model.localPosition = new Vector3(-_facing * RecoilDistance * _recoil, bob, 0f);
        _model.localScale = new Vector3(1f, 1f + bob * 0.5f, 1f);
    }

    private void AddPart(string partName, Mesh mesh, Material material, Vector3 localPosition, Quaternion rotation)
    {
        AddPart(partName, mesh, material, localPosition, rotation, Vector3.one);
    }

    private void AddPart(string partName, Mesh mesh, Material material, Vector3 localPosition, Quaternion rotation, Vector3 scale)
    {
        GameObject part = new GameObject(partName);
        part.transform.SetParent(_model, false);
        part.transform.localPosition = localPosition;
        part.transform.localRotation = rotation;
        part.transform.localScale = scale;
        part.AddComponent<MeshFilter>().sharedMesh = mesh;
        part.AddComponent<MeshRenderer>().sharedMaterial = material;
    }
}
