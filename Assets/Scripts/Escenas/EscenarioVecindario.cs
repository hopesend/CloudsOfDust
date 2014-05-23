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
	
	public void EstadoUpdate()
	{
		
	}
	
	public void Mostrar()
	{

	}
}
