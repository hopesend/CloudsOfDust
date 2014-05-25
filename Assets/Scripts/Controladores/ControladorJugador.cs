using UnityEngine;
using System.Collections;
using System.IO;
using System;
using Clouds.xml;
using System.Xml;

public class ControladorJugador : MonoBehaviour 
{
	//Instancia de la clase (Singleton)
	[HideInInspector]
	public static ControladorJugador instanceRef;
	
	//Variables publicas o privadas
	private int vitalidad;
	public int Vitalidad
	{
		get{ return vitalidad;  }
		set{ vitalidad = value; }
	}

	private int estamina;
	public int Estamina
	{
		get{ return estamina;  }
		set{ estamina = value; }
	}

	private int puntosMagicos;
	public int PuntosMagicos
	{
		get{ return puntosMagicos;  }
		set{ puntosMagicos = value; }
	}

	private int fuerza;
	public int Fuerza
	{
		get{ return fuerza;  }
		set{ fuerza = value; }
	}

	private int resistencia;
	public int Resistencia
	{
		get{ return resistencia;  }
		set{ resistencia = value; }
	}

	private int concentracion;
	public int Concentracion
	{
		get{ return concentracion;  }
		set{ concentracion = value; }
	}

	private int espiritu;
	public int Espiritu
	{
		get{ return espiritu;  }
		set{ espiritu = value; }
	}

	private int evasion;
	public int Evasion
	{
		get{ return evasion;  }
		set{ evasion = value; }
	}

	private int punteria;
	public int Punteria
	{
		get{ return punteria;  }
		set{ punteria = value; }
	}

	private int rapidez;
	public int Rapidez
	{
		get{ return rapidez;  }
		set{ rapidez = value; }
	}

	private int suerte;
	public int Suerte
	{
		get{ return suerte;  }
		set{ suerte = value; }
	}

	void Awake()
	{
		if(instanceRef == null)
		{
			instanceRef = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			DestroyImmediate(gameObject);
		}
		
	}

	public bool Cargar_Datos_XML()
	{
		string xmlPersonaje = Path.Combine(Application.persistentDataPath,"Personaje.xml");
		try
		{
			CloudsXML personajeXML = new CloudsXML ();
			personajeXML.Abrir (xmlPersonaje);

			//Devolvemos la descripcion basica del objeto activo
			XmlNode nodoAuxiliar = personajeXML.DevolverElementos("Parametros")[0];

			foreach (XmlNode nodoSeleccionado in nodoAuxiliar.ChildNodes)
			{
				Pasar_Valor(nodoSeleccionado);
			}
		}
		catch
		{
			return false;
		}
		return true;
	}

	private void Pasar_Valor (XmlNode nodo)
	{
		switch (nodo.Name) 
		{
			case "VIT": Vitalidad = int.Parse(nodo.InnerText);
						break;
			case "ESM": Estamina = int.Parse(nodo.InnerText);
						break;
			case "PM": 	PuntosMagicos = int.Parse(nodo.InnerText);
						break;
			case "FUE": Fuerza = int.Parse(nodo.InnerText);
						break;
			case "RES": Resistencia = int.Parse(nodo.InnerText);
						break;
			case "CON": Concentracion = int.Parse(nodo.InnerText);
						break;
			case "ESP": Espiritu = int.Parse(nodo.InnerText);
						break;
			case "EVA": Evasion = int.Parse(nodo.InnerText);
						break;
			case "PNT": Punteria = int.Parse(nodo.InnerText);
						break;
			case "RAP": Rapidez = int.Parse(nodo.InnerText);
						break;
			case "SUE": Suerte = int.Parse(nodo.InnerText);
						break;
		}
	}

	public bool Inicializar_Valores_XML()
	{
		string UserPath = Application.persistentDataPath;

		try
		{
			string destino = null;
			TextAsset origen = null;
			
			//Creacion del xml de las descripciones
			origen = (TextAsset)Resources.Load("XML/Personaje", typeof(TextAsset));
			destino = Path.Combine(UserPath, "Personaje.xml");
			if (!File.Exists (destino))
				Crear_Fichero (origen, destino);
		}
		catch
		{
			return false;
		}

		return true;
	}

	private void Crear_Fichero(TextAsset nuevoOrigen, string nuevoDestino)
	{
		try 
		{
			StreamWriter sw = new StreamWriter(nuevoDestino, false);
			sw.Write(nuevoOrigen.text);
			sw.Close();
		} 
			catch (IOException ex) {
			Console.WriteLine(ex.Message);
		}
	}

}