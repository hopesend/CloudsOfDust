using UnityEngine;
using System.IO;

public class EscenarioEdificioOficial: IControlador
{
	private ControladorNiveles manager;
	
	public EscenarioEdificioOficial(ControladorNiveles managerRef)
	{
		manager = managerRef;

        if (Application.loadedLevelName != ScenesParaCambio.EdificioOficial.ToString())
		{
            Application.LoadLevel(ScenesParaCambio.EdificioOficial.ToString());
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
