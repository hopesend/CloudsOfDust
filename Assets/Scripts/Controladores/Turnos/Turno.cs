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
            tiempoActual += personaje.rap.Valor * Time.deltaTime; 
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
        personaje.comportamientoActual = ComportamientoJugador.EsperandoComportamiento;
    }
}
