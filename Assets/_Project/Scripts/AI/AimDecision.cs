// Decisión de tiro del bot: a qué elevación, giro y fuerza quiere disparar. Solo guarda datos.
public class AimDecision
{
    private readonly float _angle;
    private readonly float _yaw;
    private readonly float _force;

    public AimDecision(float angle, float yaw, float force)
    {
        _angle = angle;
        _yaw = yaw;
        _force = force;
    }

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
}
