using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Clouds.xml;
using UnityEngine;

/// <summary>
/// Objeto para la devolucion del texto a mostrar en pantalla
/// </summary>
public class Informacion
{
	/// <summary>
	/// Variable con la Ruta del xml de Textos
	/// </summary>
	private string rutaConversaciones = Application.persistentDataPath+"/GlobalData/XML/Conversaciones.xml";

	/// <summary>
	/// Constructor de la Clase
	/// </summary>
	public Informacion()
	{
	}

	/// <summary>
	/// Metodo para la Devolucion de Texto para Conversaciones o Texto de personajes aleatorios
	/// </summary>
	/// <param name="id">
	/// el valor del id del texto que lanza ese personaje
	/// </param>
	/// <param name="personaje">
	/// el nombre del personaje que lanza un texto
	/// </param>
	/// <return>
	/// Devuelve una lista de Objetos tipo Texto
	/// </return>
	public List<Texto> Devolver_Texto(string id, string personaje)
	{
		List<Texto> listaAuxiliar = new List<Texto> ();

		if (File.Exists (rutaConversaciones)) 
		{
			CloudsXML archivoConversaciones = new CloudsXML ();
			archivoConversaciones.Abrir (rutaConversaciones);

			XmlNode nodoPersonaje = archivoConversaciones.DevolverElementos("Conversaciones/"+personaje)[0];
			foreach(XmlNode nodoHijo in nodoPersonaje.ChildNodes)
			{
				if(nodoHijo.Attributes["id"].Value.ToString().Contains(id))
				{
					listaAuxiliar.Add(new Texto(nodoHijo.Attributes["id"].Value.ToString(), nodoHijo.Attributes["receptor"].Value.ToString(), nodoHijo.Attributes["codigoRecepcion"].Value.ToString(), nodoHijo.InnerText));
				}
			}

			archivoConversaciones.Cerrar ();
		} 

		return listaAuxiliar;
	}

	/// <summary>
	/// Metodo para la Devolucion de Texto para HAPQ
	/// </summary>
	/// <param name="id">
	/// el valor del id del texto que lanza ese personaje
	/// </param>
	/// <param name="personaje">
	/// el nombre del personaje que lanza un texto
	/// </param>
	/// <return>
	/// Devuelve una lista de Objetos tipo Texto
	/// </return>
	public List<Texto> Devolver_Texto_HAPQ(string id, string personaje)
	{
		List<Texto> listaAuxiliar = new List<Texto> ();
		
		if (File.Exists (rutaConversaciones)) 
		{
			CloudsXML archivoConversaciones = new CloudsXML ();
			archivoConversaciones.Abrir (rutaConversaciones);
			
			XmlNode nodoPersonaje = archivoConversaciones.DevolverElementos("Conversaciones/HAPQ")[0];
			foreach(XmlNode nodoHijo in nodoPersonaje.ChildNodes)
			{
				if(nodoHijo.Attributes["personaje"].Value.ToString().Contains(personaje))
				{
					if(nodoHijo.Attributes["id"].Value.ToString().Contains(id))
					{
						listaAuxiliar.Add(new Texto(nodoHijo.Attributes["personaje"].Value.ToString(), nodoHijo.InnerText));
					}
				}
			}
			
			archivoConversaciones.Cerrar ();
		} 
		
		return listaAuxiliar;
	}

	/// <summary>
	/// Metodo para la Devolucion de Texto para Pantalla de ayuda del juego
	/// </summary>
	/// <param name="id">
	/// el valor del id del texto que lanza ese personaje
	/// </param>
	/// <param name="personaje">
	/// el nombre del personaje que lanza un texto
	/// </param>
	/// <return>
	/// Devuelve una lista de Objetos tipo Texto
	/// </return>
	public List<Texto> Devolver_Texto_Pantallas(string id, string objeto)
	{
		List<Texto> listaAuxiliar = new List<Texto> ();
		
		if (File.Exists (rutaConversaciones)) 
		{
			CloudsXML archivoConversaciones = new CloudsXML ();
			archivoConversaciones.Abrir (rutaConversaciones);
			
			XmlNode nodoPersonaje = archivoConversaciones.DevolverElementos("Conversaciones/Pantallas")[0];
			foreach(XmlNode nodoHijo in nodoPersonaje.ChildNodes)
			{
				if(nodoHijo.Attributes["objeto"].Value.ToString().Contains(objeto))
				{
					if(nodoHijo.Attributes["id"].Value.ToString().Contains(id))
					{
						listaAuxiliar.Add(new Texto(nodoHijo.Attributes["objeto"].Value.ToString(), nodoHijo.InnerText));
					}
				}
			}
			
			archivoConversaciones.Cerrar ();
		} 
		
		return listaAuxiliar;
	}
}
