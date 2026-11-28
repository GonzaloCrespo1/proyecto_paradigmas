using System.Collections.Generic;

// De dónde vienen las órdenes de apuntado: el teclado del jugador o el bot.
// AimingState solo conoce esta interfaz, así que no sabe quién está apuntando.
public interface IAimSource
{
    void Enable();
    void Disable();
    List<ICommand> ReadCommands(AimSettings aim);
    bool WasFireRequested();
}
