﻿using UnityEngine;
using System.IO;

public class EscenarioSalaDoctor: IControlador
{
	private ControladorNiveles manager;
	
	public EscenarioSalaDoctor(ControladorNiveles managerRef)
	{
		manager = managerRef;
		ControladorGlobal.instanceRef.Manager = managerRef;
		
		if(Application.loadedLevel != 6)
		{
			Application.LoadLevel(6);
		}
	}

    public void CargarDatosPlayer()
    {
        if (!ControladorJugador.instanceRef.Cargar_Datos_XML(ControladorJugador.instanceRef.Trasher.Get_Nombre()))
        {
            //TODO: Lanzar un mensaje de Error que no existe el fichero xml
        }
        
    }
	
	public void EstadoUpdate()
	{
		
	}

	public void NivelCargado(int level)
	{
		if (level == 1)
		{
			CargarDatosPlayer();
		}
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