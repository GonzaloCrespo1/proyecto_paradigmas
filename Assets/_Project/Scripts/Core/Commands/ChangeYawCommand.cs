// Orden: girar el apuntado en horizontal (positivo gira a la derecha, negativo a la izquierda).
public class ChangeYawCommand : ICommand
{
    private readonly AimSettings _aim;
    private readonly float _amount;

    public ChangeYawCommand(AimSettings aim, float amount)
    {
        _aim = aim;
        _amount = amount;
    }

    public void Execute()
    {
        _aim.ChangeYaw(_amount);
    }
}
