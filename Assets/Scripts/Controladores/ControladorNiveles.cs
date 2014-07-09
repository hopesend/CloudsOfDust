using UnityEngine;
[System.Serializable]
public class ControladorNiveles
{
	//[HideInInspector]
	public IControlador estadoActivo;

    /// <summary>
    /// Imagen que se mostrara en la pantalla de carga de la scene que se cargue.
    /// </summary>
	public Texture2D imagenCargando;

    Vector3 lastPosWorld;
	public static ControladorNiveles instanceRef;

	public static ControladorNiveles InstanceRef()
	{
		if(instanceRef == null)
		{
            instanceRef = new ControladorNiveles();
		}

        return instanceRef;
	}

    private ControladorNiveles()
    {
    }
	
	public void Update()
	{
		if(estadoActivo != null)
		{
			estadoActivo.EstadoUpdate();
		}
	}
	
	public void OnGUI()
	{
		if(estadoActivo != null)
			estadoActivo.Mostrar();
	}

	public void OnLevelWasLoaded(int level)
	{
		if (estadoActivo != null)
            estadoActivo.NivelCargado();
	}
	
	public void CambiarEstado(IControlador nuevoEstado)
	{
		estadoActivo = nuevoEstado;
	}

    public void IrMenuPrincipal()
    {
        estadoActivo = new MenuPrincipal(this);
        GameMaster.instanceRef.EstadoActual = EstadoJuego.MenuPrincipal;
    }

    public void IrSceneVecindario()
    {
        estadoActivo = new EscenarioVecindario(this);
        GameMaster.instanceRef.EstadoActual = EstadoJuego.Mundo;
    }

    public void IrSceneBatallaTutorial()
    {
        lastPosWorld = GameMaster.instanceRef.controladoraMundo.PosicionPersonajeWorld;
        estadoActivo = new EscenarioBatallaTutorial(this);
        GameMaster.instanceRef.EstadoActual = EstadoJuego.Batalla;
    }

    public void CambiarSceneSegunEnum(ScenesParaCambio temp)
    {
        switch (temp) 
        {
            case ScenesParaCambio.MenuPrincipal:
                {
                    IrMenuPrincipal();
                    break;
                }
            case ScenesParaCambio.Vecindario:
                {
                    IrSceneVecindario();
                    break;
                }
            case ScenesParaCambio.BatallaTutorial:
                {
                    IrSceneBatallaTutorial();
                    break;
                }
        }
    }
}