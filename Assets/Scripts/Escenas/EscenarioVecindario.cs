using UnityEngine;
using System.IO;

public class EscenarioVecindario: IEscenarios
{
	private ControladorNiveles manager;

    public Vector3 posicionInicial = new Vector3(173.7028f, 0.2f, 2.057544f);
	
	public EscenarioVecindario(ControladorNiveles managerRef)
	{
		manager = managerRef;

        if (Application.loadedLevelName != ScenesParaCambio.Vecindario.ToString())
		{
            Application.LoadLevel(ScenesParaCambio.Vecindario.ToString());
		}
	}
	
	public void EstadoUpdate()
	{
		
	}

    public void NivelCargado()
	{
        GameMaster.InstanceRef.InicializarMundo(posicionInicial);
        GameMaster.InstanceRef.controladoraMundo.PrepararJugadorScripts();
        GameMaster.InstanceRef.controladoraMundo.PosicionPersonajeWorld = GameObject.FindGameObjectWithTag("Respawn").transform.position;

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
