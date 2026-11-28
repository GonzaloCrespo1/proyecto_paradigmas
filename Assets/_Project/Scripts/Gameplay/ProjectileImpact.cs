using UnityEngine;

// Detecta el primer choque del proyectil: lo deja quieto y avisa a la partida (Observer).
public class ProjectileImpact : MonoBehaviour
{
    private bool _landed;

    private void OnCollisionEnter(Collision collision)
    {
        if (_landed)
        {
            return;
        }

        _landed = true;
        ImpactEffect.Spawn(transform.position);
        Rigidbody body = GetComponent<Rigidbody>();
        body.linearVelocity = Vector3.zero;
        body.isKinematic = true;
        GameEvents.RaiseProjectileLanded();
    }
}
