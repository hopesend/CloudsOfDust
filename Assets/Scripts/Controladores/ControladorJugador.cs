using UnityEngine;
using System.Collections;
using System.IO;
using System;
using Clouds.xml;
using System.Xml;

/// <summary>
/// Encargada de manejar todo lo relacionado con los PersonajesControlables
/// </summary>
/// 
public class ControladorJugador
{

	//Instancia de la clase (Singleton)
	[HideInInspector]
	private static ControladorJugador instanceRef;

	public static ControladorJugador InstanceRef()
	{
		if(instanceRef == null)
		{
            instanceRef = new ControladorJugador();
		}

        return instanceRef;
		
	}

	public bool Cargar_Datos_XML(PersonajeControlable jugador)
	{
		string xmlPersonaje = Path.Combine(Application.dataPath,"PlayerData/XML/"+jugador.Get_Nombre()+".xml");
		try
		{
			CloudsXML personajeXML = new CloudsXML ();
			personajeXML.Abrir (xmlPersonaje);
			//Devolvemos la descripcion basica del objeto activo
			XmlNode nodoAuxiliar = personajeXML.DevolverElementos("Parametros")[0];
			//Recorremos todos los nodos colgantes para rescatar sus datos
			foreach (XmlNode nodoSeleccionado in nodoAuxiliar.ChildNodes)
			{
                
				Pasar_Valor(jugador, nodoSeleccionado, true);
			}

			personajeXML.Cerrar();
		}
		catch
		{
			return false;
		}
		return true;
	}

	private void Pasar_Valor (PersonajeControlable jugador, XmlNode nodo, bool seleccion)
	{
		//si es true rescatamos valores, si es false guardamos valores
		switch (nodo.Name) 
		{
			case "VIT": if(seleccion)
							jugador.vit.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = jugador.vit.Valor.ToString();
						break;

			case "ESM": if(seleccion)
							jugador.esn.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = jugador.esn.Valor.ToString();
						break;

			case "PM": 	if(seleccion)
							jugador.pm.Valor = int.Parse(nodo.InnerText);
						else
							nodo.InnerText = jugador.pm.Valor.ToString();
						break;

			case "FUE": if(seleccion)
							jugador.fue.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = jugador.fue.Valor.ToString();
						break;

			case "RES": if(seleccion)
                            jugador.res.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = jugador.res.Valor.ToString();
						break;

			case "CON": if(seleccion)
                            jugador.con.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = jugador.con.Valor.ToString();
						break;

			case "ESP": if(seleccion)
                            jugador.esp.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = jugador.esp.Valor.ToString();
						break;

			case "EVA": if(seleccion)
                            jugador.eva.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = jugador.eva.Valor.ToString();
						break;

			case "PNT": if(seleccion)
                            jugador.pnt.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = jugador.pnt.Valor.ToString();
						break;

			case "RAP": if(seleccion)
							jugador.rap.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = jugador.rap.Valor.ToString();
						break;

			case "SUE": if(seleccion)
                            jugador.sue.Valor = int.Parse(nodo.InnerText);
						else
                            nodo.InnerText = jugador.sue.Valor.ToString();
						break;
		}
	}

	public bool Inicializar_Valores_XML(DataDBPersonaje data)
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
			origen = (TextAsset)Resources.Load("XML/spa/"+data.ID.ToString(), typeof(TextAsset));
            destino = Path.Combine(UserPath, data.ID.ToString() + ".xml");
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

	public bool Grabar_Datos_XML(DataDBPersonaje data)
	{
		try
		{
			string xmlPersonaje = Path.Combine(Application.persistentDataPath,data.ID.ToString() +".xml");

			//Creamos el objetoXml y cargamos el xml del personaje
			CloudsXML personajeXML = new CloudsXML ();
			personajeXML.Abrir (xmlPersonaje);
				
			//Buscamos sus parametros
			XmlNode nodoAuxiliar = personajeXML.DevolverElementos("Parametros")[0];
				
			//Recorremos todos los nodos colgantes para grabar sus datos
			foreach (XmlNode nodoSeleccionado in nodoAuxiliar.ChildNodes)
			{
				Pasar_Valor(data.GO, nodoSeleccionado, false);
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

    public void CambiarAccesorio(PersonajeControlable personaje, Accesorio nuevoAccesorio)
    {
        personaje.Equipamento.CambiarAccesorio(nuevoAccesorio);
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