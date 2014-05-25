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

		if (!ControladorJugador.instanceRef.Cargar_Datos_XML ()) 
		{
			//TODO: Lanzar un mensaje de Error que no existe el fichero xml
		}
	}
	
	public void EstadoUpdate()
	{
		
	}
	
	public void Mostrar()
	{

	}
}
