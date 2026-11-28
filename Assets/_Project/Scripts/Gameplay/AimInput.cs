using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Lee el teclado con Input Actions y lo convierte en órdenes (Command).
// W/S o flechas arriba/abajo: elevación. A/D o flechas izquierda/derecha: giro horizontal.
// Espacio: se mantiene pulsado para cargar la fuerza y, al soltarlo, la fuerza queda fija.
// Enter: dispara con la fuerza que haya fijada.
public class AimInput : IAimSource
{
    private const float AngleSpeed = 40f;
    private const float YawSpeed = 40f;
    private const float ChargeSpeed = 30f;

    private readonly InputAction _angle = new InputAction("Angle", InputActionType.Value);
    private readonly InputAction _yaw = new InputAction("Yaw", InputActionType.Value);
    private readonly InputAction _charge = new InputAction("Charge", InputActionType.Button, "<Keyboard>/space");
    private readonly InputAction _fire = new InputAction("Fire", InputActionType.Button, "<Keyboard>/enter");

    private bool _charging;

    public AimInput()
    {
        _fire.AddBinding("<Keyboard>/numpadEnter");
        _angle.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/s").With("Positive", "<Keyboard>/w");
        _angle.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/downArrow").With("Positive", "<Keyboard>/upArrow");
        _yaw.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/a").With("Positive", "<Keyboard>/d");
        _yaw.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/leftArrow").With("Positive", "<Keyboard>/rightArrow");
    }

    public void Enable()
    {
        _angle.Enable();
        _yaw.Enable();
        _charge.Enable();
        _fire.Enable();
    }

    public void Disable()
    {
        _angle.Disable();
        _yaw.Disable();
        _charge.Disable();
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

        float yawAxis = _yaw.ReadValue<float>();
        if (yawAxis != 0f)
        {
            commands.Add(new ChangeYawCommand(aim, yawAxis * YawSpeed * Time.deltaTime));
        }

        if (_charge.WasPressedThisFrame())
        {
            _charging = true;
            commands.Add(new ResetForceCommand(aim));
        }
        else if (_charging && _charge.IsPressed())
        {
            commands.Add(new ChangeForceCommand(aim, ChargeSpeed * Time.deltaTime));
        }
        else
        {
            _charging = false;
        }

        return commands;
    }

    // Es verdad en el fotograma en que se pulsa Enter: dispara con la fuerza ya fijada.
    public bool WasFireRequested()
    {
        return _fire.WasPressedThisFrame();
    }
}
