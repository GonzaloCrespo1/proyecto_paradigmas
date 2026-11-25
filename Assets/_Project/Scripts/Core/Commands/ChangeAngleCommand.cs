// Orden: cambiar el ángulo de disparo en una cantidad (positiva sube, negativa baja).
public class ChangeAngleCommand : ICommand
{
    private readonly AimSettings _aim;
    private readonly float _amount;

    public ChangeAngleCommand(AimSettings aim, float amount)
    {
        _aim = aim;
        _amount = amount;
    }

    public void Execute()
    {
        _aim.ChangeAngle(_amount);
    }
}
