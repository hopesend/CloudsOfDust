using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;

/// <summary>
/// Objeto para la devolucion del texto a mostrar en pantalla
/// </summary>
public class Informacion
{
	private string xmlDescripcion = Path.Combine(Application.persistentDataPath,"/GlobalData/XML/Conversaciones.xml");

	public Informacion()
	{
	}
}
