using UnityEngine;
using System.Collections;

public class HUDCombo : MonoBehaviour 
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
	private bool onPressed;

	private float HEIGHT;
	private float WIDTH;

	public void Start()
	{
		if(sizeTexture == 0) sizeTexture = 50;
		if(marge == 0) marge = 0.25f;
		HEIGHT = Screen.height;
		WIDTH = Screen.width;
		if(sizeTime == 0) sizeTime = 1000;
	}
	public void OnGUI()
	{
        if(inCombo) //Muestra todos los botones
		{
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2, HEIGHT/2 - sizeTexture/2, sizeTexture, sizeTexture), imageCombo)) //S
			{
				GetComponent<Combo>().buttonPressed = "5";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2, HEIGHT/2 - sizeTexture/2 + (sizeTexture + marge), sizeTexture, sizeTexture), imageCombo)) //X
			{
				GetComponent<Combo>().buttonPressed = "2";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2, HEIGHT/2 - sizeTexture/2 - (sizeTexture + marge), sizeTexture, sizeTexture), imageCombo)) //W
			{
				GetComponent<Combo>().buttonPressed = "8";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2 - (sizeTexture + marge), HEIGHT/2 - sizeTexture/2, sizeTexture, sizeTexture), imageCombo)) //A
			{
				GetComponent<Combo>().buttonPressed = "4";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2 + (sizeTexture + marge), HEIGHT/2 - sizeTexture/2, sizeTexture, sizeTexture), imageCombo))//D
			{
				GetComponent<Combo>().buttonPressed = "6";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2 + (sizeTexture + marge), HEIGHT/2 - sizeTexture/2 + (sizeTexture + marge), sizeTexture, sizeTexture), imageCombo))//C
			{
				GetComponent<Combo>().buttonPressed = "3";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2 + (sizeTexture + marge), HEIGHT/2 - sizeTexture/2 - (sizeTexture + marge), sizeTexture, sizeTexture), imageCombo))//D
			{
				GetComponent<Combo>().buttonPressed = "9";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2 - (sizeTexture + marge), HEIGHT/2 - sizeTexture/2 + (sizeTexture + marge), sizeTexture, sizeTexture), imageCombo)) //Z
			{
				GetComponent<Combo>().buttonPressed = "1";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexture/2 - (sizeTexture + marge), HEIGHT/2 - sizeTexture/2 - (sizeTexture + marge), sizeTexture, sizeTexture), imageCombo)) //Q
			{
				GetComponent<Combo>().buttonPressed = "7";
			}
			GUI.Box(new Rect(WIDTH/2 - sizeTime/2, HEIGHT/2 + sizeTexture * 2 + marge, sizeTime, sizeTexture/2), "TIME");
		}
	}
}
