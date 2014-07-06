using UnityEngine;
using System.Collections;
using System.IO;
using System;
using Clouds.xml;
using System.Xml;

/// <summary>
/// Encargada de manejar todo lo relacionado con los PersonajesControlables
/// </summary>
public class ControladorJugador : MonoBehaviour 
{

	public	PersonajeControlable trasher;

	public PersonajeControlable Trasher{
        get
        {
            if (trasher == null)
            {
                trasher = GameObject.FindObjectOfType<PersonajeControlable>();
            }
            return trasher;
        }

        set
        {
            if (trasher == null)
            {
                trasher = GameObject.FindObjectOfType<PersonajeControlable>();
            }
        }
	}

	//Instancia de la clase (Singleton)
	[HideInInspector]
	public static ControladorJugador instanceRef;

	void Awake()
	{
        Debug.Log("Segundo");
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

	public bool Cargar_Datos_XML(string personaje)
	{
		string xmlPersonaje = Path.Combine(Application.dataPath,"PlayerData/XML/"+personaje+".xml");
		try
		{
			CloudsXML personajeXML = new CloudsXML ();
			personajeXML.Abrir (xmlPersonaje);
			//Devolvemos la descripcion basica del objeto activo
			XmlNode nodoAuxiliar = personajeXML.DevolverElementos("Parametros")[0];
			//Recorremos todos los nodos colgantes para rescatar sus datos
			foreach (XmlNode nodoSeleccionado in nodoAuxiliar.ChildNodes)
			{
                
				Pasar_Valor(nodoSeleccionado, true);
			}

			personajeXML.Cerrar();
		}
		catch
		{
			return false;
		}
		return true;
	}

	private void Pasar_Valor (XmlNode nodo, bool seleccion)
	{
		//si es true rescatamos valores, si es false guardamos valores
		switch (nodo.Name) 
		{
			case "VIT": if(seleccion)
							trasher.vit.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = trasher.vit.Valor.ToString();
						break;

			case "ESM": if(seleccion)
							trasher.esn.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = trasher.esn.Valor.ToString();
						break;

			case "PM": 	if(seleccion)
							trasher.pm.Valor = int.Parse(nodo.InnerText);
						else
							nodo.InnerText = trasher.pm.Valor.ToString();
						break;

			case "FUE": if(seleccion)
							trasher.fue.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = trasher.fue.Valor.ToString();
						break;

			case "RES": if(seleccion)
                            trasher.res.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = trasher.res.Valor.ToString();
						break;

			case "CON": if(seleccion)
                            trasher.con.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = trasher.con.Valor.ToString();
						break;

			case "ESP": if(seleccion)
                            trasher.esp.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = trasher.esp.Valor.ToString();
						break;

			case "EVA": if(seleccion)
                            trasher.eva.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = trasher.eva.Valor.ToString();
						break;

			case "PNT": if(seleccion)
                            trasher.pnt.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = trasher.pnt.Valor.ToString();
						break;

			case "RAP": if(seleccion)
							trasher.rap.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = trasher.rap.Valor.ToString();
						break;

			case "SUE": if(seleccion)
                            trasher.sue.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = trasher.sue.Valor.ToString();
						break;
		}
	}

	public bool Inicializar_Valores_XML(string personaje)
	{
		string UserPath = Application.persistentDataPath + @"/PlayerData/XML";

		//Creamos los directorios
		if(!Directory.Exists(UserPath))
			Directory.CreateDirectory (UserPath);

		try
		{
			string destino = null;
			TextAsset origen = null;

			//Creacion del xml del personaje
			origen = (TextAsset)Resources.Load("XML/spa/"+personaje, typeof(TextAsset));
			destino = Path.Combine(UserPath,personaje+".xml");
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
			Debug.Log(ex.Message);
			Console.WriteLine(ex.Message);
		}
	}

	public bool Grabar_Datos_XML(string personaje)
	{
		try
		{
			string xmlPersonaje = Path.Combine(Application.persistentDataPath,personaje +".xml");

			//Creamos el objetoXml y cargamos el xml del personaje
			CloudsXML personajeXML = new CloudsXML ();
			personajeXML.Abrir (xmlPersonaje);
				
			//Buscamos sus parametros
			XmlNode nodoAuxiliar = personajeXML.DevolverElementos("Parametros")[0];
				
			//Recorremos todos los nodos colgantes para grabar sus datos
			foreach (XmlNode nodoSeleccionado in nodoAuxiliar.ChildNodes)
			{
				Pasar_Valor(nodoSeleccionado, false);
			}

			//Guardamos el XML
			personajeXML.Grabar();
			//Cerramos el XML
			personajeXML.Cerrar();
		}
		catch
		{
			return false;
		}

		return true;
	}

    public void CambiarAccesorio(Accesorio nuevoAccesorio)
    {
        trasher.Equipamento.CambiarAccesorio(nuevoAccesorio);
    }

    public bool AplicarEstadoAlterado(EstadosAlterados estado)
    {
        switch (estado){
            case EstadosAlterados.Paralizis:
                {
                    break;
                }
        }
        return false;
    }

    public void OrdenarMover(PersonajeControlable personaje, Vector3 pos)
    {
        
    }
}