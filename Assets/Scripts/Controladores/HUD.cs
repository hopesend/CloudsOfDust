using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HUD 
{
	//Variables Mensaje Interaccion
	[HideInInspector]
	public bool mostrarInteraccion = false;
	[HideInInspector]
	public string cabecera;
	[HideInInspector]
	public List<Texto> cuerpo;
	[HideInInspector]
	public int ventanaIDTextos = 1;
	private Vector2 posicionBarraScrollTextos;
	//--------------

	//Variables Boton Mensaje Nuevo HAPQ
	[HideInInspector]
	public bool mostrarMensajeHAPQ = false;
	[HideInInspector]
	public string idMensajeHAPQ;
	public string ObjetoMensajeHAPQ;
	//--------------

	
    public static HUD instanceRef;

    public static HUD InstanceRef()
    {
        if (instanceRef == null)
        {
            instanceRef = new HUD();
        }

        return instanceRef;
    }
	
	// Use this for initialization
	public HUD() 
	{
	}

    public void Update()
	{
    }

	public void OnGUI()
	{
		if (mostrarInteraccion)
		{
			mostrarHudInteraccion ();
		} 

		if (mostrarMensajeHAPQ) 
		{
			mostrarHudMensajeHAPQ();
		}
	}

	private void mostrarHudMensajeHAPQ()
	{
		if (GUILayout.Button ("Nuevo Mensaje HAPQ")) 
		{
			mostrarMensajeHAPQ = false;
		}
	}

	private void mostrarHudInteraccion()
	{
		GUILayout.Window (ventanaIDTextos, new Rect(0, Screen.height-(Screen.height/5), Screen.width, Screen.height), Creacion_Ventana_Textos, cabecera);
	}

	private void Creacion_Ventana_Textos(int Id) 
	{
		switch(Id)
		{
		case 1:	
			VentanaDescripciones();
			break;
		case 2:	
			VentanaConversaciones();
			break;
		}
	}

	public void VentanaDescripciones()
	{
		posicionBarraScrollTextos = GUILayout.BeginScrollView (posicionBarraScrollTextos);
			GUILayout.BeginVertical ();
				foreach (Texto nuevoTexto in cuerpo) 
				{
					GUILayout.Label(nuevoTexto.TextoMostrar);
				}
			GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}

	public void VentanaConversaciones()
	{
		posicionBarraScrollTextos = GUILayout.BeginScrollView (posicionBarraScrollTextos);
			GUILayout.BeginVertical ();
				foreach (Texto nuevoTexto in cuerpo) 
				{
					/*if (GUILayout.Button(textoBoton1))
					{
						Insertar_Label_Tabla(true, textoBoton1, Color.green);
						posicionBarraScrollObjeto.y = Mathf.Infinity;
						Limpiar_Datos();
						Iniciar_Conversacion(numeroPregunta1.ToString(), ControladorEstadoJugador.instanceRef.objetoPulsado.tag.ToString ());
					}*/
				}
			GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}

	public void InsertarMensajeHAPQ(string id, string objeto)
	{
		idMensajeHAPQ = id;
		ObjetoMensajeHAPQ = objeto;

		Informacion nuevaInformacion = new Informacion();
		foreach (Texto nuevoTexto in nuevaInformacion.Devolver_Texto_HAPQ (id, objeto)) 
		{
			nuevaInformacion.Insertar_Mensaje_HAPQ(nuevoTexto.Receptor, nuevoTexto.TextoMostrar);
		}

		HUD.instanceRef.mostrarMensajeHAPQ = true;
	}
}

