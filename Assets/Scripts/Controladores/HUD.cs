using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HUD {
	
	

	//Variables Texto
	public bool mostrarInteraccion = false;
	public string cabecera;
	public List<Texto> cuerpo;
	public int ventanaIDTextos = 1;
	private Vector2 posicionBarraScrollTextos;
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
	public HUD() {
	}

    public void Update(){
    }

    
	
	// Update is called once per frame
	
	
	public void OnGUI()
	{
		//Todo lo que vaya en la GUI, hay que insertarla en metodos...
    	if (mostrarInteraccion) 
		{
			mostrarHudInteraccion();
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
			//VentanaConversaciones();
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

	
}

