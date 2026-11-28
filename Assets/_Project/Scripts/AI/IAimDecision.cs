// Estrategia (patrón Strategy) con la que el bot decide su tiro.
// Hoy hay una aleatoria; más adelante habrá otra que aprende de sus tiros anteriores.
public interface IAimDecision
{
    AimDecision Decide();
}
