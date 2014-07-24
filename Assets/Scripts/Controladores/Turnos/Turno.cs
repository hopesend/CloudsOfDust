using UnityEngine;
using System.Collections;

[System.Serializable]
public class Turno {

    public PersonajeBase personaje;

    public PersonajeBase Personaje
    {
        get { return personaje; }
    }
    public float tiempoActual = 0;

   public bool listo = false;

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
