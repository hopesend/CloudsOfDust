using System.Collections;

/// <summary>
/// Objeto para albergar la informacion del texto a mostrar en pantalla
/// </summary>
public class Texto
{
	/// <summary>
	/// Variable que albergara el id buscado
	/// </summary>
	private string id;
	public string Id
	{
		get { return id ; }
		set { id = value; }
	}

	/// <summary>
	/// Variable que albergara el nombre del receptor del texto
	/// </summary>
	private string receptor;
	public string Receptor
	{
		get { return receptor; }
		set { receptor = value; }
	}

	/// <summary>
	/// Variable que albergara el codigo del texto que el receptor tiene que responder
	/// </summary>
	private string codigoReceptor;
	public string CodigoReceptor
	{
		get { return codigoReceptor; }
		set { codigoReceptor = value; }
	}

	/// <summary>
	/// Variable que albergara el texto a mostrar
	/// </summary>
	private string textoMostrar;
	public string TextoMostrar
	{
		get { return textoMostrar; }
		set { TextoMostrar = value; }
	}

	/// <summary>
	/// Constructor de la clase para Conversaciones
	/// </summary>
	/// <param name="nuevoId">
	/// el valor del id que se esta buscando
	/// </param>
	/// <param name="nuevoReceptor">
	/// el nombre el nuevo Receptor de la respuesta a la pregunta
	/// </param>
	/// <param name="nuevoCodigo">
	/// el valor de id de la nueva Pregunta que va asociada al nuevoReceptor
	/// </param>
	/// <param name="nuevoTexto">
	/// El texto a mostrar
	/// </param>
	public Texto(string nuevoId, string nuevoReceptor, string nuevoCodigo, string nuevoTexto)
	{
		Id = nuevoId;
		Receptor = nuevoReceptor;
		CodigoReceptor = nuevoCodigo;
		TextoMostrar = nuevoTexto;
	}

	/// <summary>
	/// Constructor sobrecargado de la clase para textos en HAPQ y/o Pantallas de Ayuda
	/// </summary>
	/// <param name="personaje">
	/// el nombre de personaje u objeto que lanza el texto a pantalla o HAPQ
	/// </param>
	/// <param name="nuevoTexto">
	/// el texto a mostrar
	/// </param>
	public Texto(string personaje, string nuevoTexto)
	{
		Receptor = personaje;
		TextoMostrar = nuevoTexto;
	}
}
