using UnityEngine;

// Cámara en tercera persona. Tiene tres modos:
// - Apuntando: detrás del jugador que dispara, mirando hacia donde apunta.
// - En vuelo: detrás del proyectil, mirando hacia donde va, para ver dónde cae.
// - Impacto: sube y mira al rival, que se acaba de revelar.
// En todos se mueve con suavidad, así al cambiar de modo o de turno barre en lugar de saltar.
public class AimCamera : MonoBehaviour
{
    private const float AimDistance = 7f;
    private const float AimHeight = 2.5f;
    private const float ShoulderOffset = 0.9f;
    private const float LookAhead = 14f;
    private const float LookHeight = 1.5f;
    private const float AimFollowSpeed = 4f;

    private const float ChaseDistance = 6f;
    private const float ChaseHeight = 2f;
    private const float ChaseLookAhead = 6f;
    private const float ChaseFollowSpeed = 6f;
    private const float MinChaseSpeed = 0.5f;

    private const float RevealDistance = 16f;
    private const float RevealHeight = 10f;
    private const float RevealFollowSpeed = 2.5f;

    private const float AimFov = 60f;
    private const float ChaseFov = 74f;
    private const float RevealFov = 52f;
    private const float ShakeSeconds = 0.4f;
    private const float ShakeStrength = 0.5f;

    private Vector3 _chaseForward = Vector3.right;
    private float _shakeUntil;
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        GameEvents.OnProjectileLanded += StartShake;
    }

    private void OnDisable()
    {
        GameEvents.OnProjectileLanded -= StartShake;
    }

    private void StartShake()
    {
        _shakeUntil = Time.time + ShakeSeconds;
    }

    private void LateUpdate()
    {
        GameController controller = GameController.GetInstance();
        if (controller == null)
        {
            return;
        }

        Rigidbody projectile = controller.GetProjectile();
        if (controller.IsImpact())
        {
            Reveal(controller);
        }
        else if (projectile != null)
        {
            Chase(projectile);
        }
        else
        {
            Aim(controller);
        }
    }

    private void Aim(GameController controller)
    {
        Vector3 player = controller.GetLaunchPosition();
        Vector3 forward = ShotPhysics.GetDirection(controller.GetCurrentAim().GetYaw(), controller.GetCurrentPlayerIndex());
        Vector3 right = Vector3.Cross(Vector3.up, forward);

        Vector3 targetPosition = player - forward * AimDistance + Vector3.up * AimHeight + right * ShoulderOffset;
        Vector3 lookPoint = player + forward * LookAhead + Vector3.up * LookHeight;
        MoveTo(targetPosition, lookPoint, AimFollowSpeed, AimFov);
    }

    private void Chase(Rigidbody projectile)
    {
        // Dirección horizontal del vuelo; si casi no se mueve, se conserva la última.
        Vector3 flat = new Vector3(projectile.linearVelocity.x, 0f, projectile.linearVelocity.z);
        if (flat.magnitude > MinChaseSpeed)
        {
            _chaseForward = flat.normalized;
        }

        Vector3 position = projectile.position;
        Vector3 targetPosition = position - _chaseForward * ChaseDistance + Vector3.up * ChaseHeight;
        Vector3 lookPoint = position + _chaseForward * ChaseLookAhead;
        MoveTo(targetPosition, lookPoint, ChaseFollowSpeed, ChaseFov);
    }

    private void Reveal(GameController controller)
    {
        Vector3 rival = controller.GetRivalPosition();
        Vector3 forward = ShotPhysics.GetDirection(0f, controller.GetCurrentPlayerIndex());

        Vector3 targetPosition = rival - forward * RevealDistance + Vector3.up * RevealHeight;
        Vector3 lookPoint = rival + Vector3.up * LookHeight;
        MoveTo(targetPosition, lookPoint, RevealFollowSpeed, RevealFov);
    }

    private void MoveTo(Vector3 targetPosition, Vector3 lookPoint, float speed, float fov)
    {
        Quaternion targetRotation = Quaternion.LookRotation(lookPoint - targetPosition);
        float blend = 1f - Mathf.Exp(-speed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPosition, blend);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, blend);
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, fov, blend);

        // Sacudida corta tras el impacto, que se apaga sola.
        float remaining = (_shakeUntil - Time.time) / ShakeSeconds;
        if (remaining > 0f)
        {
            transform.position += Random.insideUnitSphere * ShakeStrength * remaining;
        }
    }
}
