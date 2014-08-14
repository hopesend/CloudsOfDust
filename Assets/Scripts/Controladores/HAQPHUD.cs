using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class HAQPHUD : MonoBehaviour 
{
	public bool HUDstatus;
	public AudioClip soundSelected;
	private int state;
	private float heigthScreen;
	private float widthScreen;
	public Texture background;
	public Texture box;
	public Texture backButton;

	private Color auxColorGUI;
	private float fAlpha;
	public float timeRetard;
	private float sizeButtonX;
	private float sizeButtonY;
	public float aspectRatio;
	private float offsetX;
	private float offsetY;
    
	//GUI principal
	private Vector2 scrollPosition;
	public Texture character1;
	private Texture character2;
	private Texture character3;
	private Texture character4;
	private Texture character5;
	public Texture emptyCharacter;

	//GUI OPTIONS
	private float musicVolume;
	private float effectsVolume;
	private bool read;



	void Update() //Test
	{
		if(Input.GetKeyDown(KeyCode.Tab)) 
		{
			HUDstatus = !HUDstatus;
		}
	} //Test

	void Start()
	{
		heigthScreen = Screen.height;
		widthScreen = Screen.width;
		auxColorGUI = GUI.color;

		/*background = Resources.Load ("background") as Texture;
	    rect = Resources.Load("rect") as Texture;
		backButton = Resources.Load("backButton") as Texture;*/

		sizeButtonY = heigthScreen / (aspectRatio * 10);
		sizeButtonX = widthScreen / ((aspectRatio / 2) * 10);
		state = 0;
		offsetX = 0.7f;
		offsetY = 0.11f;
		aspectRatio = 1;

		//
		character2 = emptyCharacter;
		character3 = emptyCharacter;
		character4 = emptyCharacter;
		character5 = emptyCharacter;
	}
    void OnGUI()
    {
		if(HUDstatus)
		{
			Transaction();
			GUI.Box(new Rect(0, 0, widthScreen, heigthScreen),"");
			GUI.DrawTexture(new Rect(0, 0, widthScreen, heigthScreen), background);
			//GUI BOX
			GUI.DrawTexture(new Rect(widthScreen * 0.05f, heigthScreen * 0.11f, widthScreen * 0.6f, heigthScreen * 0.76f), box);
			switch(state)
			{
			case 0:
				ButtonHome();
				break;
			case 1:
				ButtonMensage();
				break;
			case 2:
				ButtonInventary();
				break;
			case 3:
				ButtonEquipment();
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
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * offsetY, sizeButtonX, sizeButtonY), "MENSAJES"))
		{
			state = 1;
		}
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 2), sizeButtonX, sizeButtonY), "INVENTARIO"))
		{
			state = 2;
		}
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 3), sizeButtonX, sizeButtonY), "EQUIPO"))
		{
			state = 3;
		}
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 4), sizeButtonX, sizeButtonY), "DESARROLLO"))
		{
			state = 4;
		}
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 5), sizeButtonX, sizeButtonY), "ESTRATEGIA"))
		{
			state = 5;
		}
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 6), sizeButtonX, sizeButtonY), "BASE DE DATOS"))
		{
			state = 6;
		}
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), "OPCIONES"))
		{
			state = 7;
		}
		//CHARACTERS


		if(GUI.Button(new Rect(widthScreen * 0.065f, heigthScreen * 0.14f, heigthScreen * 0.12f, heigthScreen * 0.12f), character1))
		{

		}
		if(GUI.Button(new Rect(widthScreen * 0.065f, heigthScreen * (0.14f * 2), heigthScreen * 0.12f, heigthScreen * 0.12f), character2))
		{
			
		}
		if(GUI.Button(new Rect(widthScreen * 0.065f, heigthScreen * (0.14f * 3), heigthScreen * 0.12f, heigthScreen * 0.12f), character3))
		{
			
		}
		if(GUI.Button(new Rect(widthScreen * 0.065f, heigthScreen * (0.14f * 4), heigthScreen * 0.12f, heigthScreen * 0.12f), character4))
		{
			
		}
		if(GUI.Button(new Rect(widthScreen * 0.065f, heigthScreen * (0.14f * 5), heigthScreen * 0.12f, heigthScreen * 0.12f), character5))
		{
			
		}
	    /*GUI.BeginGroup(new Rect(widthScreen * 0.08f, heigthScreen * 0.12f, widthScreen * 0.3f, heigthScreen * 0.71f));
		scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(widthScreen * 0.22f), GUILayout.Height(heigthScreen * 0.68f));
		if(GUILayout.Button("Trasher",  GUILayout.Width(120), GUILayout.Height(120)))
		{

		}
		if(GUILayout.Button("Vacio",  GUILayout.Width(120), GUILayout.Height(120)))
		{
			
		}
		if(GUILayout.Button("Vacio",  GUILayout.Width(120), GUILayout.Height(120)))
		{
			
		}
		if(GUILayout.Button("Vacio",  GUILayout.Width(120), GUILayout.Height(120)))
		{
			
		}
		if(GUILayout.Button("Vacio",  GUILayout.Width(120), GUILayout.Height(120)))
		{
			
		}
		GUILayout.EndScrollView();
		GUI.EndGroup();*/

	}
	void ButtonMensage()
	{
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), backButton)) state = 0;
	}
	void ButtonInventary()
	{
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), backButton)) state = 0;
	}
	void ButtonEquipment()
	{
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), backButton)) state = 0;
	}
	void ButtonDevelopment()
	{
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), backButton)) state = 0;
	}
	void ButtonStrategy()
	{
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), backButton)) state = 0;
	}
	void ButtonBaseData()
	{
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), backButton)) state = 0;
	}
	void ButtonOptions()
	{
		string path = Application.dataPath;
		string[] data = new string[5];
		int count = 0;
		StreamReader streamReader = new StreamReader (path + "\\Config.ini");
		StreamWriter streamWriter = new StreamWriter (path + "\\Config.ini");

		string line;
		using (streamReader)
		{
			while ((line = streamReader.ReadLine()) != null)
			{
				data[count] = line;
				count++;
			}
		}
		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 6), sizeButtonX, sizeButtonY), "APLICAR"))
		{

		}



		///////

		if(GUI.Button(new Rect(widthScreen * offsetX, heigthScreen * (offsetY * 7), sizeButtonX, sizeButtonY), backButton)) state = 0;
		musicVolume = GUI.HorizontalSlider(new Rect(widthScreen * 0.1f, heigthScreen * 0.5f, widthScreen * 0.5f, 50), musicVolume, 0.0F, 100.0F);
		effectsVolume = GUI.HorizontalSlider(new Rect(widthScreen * 0.1f, heigthScreen * 0.6f, widthScreen * 0.5f, 50), effectsVolume, 0.0F, 100.0F);

	}
	void Transaction()
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
