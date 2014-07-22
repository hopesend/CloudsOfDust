using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ControladorBaseMundo
{
	//Instancia de la clase (Singleton)
	[HideInInspector]
	private static ControladorBaseMundo instanceRef;

    

	//Variables publicas o privadas
	public float dinero;
	public int escenaActual;
	public Vector3[] entrada;

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

    public void PrepararJugadorScripts()
    {
        personajesSeleccionados[0].GetComponent<ThirdPersonController>().enabled = true;
        personajesSeleccionados[0].GetComponent<HAPQ>().enabled = true;
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

