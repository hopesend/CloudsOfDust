using System.Collections;

/// <summary>
/// Objeto para albergar la informacion del texto a mostrar en pantalla
/// </summary>
public class Texto
{
	private string id;
	public string Id
	{
		get { return id ; }
		set { id = value; }
	}

	private string receptor;
	public string Receptor
	{
		get { return receptor; }
		set { receptor = value; }
	}

	private string codigoReceptor;
	public string CodigoReceptor
	{
		get { return codigoReceptor; }
		set { codigoReceptor = value; }
	}

	private string textoMostrar;
	public string TextoMostrar
	{
		get { return textoMostrar; }
		set { TextoMostrar = value; }
	}

	public Texto()
	{
	}
}
