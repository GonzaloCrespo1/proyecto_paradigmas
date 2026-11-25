// Contrato común de todos los estados de la partida.
// Solo declara métodos: qué hace cada estado lo decide cada clase.
public interface IGameState
{
    void Enter();
    void Update();
    void Exit();
}
