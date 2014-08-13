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

    /// <summary>
    /// Carga los datos del jugador buscando en un XML.
    /// </summary>
    /// <param name="jugador">Jugador que le cargare los datos</param>
    /// <returns></returns>
	public bool Cargar_Datos_XML(PersonajeControlable jugador)
	{
		string xmlPersonaje = Path.Combine(Application.dataPath,"PlayerData/XML/Personajes/"+jugador.Get_Nombre()+".xml");
		try
		{
			CloudsXML personajeXML = new CloudsXML ();
			personajeXML.Abrir (xmlPersonaje);
			//Devolvemos la descripcion basica del objeto activo
			XmlNode nodoAuxiliar = personajeXML.DevolverElementos("Parametros")[0];
			//Recorremos todos los nodos colgantes para rescatar sus datos
			foreach (XmlNode nodoSeleccionado in nodoAuxiliar.ChildNodes)
			{
                
				Pasar_Valor_Stats(jugador, nodoSeleccionado, true);
			}

            foreach (XmlNode tempHabilidades in personajeXML.DevolverElementos("Habilidades"))
            {
                foreach (XmlNode tempTipo in tempHabilidades.ChildNodes)
                {
                    foreach (XmlNode tempHabilidad in tempTipo)
                    {
                        CrearHabilidadDesdeXml(jugador, tempHabilidad, (TipoHabilidad) Enum.Parse(typeof(TipoHabilidad), tempTipo.Name));
                    }
                }
            }
			personajeXML.Cerrar();
		}
		catch(Exception e)
		{
            Debug.Log(e.InnerException.Message);
			return false;
		}
		return true;
	}

    private void CrearHabilidadDesdeXml(PersonajeControlable jugador, XmlNode nodo, TipoHabilidad tipo)
    {
        Habilidad nueva = new Habilidad();
        nueva.Nombre = nodo.Attributes[0].Value;
        nueva.Descripcion = nodo.Attributes[1].Value;
        nueva.CostoPM = Convert.ToSingle(nodo.Attributes[2].Value);
        nueva.CostoPA = Convert.ToSingle(nodo.Attributes[3].Value);
        nueva.CostoEst = Convert.ToSingle(nodo.Attributes[4].Value);
        nueva.Rango = Convert.ToSingle(nodo.Attributes[5].Value);
        nueva.Probabilidad = Convert.ToSingle(nodo.Attributes[6].Value);

        if (nodo.ChildNodes.Count > 0)
        {
            foreach (XmlNode nodoEfecto in nodo.ChildNodes)
            {
                EfectosHabilidad nuevoEfecto = new EfectosHabilidad();
                nuevoEfecto.TipoEfecto = (TipoEfecto) Enum.Parse(typeof(TipoEfecto), nodoEfecto.Attributes[0].Value);
                nuevoEfecto.Duracion = Convert.ToInt32(nodoEfecto.Attributes[1].Value);
                nuevoEfecto.TipoObjEfecto = (TipoObjetivoEfecto)Enum.Parse(typeof(TipoObjetivoEfecto), nodoEfecto.Attributes[2].Value);
                nuevoEfecto.StatAfectado = (NombreAtributo)Enum.Parse(typeof(NombreAtributo), nodoEfecto.Attributes[3].Value);
                nuevoEfecto.CantidadAfectada = Convert.ToSingle(nodoEfecto.Attributes[4].Value);
                nuevoEfecto.Modificador = (TipoModificador)Enum.Parse(typeof(TipoModificador), nodoEfecto.Attributes[5].Value);
                nuevoEfecto.EstadoAlterado = (EstadosAlterados)Enum.Parse(typeof(EstadosAlterados), nodoEfecto.Attributes[6].Value);

                nueva.AgregarEfecto(nuevoEfecto);
            }
        }
        string[] a = {"A1"};
        nueva.ComboHabilidad = new Combo(a, 20);

        jugador.AgregarHabilidad(nueva);
    }

    /// <summary>
    /// Obtener/Guardar valores del XMl de Stats.
    /// </summary>
    /// <param name="jugador">Jugador que estoy manejando</param>
    /// <param name="nodo"></param>
    /// <param name="seleccion">Obtener = true / Guardar = false</param>
	private void Pasar_Valor_Stats (PersonajeControlable jugador, XmlNode nodo, bool seleccion)
	{
		//si es true rescatamos valores, si es false guardamos valores
		switch (nodo.Name) 
		{
			case "VIT":
                {
                    if (seleccion)
                    {
                        jugador.Vitalidad.ValorBase = int.Parse(nodo.Attributes[0].Value);
                        jugador.Vitalidad.ValorMaximo = int.Parse(nodo.Attributes[1].Value);
                    }
                    else
                    {
                        nodo.Attributes[0].Value = jugador.Vitalidad.ValorBase.ToString();
                    }    
					break;
                }

            case "EST":
                {
                    if (seleccion)
                    {
                        jugador.Estamina.ValorBase = int.Parse(nodo.Attributes[0].Value);
                        jugador.Estamina.ValorMaximo = int.Parse(nodo.Attributes[1].Value);
                    }
                    else
                    {
                        nodo.Attributes[0].Value = jugador.Estamina.ValorBase.ToString();
                    }
                    break;
                }

            case "PM":
                {
                    if (seleccion)
                    {
                        jugador.PM.ValorBase = int.Parse(nodo.Attributes[0].Value);
                        jugador.PM.ValorMaximo = int.Parse(nodo.Attributes[1].Value);
                    }
                    else
                    {
                        nodo.Attributes[0].Value = jugador.PM.ValorBase.ToString();
                    }
                    break;
                }

            case "FUE":
                {
                    if (seleccion)
                    {
                        jugador.Fuerza.ValorBase = int.Parse(nodo.Attributes[0].Value);
                        jugador.Fuerza.ValorMaximo = int.Parse(nodo.Attributes[1].Value);
                    }
                    else
                    {
                        nodo.Attributes[0].Value = jugador.Fuerza.ValorBase.ToString();
                    }
                    break;
                }

            case "RES":
                {
                    if (seleccion)
                    {
                        jugador.Resistencia.ValorBase = int.Parse(nodo.Attributes[0].Value);
                        jugador.Resistencia.ValorMaximo = int.Parse(nodo.Attributes[1].Value);
                    }
                    else
                    {
                        nodo.Attributes[0].Value = jugador.Resistencia.ValorBase.ToString();
                    }
                    break;
                }

            case "CON":
                {
                    if (seleccion)
                    {
                        jugador.Concentracion.ValorBase = int.Parse(nodo.Attributes[0].Value);
                        jugador.Concentracion.ValorMaximo = int.Parse(nodo.Attributes[1].Value);
                    }
                    else
                    {
                        nodo.Attributes[0].Value = jugador.Concentracion.ValorBase.ToString();
                    }
                    break;
                }

            case "ESP":
                {
                    if (seleccion)
                    {
                        jugador.Espiritu.ValorBase = int.Parse(nodo.Attributes[0].Value);
                        jugador.Espiritu.ValorMaximo = int.Parse(nodo.Attributes[1].Value);
                    }
                    else
                    {
                        nodo.Attributes[0].Value = jugador.Espiritu.ValorBase.ToString();
                    }
                    break;
                }

            case "EVA":
                {
                    if (seleccion)
                    {
                        jugador.Evasion.ValorBase = int.Parse(nodo.Attributes[0].Value);
                        jugador.Evasion.ValorMaximo = int.Parse(nodo.Attributes[1].Value);
                    }
                    else
                    {
                        nodo.Attributes[0].Value = jugador.Evasion.ValorBase.ToString();
                    }
                    break;
                }

            case "PNT":
                {
                    if (seleccion)
                    {
                        jugador.Punteria.ValorBase = int.Parse(nodo.Attributes[0].Value);
                        jugador.Punteria.ValorMaximo = int.Parse(nodo.Attributes[1].Value);
                    }
                    else
                    {
                        nodo.Attributes[0].Value = jugador.Punteria.ValorBase.ToString();
                    }
                    break;
                }

            case "RAP":
                {
                    if (seleccion)
                    {
                        jugador.Rapidez.ValorBase = int.Parse(nodo.Attributes[0].Value);
                        jugador.Rapidez.ValorMaximo = int.Parse(nodo.Attributes[1].Value);
                    }
                    else
                    {
                        nodo.Attributes[0].Value = jugador.Rapidez.ValorBase.ToString();
                    }
                    break;
                }
            case "MOV":
                {
                    if (seleccion)
                    {
                        jugador.Movimiento.ValorBase = int.Parse(nodo.Attributes[0].Value);
                        jugador.Movimiento.ValorMaximo = int.Parse(nodo.Attributes[1].Value);
                    }
                    else
                    {
                        nodo.Attributes[0].Value = jugador.Movimiento.ValorBase.ToString();
                    }
                    break;
                }

            case "SUE":
                {
                    if (seleccion)
                    {
                        jugador.Suerte.ValorBase = int.Parse(nodo.Attributes[0].Value);
                        jugador.Suerte.ValorMaximo = int.Parse(nodo.Attributes[1].Value);
                    }
                    else
                    {
                        nodo.Attributes[0].Value = jugador.Suerte.ValorBase.ToString();
                    }
                    break;
                }
		}
	}
    [Obsolete("En realidad los XML ya estan creados")]
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
				Pasar_Valor_Stats(data.GO, nodoSeleccionado, false);
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
}