using System.Collections.Generic;
using UnityEngine;

// El bot apunta como un jugador: espera un momento y va generando las mismas órdenes (Command) que el teclado.
// Primero mueve elevación y giro hasta su decisión, luego carga la fuerza y dispara.
public class BotInput : IAimSource
{
    private const float ThinkSeconds = 1f;
    private const float AngleSpeed = 40f;
    private const float YawSpeed = 40f;
    private const float ChargeSpeed = 40f;

    private readonly IAimDecision _strategy;
    private AimDecision _decision;
    private float _startTime;
    private bool _charging;
    private bool _fire;

    public BotInput(IAimDecision strategy)
    {
        _strategy = strategy;
    }

    public void Enable()
    {
        _decision = _strategy.Decide();
        _startTime = Time.time;
        _charging = false;
        _fire = false;
    }

    public void Disable()
    {
    }

    public List<ICommand> ReadCommands(AimSettings aim)
    {
        List<ICommand> commands = new List<ICommand>();
        if (Time.time - _startTime < ThinkSeconds)
        {
            return commands;
        }

        float angleStep = Step(_decision.GetAngle() - aim.GetAngle(), AngleSpeed);
        float yawStep = Step(_decision.GetYaw() - aim.GetYaw(), YawSpeed);
        if (angleStep != 0f)
        {
            commands.Add(new ChangeAngleCommand(aim, angleStep));
        }
        if (yawStep != 0f)
        {
            commands.Add(new ChangeYawCommand(aim, yawStep));
        }
        if (angleStep != 0f || yawStep != 0f)
        {
            return commands;
        }

        if (!_charging)
        {
            _charging = true;
            commands.Add(new ResetForceCommand(aim));
        }
        else if (aim.GetForce() < _decision.GetForce())
        {
            commands.Add(new ChangeForceCommand(aim, ChargeSpeed * Time.deltaTime));
        }
        else
        {
            _fire = true;
        }
        return commands;
    }

    public bool WasFireRequested()
    {
        return _fire;
    }

    // Cuánto moverse este fotograma hacia el objetivo, sin pasarse.
    private float Step(float difference, float speed)
    {
        float maxStep = speed * Time.deltaTime;
        return Mathf.Abs(difference) <= maxStep ? difference : Mathf.Sign(difference) * maxStep;
    }
}
