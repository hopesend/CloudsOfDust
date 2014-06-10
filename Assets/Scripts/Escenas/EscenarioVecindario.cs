using UnityEngine;
using System.IO;

public class EscenarioVecindario: IControlador
{
	private ControladorNiveles manager;
	
	public EscenarioVecindario(ControladorNiveles managerRef)
	{
		manager = managerRef;
		
		if(Application.loadedLevelName != "Vecindario")
		{
			Application.LoadLevel("Vecindario");
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
	
	public void Mostrar()
	{
		//TODO: Hacer una barra de progreso...

		if (Application.isLoadingLevel) 
		{
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), manager.imagenCargando, ScaleMode.StretchToFill);
		}
	}
}
