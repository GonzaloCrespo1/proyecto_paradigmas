// Orden: dejar la fuerza en su valor mínimo (al empezar a cargar el disparo).
public class ResetForceCommand : ICommand
{
    private readonly AimSettings _aim;

    public ResetForceCommand(AimSettings aim)
    {
        _aim = aim;
    }

    public void Execute()
    {
        _aim.ResetForce();
    }
}
