using UnityEngine;
using System.Collections;

public class HAQPHUD : MonoBehaviour 
{
	public bool inTab;
	private int state;
	private float heigthScreen;
	private float widthScreen;
	public Texture background;
	public Texture rect;

	private Color auxColorGUI;
	private float fAlpha;
	public float timeRetard;
	private float sizeButtonX;
	private float sizeButtonY;
	public float aspectRatio;
	private float offsetX;
	private float offsetY;

	void Update() //Test
	{
		if(Input.GetKeyDown(KeyCode.Tab)) 
		{
			inTab = !inTab;
		}
	} //Test

	void Start()
	{
		heigthScreen = Screen.height;
		widthScreen = Screen.width;
		auxColorGUI = GUI.color;

		sizeButtonY = heigthScreen / (aspectRatio * 10);
		sizeButtonX = widthScreen / ((aspectRatio / 2) * 10);
		state = 0;
		offsetX = 0.7f;
		offsetY = 0.11f;
	}
    void OnGUI()
    {
		if(inTab)
		{
			Transaction();
			GUI.Box(new Rect(0, 0, widthScreen, heigthScreen),"");
			GUI.DrawTexture(new Rect(0, 0, widthScreen, heigthScreen), background);
			GUI.DrawTexture(new Rect(widthScreen * 0.01f, heigthScreen * 0.01f, widthScreen * 0.98f, heigthScreen * 0.98f), rect); 
			switch(state)
			{
			case 0:
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
				break;
			case 1:

				break;
			}
		}
		else
		{
			fAlpha = 0; //reseteamos la transaccion
		}
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
