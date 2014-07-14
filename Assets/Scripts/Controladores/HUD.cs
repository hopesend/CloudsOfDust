using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HUD {
	
	bool b_movimiento;
	bool b_fisico;
	bool b_magia;
	bool b_objeto;
	public Vector2 scrollPosition = Vector2.zero;


    public PersonajeControlable personajeActual;

    public bool showHudPLayer = false;

    Rect HUDBox;
    Rect Ataques;

    GUIStyle style1;
    GUIStyle style2;
    GUIStyle style3;
    GUIStyle style4;
    Texture2D textura1;
    Texture2D textura2;
    Texture2D textura3;
    Texture2D textura4;

	//Variables Texto
	public bool mostrarInteraccion = false;
	public string cabecera;
	public List<Texto> cuerpo;
	public int ventanaIDTextos = 1;
	private Vector2 posicionBarraScrollTextos;
	//--------------



    public static HUD instanceRef;

    public static HUD InstanceRef()
    {
        if (instanceRef == null)
        {
            instanceRef = new HUD();
        }

        return instanceRef;
    }


	

	// Use this for initialization
	public HUD() {
	}

    public void PrepararTexturas()
    {
        HUDBox = new Rect(0, Screen.height * 4 / 5, Screen.width, Screen.height / 5);
        Ataques = new Rect(0, 0, Screen.width / 3, Screen.height / 2);
        style1 = new GUIStyle();
        style2 = new GUIStyle();
        style3 = new GUIStyle();
        style4 = new GUIStyle();
        textura1 = new Texture2D(1, 1);
        textura2 = new Texture2D(1, 1);
        textura3 = new Texture2D(1, 1);
        textura4 = new Texture2D(1, 1);
    }
	
	// Update is called once per frame
	public void Update () {
        if (personajeActual != null)
        {
            showHudPLayer = true;
        }

        else
        {
            if (ControladoraBaseBatalla.InstanceRef().TurnoActual != null)
            {
                personajeActual = (PersonajeControlable)ControladoraBaseBatalla.InstanceRef().TurnoActual.Personaje;
            }
        }
	}
	
	public void OnGUI()
	{
		//Todo lo que vaya en la GUI, hay que insertarla en metodos...
    	if (mostrarInteraccion) 
		{
			mostrarHudInteraccion();
		}

        if (showHudPLayer)
        {
			mostrarHudPlayer();     
        }

		mostrarHudBase ();
        
	}

	private void mostrarHudInteraccion()
	{
		GUILayout.Window (ventanaIDTextos, new Rect(0, Screen.height-(Screen.height/5), Screen.width, Screen.height), Creacion_Ventana_Textos, cabecera);
	}

	private void Creacion_Ventana_Textos(int Id) 
	{
		switch(Id)
		{
		case 1:	
			VentanaDescripciones();
			break;
		case 2:	
			//VentanaConversaciones();
			break;
		}
	}

	public void VentanaDescripciones()
	{
		posicionBarraScrollTextos = GUILayout.BeginScrollView (posicionBarraScrollTextos);
			GUILayout.BeginVertical ();
				foreach (Texto nuevoTexto in cuerpo) 
				{
					GUILayout.Label(nuevoTexto.TextoMostrar);
				}
			GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}

	private void mostrarHudPlayer()
	{
		scrollPosition = GUI.BeginScrollView(new Rect(Ataques.width / 3, 0, Ataques.width * 2 / 3, Ataques.height), scrollPosition, new Rect(0, 0, Ataques.width * 2 / 3, Screen.height));
		
		GUI.EndScrollView();
		
		
		//Ataques
		GUI.Box(Ataques, "");
		if (ControladoraBaseBatalla.InstanceRef().faseActual == FasesBatalla.Estrategia)
		{
			if (b_movimiento = GUILayout.Toggle(b_movimiento, "Movimiento", "Button", GUILayout.Height(Ataques.height / 4 - 5), GUILayout.Width(Ataques.width / 3)))
			{
				b_fisico = false;
				b_magia = false;
				b_objeto = false;
				
				scrollPosition = GUI.BeginScrollView(new Rect(Ataques.width / 3, 0, Ataques.width * 2 / 3, Ataques.height), scrollPosition, new Rect(0, 0, Ataques.width * 2 / 3, Screen.height));
				if (personajeActual.comportamientoActual == ComportamientoJugador.EsperandoComportamiento)
				{
					if (GUI.Button(new Rect(0, 0, 100, 20), "Mover"))
					{
						personajeActual.MoverBatalla(20);
					} 
				}
				GUI.EndScrollView();
			} 
		}
		
		if (ControladoraBaseBatalla.InstanceRef().faseActual == FasesBatalla.Accion)
		{
			if (b_fisico = GUILayout.Toggle(b_fisico, "Fisico", "Button", GUILayout.Height(Ataques.height / 4 - 5)))
			{
				b_movimiento = false;
				b_magia = false;
				b_objeto = false;
				
				scrollPosition = GUI.BeginScrollView(new Rect(Ataques.width / 3, 0, Ataques.width * 2 / 3, Ataques.height), scrollPosition, new Rect(0, 0, Ataques.width * 2 / 3, Screen.height));
				
				GUI.EndScrollView();
			}
			
			if (b_magia = GUILayout.Toggle(b_magia, "Magia", "Button", GUILayout.Height(Ataques.height / 4 - 5)))
			{
				b_movimiento = false;
				b_fisico = false;
				b_objeto = false;
				
				
				scrollPosition = GUI.BeginScrollView(new Rect(Ataques.width / 3, 0, Ataques.width * 2 / 3, Ataques.height), scrollPosition, new Rect(0, 0, Ataques.width * 2 / 3, Screen.height));
				
				
				GUI.EndScrollView();
				
			}
			
			if (b_objeto = GUILayout.Toggle(b_objeto, "Objeto", "Button", GUILayout.Height(Ataques.height / 4 - 5)))
			{
				b_movimiento = false;
				b_fisico = false;
				b_magia = false;
				
				scrollPosition = GUI.BeginScrollView(new Rect(Ataques.width / 3, 0, Ataques.width * 2 / 3, Ataques.height), scrollPosition, new Rect(0, 0, Ataques.width * 2 / 3, Screen.height));
				
				GUI.EndScrollView();
			} 
		}
	}

	private void mostrarHudBase()
	{
		//HUD
		GUI.Box(HUDBox, "");
		
		for (int i = 0; i < ControladoraBaseBatalla.jugadores.Count; i++)
		{
			PersonajeControlable temp = ControladoraBaseBatalla.jugadores[i];
			GUI.backgroundColor = Color.clear;
			GUI.TextArea(new Rect(HUDBox.x + 5, HUDBox.y + 5, 100, 30), temp.Get_Nombre()); //Nombre
			GUI.TextArea(new Rect(HUDBox.x + 5, HUDBox.y + 35, 100, 30), "Vit: " + temp.vit.Actual + "/" + temp.vit.Valor); //vitalidad
			GUI.TextArea(new Rect(HUDBox.x + 5, HUDBox.y + 55, 100, 30), "Esn: " + temp.esn.Actual + "/" + temp.esn.Valor);	//estamina
			GUI.TextArea(new Rect(HUDBox.x + 5, HUDBox.y + 75, 100, 40), "PM: " + temp.pm.Actual + "/" + temp.pm.Valor);	//puntos magicos
			
			
			GUI.backgroundColor = Color.red;
			textura1.SetPixel(1, 1, Color.red);
			style1.normal.background = textura1;
			GUI.Box(new Rect(HUDBox.x + 105, HUDBox.y + 40, temp.vit.Actual * (Screen.width / 3 - 110) / temp.vit.Valor, 10), "", style1);
			
			GUI.backgroundColor = Color.green;
			textura2.SetPixel(1, 1, Color.green);
			style2.normal.background = textura2;
			GUI.Box(new Rect(HUDBox.x + 105, HUDBox.y + 60, temp.esn.Actual * (Screen.width / 3 - 110) / temp.esn.Valor, 10), "", style2);
			
			GUI.backgroundColor = Color.blue;
			textura3.SetPixel(1, 1, Color.yellow);
			style3.normal.background = textura3;
			GUI.Box(new Rect(HUDBox.x + 105, HUDBox.y + 80, temp.pm.Actual * (Screen.width / 3 - 110) / temp.pm.Valor, 10), "", style3);
			
			GUI.Box(new Rect(HUDBox.x + 105, HUDBox.y + 5, ControladoraBaseBatalla.InstanceRef().controladoraTurno.listaOrdenTurnos[0].TiempoActual * (Screen.width / 3 - 110) / 100, 10), "", style3); 
		}
	}
}

