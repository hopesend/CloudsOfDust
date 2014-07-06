using UnityEngine;

public class ControladorNiveles : MonoBehaviour 
{
	//[HideInInspector]
	public IControlador estadoActivo;

    /// <summary>
    /// Imagen que se mostrara en la pantalla de carga de la scene que se cargue.
    /// </summary>
	public Texture2D imagenCargando;

	public static ControladorNiveles instanceRef;

	void Awake()
	{
		if(instanceRef == null)
		{
			instanceRef = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			DestroyImmediate(gameObject);
		}
	}

	void Start()
	{
		estadoActivo = new MenuPrincipal(this);
	}
	
	void Update()
	{
		if(estadoActivo != null)
		{
			estadoActivo.EstadoUpdate();
		}
	}
	
	void OnGUI()
	{
		if(estadoActivo != null)
			estadoActivo.Mostrar();
	}

	void OnLevelWasLoaded(int level)
	{
		if (estadoActivo != null)
			estadoActivo.NivelCargado (level);
	}
	
	public void CambiarEstado(IControlador nuevoEstado)
	{
		estadoActivo = nuevoEstado;
	}

    /*public void OnLevelWasLoaded(int level){
        if (level == 1)
        {
            ((EscenarioVecindario)estadoActivo).CargarDatosPlayer();
        }
    }*/
}