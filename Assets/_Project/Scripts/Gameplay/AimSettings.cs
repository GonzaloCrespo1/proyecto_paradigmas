using UnityEngine;

// Datos del apuntado de un jugador: elevación (grados), giro horizontal (grados) y fuerza (de 10 a 100).
// También recuerda su último disparo. Solo guarda valores y los mantiene dentro de sus límites.
public class AimSettings
{
    public const float MinAngle = 5f;
    public const float MaxAngle = 85f;
    private const float MaxYaw = 75f;
    public const float MinForce = 10f;
    public const float MaxForce = 100f;
    private const float DefaultAngle = 45f;

    private float _angle = DefaultAngle;
    private float _yaw;
    private float _force = MinForce;
    private float _lastAngle;
    private float _lastYaw;
    private float _lastForce;
    private bool _hasLastShot;

    public float GetAngle()
    {
        return _angle;
    }

    public float GetYaw()
    {
        return _yaw;
    }

    public float GetForce()
    {
        return _force;
    }

    public float GetLastAngle()
    {
        return _lastAngle;
    }

    public float GetLastYaw()
    {
        return _lastYaw;
    }

    public float GetLastForce()
    {
        return _lastForce;
    }

    public bool HasLastShot()
    {
        return _hasLastShot;
    }

    public void ChangeAngle(float amount)
    {
        _angle = Mathf.Clamp(_angle + amount, MinAngle, MaxAngle);
    }

    public void ChangeYaw(float amount)
    {
        _yaw = Mathf.Clamp(_yaw + amount, -MaxYaw, MaxYaw);
    }

    public void ChangeForce(float amount)
    {
        _force = Mathf.Clamp(_force + amount, MinForce, MaxForce);
    }

    public void ResetForce()
    {
        _force = MinForce;
    }

    public void RememberShot()
    {
        _lastAngle = _angle;
        _lastYaw = _yaw;
        _lastForce = _force;
        _hasLastShot = true;
    }

    public void Reset()
    {
        _angle = DefaultAngle;
        _yaw = 0f;
        _force = MinForce;
        _hasLastShot = false;
    }
}
