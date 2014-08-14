using UnityEngine;
using System.Collections;

/// <summary>
/// Una instancia de turno.
/// </summary>
[System.Serializable]
public class Turno {

    /// <summary>
    /// Personaje dueño del turno.
    /// </summary>
    public PersonajeBase personaje;

    public PersonajeBase Personaje
    {
        get { return personaje; }
    }
    /// <summary>
    /// Tiempo que lleva el turno cargandose.
    /// </summary>
    public float tiempoActual = 0;

    public bool seMovio;

    public bool usoHabilidad;

    /// <summary>
    /// Esta listo el turno ??
    /// </summary>
   public bool listo = false;

    /// <summary>
    /// Listo el turno !!
    /// </summary>
    public void Listo()
    {
        listo = true;

        personaje.VerificarAtributosAlterados();
        personaje.RestarTurno();
        
    }


    public float TiempoActual
    {
        get { return tiempoActual; }
    }

    /// <summary>
    /// Constructor obligatorio. 
    /// </summary>
    /// <param name="_personaje">El dueño del turno</param>
    public Turno(PersonajeBase _personaje)
    {
        personaje = _personaje;
    }

    public void Update()
    {
        if (!listo)
        {
            tiempoActual += personaje.Rapidez.ValorActual * Time.deltaTime; 
        }
    }

    /// <summary>
    /// Resta tiempo del turno, al usar una habilidad.
    /// </summary>
    /// <param name="tiempoRestar">El tiempo que hay que restar.</param>
    public void UsarHabilidad(float tiempoRestar)
    {
        if ((tiempoActual - tiempoRestar) <= 0)
        {
            tiempoActual = 0;
        }
        else
        {
            tiempoActual -= tiempoRestar;
        }

        listo = false;
        //personaje.comportamientoActual = ComportamientoJugador.EsperandoComportamiento;
    }
}
