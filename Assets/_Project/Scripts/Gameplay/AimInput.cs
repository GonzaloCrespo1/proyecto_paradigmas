using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Lee el teclado con Input Actions y lo convierte en órdenes (Command).
// W/S o flechas arriba/abajo: ángulo. D/A o flechas derecha/izquierda: fuerza. Espacio: disparar.
public class AimInput
{
    private const float AngleSpeed = 40f;
    private const float ForceSpeed = 50f;

    private readonly InputAction _angle = new InputAction("Angle", InputActionType.Value);
    private readonly InputAction _force = new InputAction("Force", InputActionType.Value);
    private readonly InputAction _fire = new InputAction("Fire", InputActionType.Button, "<Keyboard>/space");

    public AimInput()
    {
        _angle.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/s").With("Positive", "<Keyboard>/w");
        _angle.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/downArrow").With("Positive", "<Keyboard>/upArrow");
        _force.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/a").With("Positive", "<Keyboard>/d");
        _force.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/leftArrow").With("Positive", "<Keyboard>/rightArrow");
    }

    public void Enable()
    {
        _angle.Enable();
        _force.Enable();
        _fire.Enable();
    }

    public void Disable()
    {
        _angle.Disable();
        _force.Disable();
        _fire.Disable();
    }

    // Devuelve las órdenes de este fotograma para el apuntado indicado.
    public List<ICommand> ReadCommands(AimSettings aim)
    {
        List<ICommand> commands = new List<ICommand>();

        float angleAxis = _angle.ReadValue<float>();
        if (angleAxis != 0f)
        {
            commands.Add(new ChangeAngleCommand(aim, angleAxis * AngleSpeed * Time.deltaTime));
        }

        float forceAxis = _force.ReadValue<float>();
        if (forceAxis != 0f)
        {
            commands.Add(new ChangeForceCommand(aim, forceAxis * ForceSpeed * Time.deltaTime));
        }

        return commands;
    }

    public bool WasFirePressed()
    {
        return _fire.WasPressedThisFrame();
    }
}
