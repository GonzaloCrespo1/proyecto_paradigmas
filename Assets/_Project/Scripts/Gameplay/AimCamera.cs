using UnityEngine;

// Cámara en tercera persona: se coloca detrás del jugador que dispara, mirando hacia donde apunta.
// Se mueve con suavidad, así al cambiar de turno barre de un jugador al otro.
public class AimCamera : MonoBehaviour
{
    private const float Distance = 7f;
    private const float Height = 2.5f;
    private const float ShoulderOffset = 0.9f;
    private const float LookAhead = 14f;
    private const float LookHeight = 1.5f;
    private const float FollowSpeed = 4f;

    private void LateUpdate()
    {
        GameController controller = GameController.GetInstance();
        if (controller == null)
        {
            return;
        }

        Vector3 player = controller.GetLaunchPosition();
        Vector3 forward = ShotPhysics.GetDirection(controller.GetCurrentAim().GetYaw(), controller.GetCurrentPlayerIndex());
        Vector3 right = Vector3.Cross(Vector3.up, forward);

        Vector3 targetPosition = player - forward * Distance + Vector3.up * Height + right * ShoulderOffset;
        Vector3 lookPoint = player + forward * LookAhead + Vector3.up * LookHeight;
        Quaternion targetRotation = Quaternion.LookRotation(lookPoint - targetPosition);

        float blend = 1f - Mathf.Exp(-FollowSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPosition, blend);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, blend);
    }
}
