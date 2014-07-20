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
	/// Variable con la Ruta del xml del HAPQ
	/// </summary>
	private string rutaHAPQ = Application.persistentDataPath+"/GlobalData/XML/HAPQ.xml";

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
	/// el Nombre del personaje que lanza un texto
	/// </param>
	/// <return>
	/// Devuelve una lista de Objetos Tipo Texto
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
	/// el Nombre del personaje que lanza un texto
	/// </param>
	/// <return>
	/// Devuelve una lista de Objetos Tipo Texto
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
	/// el Nombre del personaje que lanza un texto
	/// </param>
	/// <return>
	/// Devuelve una lista de Objetos Tipo Texto
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

	/// <summary>
	/// Metodo para la Devolucion de la lista de Mensajes guardados en xml de HAPQ
	/// </summary>
	/// <return>
	/// Devuelve una lista de Objetos Tipo Texto
	/// </return>
	public List<Texto> Devolver_Lista_Mensajes_HAPQ()
	{
		List<Texto> listaAuxiliar = new List<Texto> ();
		
		if (File.Exists (rutaConversaciones)) 
		{
			CloudsXML archivoHAPQ = new CloudsXML ();
			archivoHAPQ.Abrir (rutaHAPQ);
			
			XmlNode nodoPersonaje = archivoHAPQ.DevolverElementos("Mensajes")[0];
			foreach(XmlNode nodoHijo in nodoPersonaje.ChildNodes)
			{
				listaAuxiliar.Add(new Texto(nodoHijo.Attributes["personaje"].Value.ToString(), nodoHijo.InnerText));
			}
			
			archivoHAPQ.Cerrar ();
		} 
		
		return listaAuxiliar;
	}

	/// <summary>
	/// Metodo para la insercion de nuevo mensaje en la lista de mensajes leidos y por leer de la HAPQ
	/// </summary>
	/// <param name="personaje">
	/// el nombre del personaje u objeto que lanza el mensaje
	/// </param>
	/// <param name="texto">
	/// el texto que que a lanzado el personaje u objeto
	/// </param>
	public void Insertar_Mensaje_HAPQ(string personaje, string texto)
	{
		if (File.Exists (rutaHAPQ)) 
		{
			CloudsXML archivoHAPQ = new CloudsXML ();
			archivoHAPQ.Abrir (rutaHAPQ);

			XmlNode nodoAuxiliar = archivoHAPQ.CrearElemento("Mensajes", "Mensaje", texto, new string[]{"personaje:"+personaje});

			archivoHAPQ.Grabar();
			
			archivoHAPQ.Cerrar ();
		} 
	}
}
