// Orden: cambiar la fuerza de disparo en una cantidad (positiva sube, negativa baja).
public class ChangeForceCommand : ICommand
{
    private readonly AimSettings _aim;
    private readonly float _amount;

    public ChangeForceCommand(AimSettings aim, float amount)
    {
        _aim = aim;
        _amount = amount;
    }

    public void Execute()
    {
        _aim.ChangeForce(_amount);
    }
}
