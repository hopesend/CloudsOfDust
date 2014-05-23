using UnityEngine;

public class ControladorNiveles : MonoBehaviour 
{
	[HideInInspector]
	public IControlador estadoActivo;

	private static ControladorNiveles instanceRef;

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
	
	public void CambiarEstado(IControlador nuevoEstado)
	{
		estadoActivo = nuevoEstado;
	}
}