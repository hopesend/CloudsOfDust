using UnityEngine;
using System.IO;

public class EscenarioInteriorLeveler: IControlador
{
	private ControladorNiveles manager;
	
	public EscenarioInteriorLeveler(ControladorNiveles managerRef)
	{
		manager = managerRef;
		ControladorGlobal.instanceRef.Manager = managerRef;
		
		if(Application.loadedLevelName != ScenesParaCambio.InteriorLeveler.ToString())
		{
            Application.LoadLevel(ScenesParaCambio.InteriorLeveler.ToString());
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
