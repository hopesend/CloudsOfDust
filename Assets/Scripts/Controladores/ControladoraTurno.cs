using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Tiene los metodos necesarios para el funcionamiento del sistema de turnos.
/// </summary>
[System.Serializable]
public class ControladoraTurno {

    /// <summary>
    /// Tiempo necesario para completarse un turno.
    /// </summary>
    float tiempoParaTurno = 100.0f;

    /// <summary>
    /// Lista que contiene todos los turnos con sus respectivos jugadores.
    /// </summary>
    [SerializeField]
    public List<Turno> listaOrdenTurnos = new List<Turno>();

    /// <summary>
    /// Lista tipo FIFO (Primero que entra Primero que sale). 
    /// </summary>
    [SerializeField]
    public Queue<Turno> turnosCompletos = new Queue<Turno>();

    public ControladoraTurno()
    {
    }

    /// <summary>
    /// Crea los turnos en base a las listas de personajes.
    /// </summary>
    /// <param name="listaJugadores">Jugadores Selecionados.</param>
    /// <param name="listaEnemigos">Enemigos.</param>
    public void CargarDatosParaTurnos(List<PersonajeControlable> listaJugadores, List<Enemigo> listaEnemigos)
    {
        foreach (PersonajeControlable a in listaJugadores)
        {
            listaOrdenTurnos.Add(new Turno(a));
        }

        foreach (Enemigo b in listaEnemigos)
        {
            listaOrdenTurnos.Add(new Turno(b));
        }
    }


    public void Update()
    {
        foreach (Turno turno in listaOrdenTurnos)
        {
            if (!turno.listo)
            {
                turno.Update();
                if (turno.TiempoActual >= tiempoParaTurno)
                {
                    turno.Listo();
                    turnosCompletos.Enqueue(turno);
                } 
            }
        }
    }
}
