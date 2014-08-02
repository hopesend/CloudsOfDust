using UnityEngine;
using System;
/// <summary>
/// Encargada de manejar los cambios de escenarios
/// </summary>
[System.Serializable]
public class ControladorNiveles
{
    /// <summary>
    /// Escenario actual.
    /// </summary>
	public IEscenarios estadoActivo;

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
        {
            estadoActivo.NivelCargado();
        }
	}
	
    /// <summary>
    /// Este metodo no debe llamarse externamente, se puede usar los metodos mas directos como IrMenuPrincipal()
    /// </summary>
    /// <param name="nuevoEstado"></param>
	private void CambiarEstado(IEscenarios nuevoEstado)
	{
		estadoActivo = nuevoEstado;
	}

    public void IrMenuPrincipal()
    {
        estadoActivo = new MenuPrincipal(this);
        GameMaster.InstanceRef.EstadoActual = EstadoJuego.MenuPrincipal;
    }

    public void IrSceneVecindario()
    {
        estadoActivo = new EscenarioVecindario(this);
        GameMaster.InstanceRef.EstadoActual = EstadoJuego.Mundo;
    }

    public void IrSceneSalaActualizacion()
    {
        estadoActivo = new EscenarioSalaActualizacion(this);
        GameMaster.InstanceRef.EstadoActual = EstadoJuego.Mundo;
    }

    public void IrSceneCasaInterior()
    {
        estadoActivo = new EscenarioCasaInterior(this);
        GameMaster.InstanceRef.EstadoActual = EstadoJuego.Mundo;
    }

    public void IrSceneRecepcion()
    {
        estadoActivo = new EscenarioRecepcion(this);
        GameMaster.InstanceRef.EstadoActual = EstadoJuego.Mundo;
    }

    public void IrSceneInteriorLeveler()
    {
        estadoActivo = new EscenarioInteriorLeveler(this);
        GameMaster.InstanceRef.EstadoActual = EstadoJuego.Mundo;
    }

    public void IrSceneEdificioOficial()
    {
        estadoActivo = new EscenarioEdificioOficial(this);
        GameMaster.InstanceRef.EstadoActual = EstadoJuego.Mundo;
    }

    public void IrSceneBatallaTutorial()
    {
        lastPosWorld = GameMaster.InstanceRef.controladoraMundo.PosicionPersonajeWorld;
        estadoActivo = new EscenarioBatallaTutorial(this);
        GameMaster.InstanceRef.EstadoActual = EstadoJuego.Batalla;
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
            case ScenesParaCambio.Batalla:
                break;
            case ScenesParaCambio.EdificioOficial:
                {
                    IrSceneEdificioOficial();
                    break;
                }
            case ScenesParaCambio.CasaInterior:
                {
                    IrSceneCasaInterior();
                    break;
                }
            case ScenesParaCambio.InteriorLeveler:
                {
                    IrSceneInteriorLeveler();
                    break;
                }
            case ScenesParaCambio.Recepcion:
                {
                    IrSceneRecepcion();
                    break;
                }
            case ScenesParaCambio.SalaActualizacion:
                {
                    IrSceneSalaActualizacion();
                    break;
                }
            default:
                break;
        }


        
    }
}