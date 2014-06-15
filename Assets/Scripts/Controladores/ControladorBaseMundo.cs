using UnityEngine;
using System.Collections;

public class ControladorBase : MonoBehaviour 
{
	//Instancia de la clase (Singleton)
	[HideInInspector]
	public static ControladorBase instanceRef;

	//Variables publicas o privadas
	public float dinero;
	public int escenaActual;
	public Vector3[] entrada;

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
		dinero=100;

	}
}

