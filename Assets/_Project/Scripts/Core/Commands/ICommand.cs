// Contrato de una orden del jugador (o del bot): una acción lista para ejecutarse.
public interface ICommand
{
    void Execute();
}
