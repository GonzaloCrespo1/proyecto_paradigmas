# Hill Slingers

Artillería 3D por turnos contra un bot que aprende. Proyecto final de la asignatura Paradigmas y Técnicas de Programación (ICAI, curso 26/27), desarrollado por Mateo Gómez-Acebo y Gonzalo Crespo Comín.

- **Unity:** la misma versión para los dos (el enunciado pide 6000.6.8f1).
- **Documentación:** `docs/Hill_Slingers_Cuaderno.docx` (cuaderno de desarrollo). Solo una persona lo edita a la vez.

## Cómo trabajamos

### Ramas
- `main` siempre compila y se puede jugar. No se hace push directo a `main`.
- Una rama por fase, con el nombre `fase-N-nombre` (por ejemplo `fase-2-flujo-partida`).
- Cada fase termina con un pull request revisado por la otra persona. Al unirlo a `main` se pone la etiqueta `fase-N`.

### Commits
- Un commit por subpaso del plan como mínimo, y más si hace falta.
- El mensaje describe en pocas palabras qué se ha hecho. Ejemplo: `estados de partida`. La relación entre subpasos y commits queda registrada en el cuaderno de desarrollo.

### Unity y git
- Se suben siempre los archivos `.meta`, junto a su script o asset.
- No se suben `Library`, `Temp`, `Logs`, `UserSettings` ni `obj`: ya los ignora el `.gitignore`.
- Mover o renombrar archivos siempre desde la ventana Project de Unity, nunca desde el Explorador.
- Una sola persona toca una escena a la vez; lo demás va en prefabs.
- Hacer pull antes de empezar a trabajar.

## Estructura del proyecto

```
Assets/_Project/
  Scenes/      escenas
  Scripts/
    Core/      flujo de la partida y eventos
    Gameplay/  terreno, armas, proyectiles y vida
    AI/        bot
    UI/        interfaz
  Data/        datos de armas y configuración
  Prefabs/     prefabs
docs/          cuaderno de desarrollo y diseño
```

## Convenciones de código
- Clases, interfaces y métodos en `PascalCase`; las interfaces empiezan por `I` y solo declaran métodos.
- Variables en `camelCase`; los campos privados llevan `_` delante.
- Un archivo por clase, con el mismo nombre que la clase.
- Antes de unir una rama se revisa: una responsabilidad por clase, sin código duplicado (DRY), sin código "por si acaso" (YAGNI), sin cadenas del tipo `a.b().c().d()` (Ley de Deméter) y con los eventos desuscritos al destruir el objeto.
