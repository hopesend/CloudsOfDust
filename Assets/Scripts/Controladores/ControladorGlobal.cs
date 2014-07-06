using UnityEngine;
using System.Collections;
using System.IO;
using System;
using Clouds.xml;
using System.Xml;

/// <summary>
/// Clase puente de controladores, controladora global de acciones del juego
/// </summary>
public class ControladorGlobal : MonoBehaviour 
{
	//Instancia de la clase (Singleton)
	[HideInInspector]
	public static ControladorGlobal instanceRef;

	private ControladorNiveles manager;
	public ControladorNiveles Manager
	{
		get {return manager;}
		set { manager = value;}
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

	public bool Inicializar_Valores_XML()
	{
		string UserPath = Application.persistentDataPath + @"/GlobalData/XML";

		//Creamos los directorios
		if(!Directory.Exists(UserPath))
			Directory.CreateDirectory (UserPath);

		try
		{
			string destino = null;
			TextAsset origen = null;

			//Creacion del xml de los textos
			origen = (TextAsset)Resources.Load("XML/spa/Conversaciones", typeof(TextAsset));
			destino = Path.Combine(UserPath, "Conversaciones.xml");
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

	public void Lanzar_Pantalla(int id)
	{
		switch (id) 
		{
			case 1: ControladorNiveles.instanceRef.CambiarEstado (new EscenarioVecindario(Manager));
					break;
		}
	}
}