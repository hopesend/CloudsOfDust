﻿using UnityEngine;
using System.IO;

public class EscenarioCasaInterior: IControlador
{
	private ControladorNiveles manager;
	
	public EscenarioCasaInterior(ControladorNiveles managerRef)
	{
		manager = managerRef;
		ControladorGlobal.instanceRef.Manager = managerRef;
		
		if(Application.loadedLevelName != "Casa interior")
		{
			Application.LoadLevel("Casa interior");
		}
	}

    public void CargarDatosPlayer()
    {
        
    }
	
	public void EstadoUpdate()
	{
		
	}

	public void NivelCargado()
	{
	}
	
	public void Mostrar()
	{
		//TODO: Hacer una barra de progreso...

		if (Application.isLoadingLevel) 
		{
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), manager.imagenCargando, ScaleMode.StretchToFill);
		}
	}
}
