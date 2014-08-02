using System;

/// <summary>
/// Las diferentes fases de la batalla.
/// </summary>
public enum FasesBatalla 
{ 
    /// <summary>
    /// Todavia no hay ningun turno en la cola.
    /// </summary>
    EsperandoTurno, 
    /// <summary>
    /// El jugador se esta moviendo.
    /// </summary>
    Estrategia, 
    /// <summary>
    /// El jugador esta elegiendo una accion.
    /// </summary>
    Accion, 
    /// <summary>
    /// Se esta ejecutando la accion (animacion)
    /// </summary>
    EjecutandoAccion, 
    /// <summary>
    /// Se estan posicionando los jugadores.
    /// </summary>
    PosicionandoJugadores
}
