using UnityEngine;
using System.Collections;

[System.Serializable]
public class HUDCombo
{
	public Texture imageCombo;

	/*
	public Texture buttonQ;
	public Texture buttonW;
	public Texture buttonE;
	public Texture buttonA;
	public Texture buttonS;
	public Texture buttonD;
	public Texture buttonZ;
	public Texture buttonX;
	public Texture buttonC;
    */

	public int sizeTexture;
	public float  sizeTime;
	public float marge;
    public bool inCombo;

    public bool InCombo
    {
        get { return inCombo; }
        set 
        { 
            inCombo = value;
            if (inCombo)
            {
                ControladoraBaseBatalla.InstanceRef().faseActual = FasesBatalla.EjecutandoCombo;
                Camera.main.GetComponent<CameraBatalla>().keyBoardControl = false;
                Camera.main.GetComponent<CameraBatalla>().mouseControl = false;
            }
            else
            {
                Camera.main.GetComponent<CameraBatalla>().keyBoardControl = true;
                Camera.main.GetComponent<CameraBatalla>().mouseControl = true;
            }
        }
    }
	public bool showCombo;
	private bool onPressed;

	public string button;
	public string key;

	private float HEIGHT;
	private float WIDTH;

	private GUIStyle style;

 
	public HUDCombo()
	{
		if(sizeTexture == 0) sizeTexture = 50;
		if(marge == 0) marge = 0.25f;
		HEIGHT = Screen.height;
		WIDTH = Screen.width;
		if(sizeTime == 0) sizeTime = 500;
		
	}

    public void PrepararFonts()
    {
        style = new GUIStyle();
        style.fontSize = sizeTexture - 25;
    }
	public void OnGUI()
	{
        if(inCombo) //Muestra todos los botones
		{
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2, HEIGHT/2 - sizeTexture/2, sizeTexture, sizeTexture), imageCombo)) //S
			{
                ControladoraBaseBatalla.InstanceRef().controladoraCombo.buttonPressed = "5";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2, HEIGHT/2 - sizeTexture/2 + (sizeTexture + marge), sizeTexture, sizeTexture), imageCombo)) //X
			{
                ControladoraBaseBatalla.InstanceRef().controladoraCombo.buttonPressed = "2";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2, HEIGHT/2 - sizeTexture/2 - (sizeTexture + marge), sizeTexture, sizeTexture), imageCombo)) //W
			{
                ControladoraBaseBatalla.InstanceRef().controladoraCombo.buttonPressed = "8";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2 - (sizeTexture + marge), HEIGHT/2 - sizeTexture/2, sizeTexture, sizeTexture), imageCombo)) //A
			{
                ControladoraBaseBatalla.InstanceRef().controladoraCombo.buttonPressed = "4";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2 + (sizeTexture + marge), HEIGHT/2 - sizeTexture/2, sizeTexture, sizeTexture), imageCombo))//D
			{
                ControladoraBaseBatalla.InstanceRef().controladoraCombo.buttonPressed = "6";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2 + (sizeTexture + marge), HEIGHT/2 - sizeTexture/2 + (sizeTexture + marge), sizeTexture, sizeTexture), imageCombo))//C
			{
                ControladoraBaseBatalla.InstanceRef().controladoraCombo.buttonPressed = "3";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2 + (sizeTexture + marge), HEIGHT/2 - sizeTexture/2 - (sizeTexture + marge), sizeTexture, sizeTexture), imageCombo))//D
			{
                ControladoraBaseBatalla.InstanceRef().controladoraCombo.buttonPressed = "9";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2 - (sizeTexture + marge), HEIGHT/2 - sizeTexture/2 + (sizeTexture + marge), sizeTexture, sizeTexture), imageCombo)) //Z
			{
                ControladoraBaseBatalla.InstanceRef().controladoraCombo.buttonPressed = "1";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2 - (sizeTexture + marge), HEIGHT/2 - sizeTexture/2 - (sizeTexture + marge), sizeTexture, sizeTexture), imageCombo)) //Q
			{
                ControladoraBaseBatalla.InstanceRef().controladoraCombo.buttonPressed = "7";
			}
			GUI.Box(new Rect(WIDTH/2 - sizeTime/2, HEIGHT/2 + sizeTexture * 2 + marge, sizeTime, sizeTexture/2), "TIME");

			switch(button)
			{
			    case "1":
				    GUI.Label(new Rect(WIDTH/2 - sizeTexture/2 - (sizeTexture + marge) + marge, HEIGHT/2 - sizeTexture/2 + (sizeTexture + marge),sizeTexture ,sizeTexture), key, style);
				    break;
			    case "2":
				    GUI.Label(new Rect(WIDTH/2 - sizeTexture/2 + marge, HEIGHT/2 - sizeTexture/2 + (sizeTexture + marge), sizeTexture ,sizeTexture), key, style);
				    break;
			    case "3":
				GUI.Label(new Rect(WIDTH/2 - sizeTexture/2 + (sizeTexture + marge) + marge, HEIGHT/2 - sizeTexture/2 + (sizeTexture + marge), sizeTexture ,sizeTexture), key, style);
				    break;
			    case "4":
				GUI.Label(new Rect(WIDTH/2 - sizeTexture/2 - (sizeTexture + marge) + marge, HEIGHT/2 - sizeTexture/2, sizeTexture ,sizeTexture), key, style);
				    break;
			    case "5":
				    GUI.Label(new Rect(WIDTH/2 - sizeTexture/2 + marge, HEIGHT/2 - sizeTexture/2,sizeTexture ,sizeTexture), key, style);
				    break;
			    case "6":
				GUI.Label(new Rect(WIDTH/2 - sizeTexture/2 + (sizeTexture + marge) + marge, HEIGHT/2 - sizeTexture/2,sizeTexture ,sizeTexture), key, style);
				    break;
			    case "7":
				    GUI.Label(new Rect(WIDTH/2 - sizeTexture/2 - (sizeTexture + marge) + marge, HEIGHT/2 - sizeTexture/2 - (sizeTexture + marge),sizeTexture ,sizeTexture), key, style);
				    break;
			    case "8":
				    GUI.Label(new Rect(WIDTH/2 - sizeTexture/2 + marge, HEIGHT/2 - sizeTexture/2 - (sizeTexture + marge),sizeTexture ,sizeTexture), key, style);
				    break;
			    case "9":
				    GUI.Label(new Rect(WIDTH/2 - sizeTexture/2 + (sizeTexture + marge) + marge, HEIGHT/2 - sizeTexture/2 - (sizeTexture + marge),sizeTexture ,sizeTexture), key, style);
				    break;
			}
		}
	}
}
