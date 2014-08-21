using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HAQPHUD : MonoBehaviour 
{
	//Start
	public List<PersonajeControlable> characters;
	private PersonajeControlable characterSelected;

	private PersonajeControlable character01; //Principal
	private PersonajeControlable character02;
	private PersonajeControlable character03;
	private PersonajeControlable character04;
	private PersonajeControlable character05;


	public List<Texture> texturesCharacter;
	public List<Texture> texturesGUI;
	public List<string> configuration;

	private bool HUDstatus;
	public int state;
	private float heigthScreen;
	private float widthScreen;
	private Color auxColorGUI;
	private float fAlpha;
	public float timeRetard;
	private float sizeButtonX;
	private float sizeButtonY;
	private float offsetX;
	private float offsetY; 
	private float musicVolume = 100;
	private float effectsVolume = 100;
	private int indexResolution = 0;
	private int indexGraphics = 0;

	private GUIStyle styleParameters = new GUIStyle();

	void Update() {if(Input.GetKeyDown(KeyCode.Tab)) HUDstatus = !HUDstatus;} //Test
	void Start()
	{
		//characters = GetComponent<GameMaster>().controladoraMundo.personajesSeleccionados; //?
		characterSelected = character01;

		heigthScreen = Screen.height;
		widthScreen = Screen.width;
		auxColorGUI = GUI.color;
		sizeButtonY = heigthScreen / 10;
		sizeButtonX = widthScreen / 5;
		offsetX = 0.7f;
		offsetY = 0.11f;
		state = 0;
		styleParameters.normal.textColor = Color.black;
		styleParameters.alignment = TextAnchor.MiddleRight;
	}
    private void OnGUI()
    {
		if(HUDstatus)
		{
			Transaction();
			GUI.Box(new Rect(0, 0, widthScreen, heigthScreen),"");
			GUI.DrawTexture(new Rect(0, 0, widthScreen, heigthScreen), texturesGUI[0]);
			//GUI BOX
			GUI.DrawTexture(new Rect(widthScreen * 0.05f, heigthScreen * 0.11f, widthScreen * 0.6f, heigthScreen * 0.76f), texturesGUI[1]);
			switch(state)
			{
			case 0:
				ButtonHome();
				break;
			case 1:
				ButtonMessage();
				break;
			case 2:
				ButtonInventary();
				break;
			case 3:
				ButtonTeam();
				break;
			case 4:
				ButtonDevelopment();
				break;
			case 5:
				ButtonStrategy();
				break;
			case 6:
				ButtonBaseData();
				break;
			case 7:
				ButtonOptions();
				break;
			}
		}
		else
		{
			fAlpha = 0; //reseteamos la transaccion
		}
	}
	void ButtonHome()
	{
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * offsetY, sizeButtonX, sizeButtonY), "MENSAJES")) state = state = 1;
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 2), sizeButtonX, sizeButtonY), "INVENTARIO")) state = 2;
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 3), sizeButtonX, sizeButtonY), "EQUIPO")) state = 3;
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 4), sizeButtonX, sizeButtonY), "DESARROLLO")) state = 4;
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 5), sizeButtonX, sizeButtonY), "ESTRATEGIA")) state = 5;
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 6), sizeButtonX, sizeButtonY), "BASE DE DATOS")) state = 6;
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), "OPCIONES")) state = 7;
	}
	void ButtonMessage()
	{
		if(GUI.Button(new Rect(widthScreen * 0.1f, heigthScreen * 0.15f,widthScreen * 0.2f, heigthScreen * 0.07f), "NUEVOS"))
		{

		}
		if(GUI.Button(new Rect(widthScreen * 0.1f, heigthScreen * 0.25f, widthScreen * 0.2f, heigthScreen * 0.07f), "VISTOS"))
		{
			
		}
		if(GUI.Button(new Rect(widthScreen * 0.1f, heigthScreen * 0.35f, widthScreen * 0.2f, heigthScreen * 0.07f), "FAVORITOS"))
		{
			
		}
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), texturesGUI[2])) state = 0;
	}
	void ButtonInventary()
	{
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), texturesGUI[2])) state = 0;
	}
	void ButtonTeam()
	{
		if(characters.Count == 1) //Lectura de personajes
		{
			character01 = characters [0];
		}
		else //textura vacia si no hay personaje
		{
			texturesCharacter[1] = texturesCharacter[0];
		}
		if(characters.Count == 2)
		{
			character02 = characters [1];
		}
		else
		{
			texturesCharacter[2] = texturesCharacter[0];
		}
		if(characters.Count == 3)
		{
			character03 = characters [2];
		}
		else
		{
			texturesCharacter[3] = texturesCharacter[0];
		}
		if(characters.Count == 4)
		{
			character04 = characters [3];
		}
		else
		{
			texturesCharacter[4] = texturesCharacter[0];
		}
		if(characters.Count == 5)
		{
			character05 = characters [5];
		}
		else
		{
			texturesCharacter[5] = texturesCharacter[0];
		}
		//////////////////////////////////////////////////
		//CHARACTERS
		if(GUI.Button(new Rect(widthScreen * 0.065f, heigthScreen * 0.14f, heigthScreen * 0.12f, heigthScreen * 0.12f), texturesCharacter[0]))
		{
			characterSelected = character01;
			if(characterSelected == null)
			{
				Debug.Log("No hay personaje");
			}
		}
		if(GUI.Button(new Rect(widthScreen * 0.065f, heigthScreen * (0.14f * 2), heigthScreen * 0.12f, heigthScreen * 0.12f), texturesCharacter[1]))
		{
			characterSelected = character02;
			if(characterSelected == null)
			{
				Debug.Log("No hay personaje");
			}
		}
		if(GUI.Button(new Rect(widthScreen * 0.065f, heigthScreen * (0.14f * 3), heigthScreen * 0.12f, heigthScreen * 0.12f), texturesCharacter[2]))
		{
			characterSelected = character03;
			if(characterSelected == null)
			{
				Debug.Log("No hay personaje");
			}
		}
		if(GUI.Button(new Rect(widthScreen * 0.065f, heigthScreen * (0.14f * 4), heigthScreen * 0.12f, heigthScreen * 0.12f), texturesCharacter[3]))
		{
			characterSelected = character04;
			if(characterSelected == null)
			{
				Debug.Log("No hay personaje");
			}
		}
		if(GUI.Button(new Rect(widthScreen * 0.065f, heigthScreen * (0.14f * 5), heigthScreen * 0.12f, heigthScreen * 0.12f), texturesCharacter[4]))
		{
			characterSelected = character05;
			if(characterSelected == null)
			{
				Debug.Log("No hay personaje");
			}
		}

		if(characterSelected == null) //En realidad es != //Test
		{
			GUI.Label (new Rect (widthScreen * 0.16f, heigthScreen * 0.17f, widthScreen * 0.2f, heigthScreen * 0.12f), "*"/*characterSelected.name*/, styleParameters);

		    GUI.Label (new Rect (widthScreen * 0.16f, heigthScreen * 0.45f, widthScreen * 0.2f, heigthScreen * 0.12f), "VITALIDAD: "/*+ characterSelected.Vitalidad*/, styleParameters);
			GUI.Label (new Rect (widthScreen * 0.16f, heigthScreen * 0.50f, widthScreen * 0.2f, heigthScreen * 0.12f), "ESTAMINA: "/*+characterSelected.Estamina*/, styleParameters);
			GUI.Label (new Rect (widthScreen * 0.16f, heigthScreen * 0.55f, widthScreen * 0.2f, heigthScreen * 0.12f), "PM: "/*+characterSelected.PM*/, styleParameters);
			GUI.Label (new Rect (widthScreen * 0.16f, heigthScreen * 0.60f, widthScreen * 0.2f, heigthScreen * 0.12f), "FUERZA: "/*+characterSelected.Fuerza*/, styleParameters);
			GUI.Label (new Rect (widthScreen * 0.16f, heigthScreen * 0.65f, widthScreen * 0.2f, heigthScreen * 0.12f), "RESISTENCIA: "/*+characterSelected.Resistencia*/, styleParameters);
			GUI.Label (new Rect (widthScreen * 0.16f, heigthScreen * 0.70f, widthScreen * 0.2f, heigthScreen * 0.12f), "CONCENTRACION: "/*+characterSelected.Concentracion*/, styleParameters);

			GUI.Label (new Rect (widthScreen * 0.35f, heigthScreen * 0.45f, widthScreen * 0.2f, heigthScreen * 0.12f), "ESPIRITU: "/*+characterSelected.Espiritu*/, styleParameters);
			GUI.Label (new Rect (widthScreen * 0.35f, heigthScreen * 0.50f, widthScreen * 0.2f, heigthScreen * 0.12f), "EVASION: "/*+characterSelected.Evasion*/, styleParameters);
			GUI.Label (new Rect (widthScreen * 0.35f, heigthScreen * 0.55f, widthScreen * 0.2f, heigthScreen * 0.12f), "PUNTERIA: "/*+characterSelected.Punteria*/, styleParameters);
			GUI.Label (new Rect (widthScreen * 0.35f, heigthScreen * 0.60f, widthScreen * 0.2f, heigthScreen * 0.12f), "RAPIDEZ: "/*+characterSelected.Rapidez*/, styleParameters);
			GUI.Label (new Rect (widthScreen * 0.35f, heigthScreen * 0.65f, widthScreen * 0.2f, heigthScreen * 0.12f), "SUERTE: "/*+characterSelected.Suerte*/, styleParameters);
			GUI.Label (new Rect (widthScreen * 0.35f, heigthScreen * 0.70f, widthScreen * 0.2f, heigthScreen * 0.12f), "MOVILIDAD: "/*+characterSelected.Movimiento*/, styleParameters);

			if(GUI.Button(new Rect (widthScreen * 0.43f, heigthScreen * 0.15f, widthScreen * 0.2f, heigthScreen * 0.07f), "MOVIMIENTO"))
			{

			}
			if(GUI.Button(new Rect (widthScreen * 0.43f, heigthScreen * 0.25f, widthScreen * 0.2f, heigthScreen * 0.07f), "FISICO"))
			{
				
			}
			if(GUI.Button(new Rect (widthScreen * 0.43f, heigthScreen * 0.35f, widthScreen * 0.2f, heigthScreen * 0.07f), "MAGICO"))
			{
				
			}
		}
		else
		{
			GUI.Label (new Rect (widthScreen * 0.3f, heigthScreen * 0.17f, widthScreen * 0.2f, heigthScreen * 0.12f), "Desbloquee jugadores en el mundo¿?", styleParameters);
		}

		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), texturesGUI[2])) state = 0; //Atras
	}
	void ButtonDevelopment()
	{
		GUI.Label (new Rect (widthScreen * 0.3f, heigthScreen * 0.17f, widthScreen * 0.2f, heigthScreen * 0.12f), "No hay datos", styleParameters);
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), texturesGUI[2])) state = 0; //Atras
	}
	void ButtonStrategy()
	{
		GUI.Label (new Rect (widthScreen * 0.3f, heigthScreen * 0.17f, widthScreen * 0.2f, heigthScreen * 0.12f), "No hay datos", styleParameters);
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), texturesGUI[2])) state = 0; //Atras
	}
	void ButtonBaseData()
	{
		GUI.Label (new Rect (widthScreen * 0.3f, heigthScreen * 0.17f, widthScreen * 0.2f, heigthScreen * 0.12f), "No hay datos", styleParameters);
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), texturesGUI[2])) state = 0; //Atras
	}
	void ButtonOptions()
	{
		string[] auxResolution = {"800x600", "1200x1024", "1600x900","1920x1080"};
		string[] auxGraphics = {"Buena", "Muy Buena", "Excelente"};

		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 6), sizeButtonX, sizeButtonY), "APLICAR"))
		{
			configuration[0] = (auxResolution[indexResolution]);
			configuration[1] = (auxGraphics[indexGraphics]);
			configuration[2] = musicVolume.ToString();
			configuration[3] = effectsVolume.ToString();
		}
		if(GUI.Button(new Rect(widthScreen * 0.1f, heigthScreen * 0.3f, widthScreen * 0.25f, heigthScreen * 0.1f), auxResolution[indexResolution]))
		{
			if(indexResolution == auxResolution.Length - 1) indexResolution = 0;
			else indexResolution++;
		}
		if(GUI.Button(new Rect(widthScreen * 0.35f, heigthScreen * 0.3f, widthScreen * 0.25f, heigthScreen * 0.1f), auxGraphics[indexGraphics]))
		{
			if(indexGraphics == auxGraphics.Length - 1) indexGraphics = 0;
			else indexGraphics++;
		}
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), texturesGUI[2])) state = 0; //Atras
		musicVolume = GUI.HorizontalSlider(new Rect(widthScreen * 0.1f, heigthScreen * 0.5f, widthScreen * 0.5f, 50), musicVolume, 0.0F, 100.0F);
		effectsVolume = GUI.HorizontalSlider(new Rect(widthScreen * 0.1f, heigthScreen * 0.6f, widthScreen * 0.5f, 50), effectsVolume, 0.0F, 100.0F);

	}
	private void Transaction()
	{
		fAlpha = fAlpha + Time.deltaTime / (timeRetard * 2);
		//fAlfha texture transaction
		if(fAlpha < 1)
		{
			 GUI.color = new Color(auxColorGUI.r, auxColorGUI.g, auxColorGUI.b, fAlpha);
	    }
		else
		{
			GUI.color = auxColorGUI;
		}
	}
}