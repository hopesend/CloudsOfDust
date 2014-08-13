using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HAQPHUD2 : MonoBehaviour {
	//int estado;
	public bool menu;
	public bool mostrarHAPQ = false;
	public GUISkin mySkin;
	public int ventanaIDTextos = 0;
	public string cabecera = "";
	private List<Texto> mensajesHAPQ;
	private Vector2 posicionBarraScroll;
	// Use this for initialization
	void Start () {
		menu = false;

	}
	
	// Update is called once per frame
	void Update () {
		//estado = gameObject.GetComponent<Historia>().estado; //esto es para la cuando estemos programando la historia.
		gameObject.GetComponent<CharacterController>().enabled = !menu; //desactiva el movimiento del Personaje cuando este activo el menu
		gameObject.GetComponent<AnimPlayer>().enabled = !menu; //desactiva la animacion del Personaje cuando este activo el menu
		//Camera.main.GetComponent<MouseOrbitImproved>().enabled = !menu; //desactiva el movimiento de la camara
		if (Input.GetKeyDown(KeyCode.Tab))
		{	
			menu = !menu;

			Informacion nuevaInformacion = new Informacion();
			mensajesHAPQ = nuevaInformacion.Devolver_Lista_Mensajes_HAPQ();
		}
	}

	void OnGUI()
	{
		if (menu)
		{
			Mostrar_Menu();
		}

		if (mostrarHAPQ) 
		{
			GUILayout.Window (ventanaIDTextos, new Rect(0, 0, Screen.width, Screen.height), Creacion_Ventana_Textos, cabecera);
		}
	}

	private void Mostrar_Menu()
	{
		Rect R_M = new Rect(0,0,Screen.width,Screen.height);
		Rect[] R_der = new Rect[8];
		string[] apartados = new string[8]{"Mensajes","Inventario","Equipo","Desarrollo","Estrategia","Condicion","Base de Datos","Opciones"};

		GUI.Box (R_M,"");
		for (int i = 0;i<8;i++){
			R_der[i] = new Rect(Screen.width/2,i*Screen.height/8,Screen.width/2,Screen.height/8);
			if (GUI.Button(R_der[i], apartados[i]))
			{
				menu = false;

				switch(i)
				{	
					//Mensajes
					case 0: 
						ventanaIDTextos = 0;
						cabecera = "Mensajes HAPQ";
						break;
			
					//Inventario
					case 1: 
						ventanaIDTextos = 1;
						cabecera = "Inventario";
						break;

					//Equipo
					case 2: 
						ventanaIDTextos = 2;
						cabecera = "Equipo";
						break;

					//Desarrollo
					case 3: 
						ventanaIDTextos = 3;
						cabecera = "Desarrollo";
						break;

					//Estrategia
					case 4: 
						ventanaIDTextos = 4;
						cabecera = "Estrategia";
						break;

					//Condicion
					case 5:	
						ventanaIDTextos = 5;
						cabecera = "Condicion";
						break;

					//Base de Datos
					case 6: 
						ventanaIDTextos = 6;
						cabecera = "Base de Datos";
						break;

					//Opciones
					case 7: 
						ventanaIDTextos = 7;
						cabecera = "Opciones";
						break;
				}
				mostrarHAPQ = true;
			}
		}
	}

	private void Creacion_Ventana_Textos(int Id) 
	{
		switch(Id)
		{
			case 0:	
				Mostrar_Mensajes();
				break;
			case 1:	
				Mostrar_Inventario();
				break;
			case 2: 
				Mostrar_Equipo();
				break;
			case 3: 
				Mostrar_Desarrollo();
				break;
			case 4: 
				Mostrar_Estrategia();
				break;
			case 5:	
				Mostrar_Condicion();
				break;
			case 6: 
				Mostrar_BaseDeDatos();
				break;
			case 7: 
				Mostrar_Opciones();
				break;
		}
	}
	
	
	private void Mostrar_Mensajes()
	{
		posicionBarraScroll = GUILayout.BeginScrollView (posicionBarraScroll);
			GUILayout.BeginVertical ();
				if (GUILayout.Button ("Volver")) 
				{
					mostrarHAPQ = false;
					menu = true;
				}

				foreach (Texto nuevoLabel in mensajesHAPQ) 
				{
					GUILayout.Label(nuevoLabel.Receptor);
			        GUILayout.Label(nuevoLabel.TextoMostrar);
			        GUILayout.Label("");
				}
			GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}

	private void Mostrar_Inventario()
	{
	}

	private void Mostrar_Equipo()
	{
	}

	private void Mostrar_Desarrollo()
	{
	}

	private void Mostrar_Estrategia()
	{
	}

	private void Mostrar_Condicion()
	{
	}

	private void Mostrar_BaseDeDatos()
	{
	}

	private void Mostrar_Opciones()
	{
	}
}
