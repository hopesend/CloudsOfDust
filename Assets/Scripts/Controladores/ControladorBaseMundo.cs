using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controla todos los aspectos del Mundo.
/// </summary>
[System.Serializable]
public class ControladorBaseMundo
{
	//Instancia de la clase (Singleton)
	[HideInInspector]
	private static ControladorBaseMundo instanceRef;

    

	//Variables publicas o privadas

    /// <summary>
    /// Dinero actual del Jugador.
    /// </summary>
	public float dinero;

    /// <summary>
    /// Cantidad maxima de jugadores seleccioandos.
    /// </summary>
    int cantidadJugadoresSeleccionados = 3;

    /// <summary>
    /// Contiene los personajes seleccionados para la pelea
    /// </summary>
    public List<PersonajeControlable> personajesSeleccionados;

    public static ControladorBaseMundo InstanceRef()
	{
		if(instanceRef == null)
		{
            instanceRef = new ControladorBaseMundo();
		}

        return instanceRef;

	}
	
	private ControladorBaseMundo()
	{
        personajesSeleccionados = new List<PersonajeControlable>(cantidadJugadoresSeleccionados);
	}

    /// <summary>
    /// Agrega un nuevo jugador a los Personajes Seleccionados
    /// </summary>
    /// <param name="nuevo">El nuevo jugador</param>
    /// <param name="isWorldPlayer">Es el que aparece en el Mundo ??</param>
    public void AddPersonajeSeleccionado(PersonajeControlable nuevo, bool isWorldPlayer)
    {
        if (nuevo != null)
        {
            if(!personajesSeleccionados.Contains(nuevo)){
                if (personajesSeleccionados.Count < cantidadJugadoresSeleccionados)
                {
                    if (isWorldPlayer)
                    {
                        personajesSeleccionados.Insert(0, nuevo);
                    }
                    else
                    {
                        //Lo deshabilito para que no se vea en el World;
                        nuevo.enabled = false;

                        personajesSeleccionados.Add(nuevo);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Remueve un jugador y agrega otro nuevo
    /// </summary>
    /// <param name="nuevo">Jugador nuevo</param>
    /// <param name="aRemover">El Jugador que tengo que remover.</param>
    /// <param name="isWorldPlayer">Es el que aparece en el Mundo ??</param>
    public void CambiarPersonajeSeleccionado(PersonajeControlable nuevo, PersonajeControlable aRemover, bool isWorldPlayer)
    {
        if (personajesSeleccionados.Remove(aRemover))
        {
            AddPersonajeSeleccionado(nuevo, isWorldPlayer);

            
            if (personajesSeleccionados.Count>1)
            {
                //Deshabilito los otros personajes
                for (int i = 1; i < personajesSeleccionados.Count; i++)
                {
                    personajesSeleccionados[i].enabled = false;
                } 
            }
        }
    }

    /// <summary>
    /// Prepara los GO de la lista de Jugadores, para que funcionen correctamente en la scene de batalla
    /// </summary>
    public void PrepararJugadorScripts()
    {
        //personajesSeleccionados[0].GetComponent<HAQPHUD2>().enabled = true;
        personajesSeleccionados[0].GetComponent<Historia>().enabled = true;
    }

    /// <summary>
    /// Cambia la posicion del personaje que se muestra en el juego
    /// </summary>
    public Vector3 PosicionPersonajeWorld
    {
        get { return personajesSeleccionados[0].transform.position; }
        set 
        { 
            Vector3 temp;
            temp = value;
            personajesSeleccionados[0].transform.position = temp;
        }
    }
}

