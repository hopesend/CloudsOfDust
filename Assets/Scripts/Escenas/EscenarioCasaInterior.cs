using UnityEngine;
using System.IO;

public class EscenarioCasaInterior: IEscenarios
{
	private ControladorNiveles manager;
	
	public EscenarioCasaInterior(ControladorNiveles managerRef)
	{
		manager = managerRef;
		//ControladorGlobal.instanceRef.Manager = managerRef;

        if (Application.loadedLevelName != ScenesParaCambio.CasaInterior.ToString())
		{
            Application.LoadLevel(ScenesParaCambio.CasaInterior.ToString());
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
		HUD.instanceRef.InsertarMensajeHAPQ ("01", "Tuly");
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
