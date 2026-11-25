using UnityEngine;

// Datos del apuntado de un jugador: ángulo (grados) y fuerza (de 10 a 100).
// Solo guarda los dos valores y los mantiene dentro de sus límites.
public class AimSettings
{
    private const float MinAngle = 5f;
    private const float MaxAngle = 85f;
    private const float MinForce = 10f;
    private const float MaxForce = 100f;
    private const float DefaultAngle = 45f;
    private const float DefaultForce = 50f;

    private float _angle = DefaultAngle;
    private float _force = DefaultForce;

    public float GetAngle()
    {
        return _angle;
    }

    public float GetForce()
    {
        return _force;
    }

    public void ChangeAngle(float amount)
    {
        _angle = Mathf.Clamp(_angle + amount, MinAngle, MaxAngle);
    }

    public void ChangeForce(float amount)
    {
        _force = Mathf.Clamp(_force + amount, MinForce, MaxForce);
    }

    public void Reset()
    {
        _angle = DefaultAngle;
        _force = DefaultForce;
    }
}
