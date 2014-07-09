using UnityEngine;
using System.IO;

public class EscenarioRecepcion: IControlador
{
	private ControladorNiveles manager;
	
	public EscenarioRecepcion(ControladorNiveles managerRef)
	{
		manager = managerRef;
		ControladorGlobal.instanceRef.Manager = managerRef;

        if (Application.loadedLevelName != ScenesParaCambio.Recepcion.ToString())
		{
            Application.LoadLevel(ScenesParaCambio.Recepcion.ToString());
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
