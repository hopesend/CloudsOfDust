using UnityEngine;
using System.IO;

public class EscenarioCasaInterior: IEscenarios
{
	private ControladorNiveles manager;
	
	public EscenarioCasaInterior(ControladorNiveles managerRef)
	{
		manager = managerRef;
		//ControladorGlobal.InstanceRef.Manager = managerRef;

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

        PersonajeControlable temp = GameObject.FindObjectOfType<PersonajeControlable>();
        if (temp == null)
        {
            GameMaster.InstanceRef.controladoraMundo.AddPersonajeSeleccionado(GameMaster.InstanceRef.InstanciarJugador(GameMaster.InstanceRef.controladorDB.DBpersonajes.GetPersonajeByID(IDPersonajes.Trasher), GameObject.FindGameObjectWithTag("Respawn").transform.position), true);
        }
        else
        {
            GameMaster.InstanceRef.controladoraMundo.AddPersonajeSeleccionado(temp, true);
            GameMaster.InstanceRef.controladoraMundo.PosicionPersonajeWorld = GameObject.FindGameObjectWithTag("Respawn").transform.position;
        }
        //
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
