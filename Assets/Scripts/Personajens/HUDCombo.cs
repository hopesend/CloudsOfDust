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

	public int sizeTexure;
	public float marge;
	public bool inCombo;
	private bool onPressed;

	private float HEIGHT;
	private float WIDTH;

	public string keyPressed;

	void Start()
	{
		if(sizeTexure == 0) sizeTexure = 50;
		if(marge == 0) marge = 0.25f;
		HEIGHT = Screen.height;
		WIDTH = Screen.width;
	}
	void OnGUI()
	{
        if(inCombo) //Muestra todos los botones
		{
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2, HEIGHT/2 - sizeTexure/2, sizeTexure, sizeTexure), imageCombo)) //S
			{
				onPressed = true;
				keyPressed = "S";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2, HEIGHT/2 - sizeTexure/2 + (sizeTexure + marge), sizeTexure, sizeTexure), imageCombo)) //X
			{
				onPressed = true;
				keyPressed = "X";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2, HEIGHT/2 - sizeTexure/2 - (sizeTexure + marge), sizeTexure, sizeTexure), imageCombo)) //W
			{
				onPressed = true;
				keyPressed = "W";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2 - (sizeTexure + marge), HEIGHT/2 - sizeTexure/2, sizeTexure, sizeTexure), imageCombo)) //A
			{
				onPressed = true;
				keyPressed = "A";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2 + (sizeTexure + marge), HEIGHT/2 - sizeTexure/2, sizeTexure, sizeTexure), imageCombo))//D
			{
				onPressed = true;
				keyPressed = "D";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2 + (sizeTexure + marge), HEIGHT/2 - sizeTexure/2 + (sizeTexure + marge), sizeTexure, sizeTexure), imageCombo))//D
			{
				onPressed = true;
				keyPressed = "C";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2 + (sizeTexure + marge), HEIGHT/2 - sizeTexure/2 - (sizeTexure + marge), sizeTexure, sizeTexure), imageCombo))//D
			{
				onPressed = true;
				keyPressed = "E";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2 - (sizeTexure + marge), HEIGHT/2 - sizeTexure/2 + (sizeTexure + marge), sizeTexure, sizeTexure), imageCombo)) //Z
			{
				onPressed = true;
				keyPressed = "Z";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2 - (sizeTexure + marge), HEIGHT/2 - sizeTexure/2 - (sizeTexure + marge), sizeTexure, sizeTexure), imageCombo)) //Q
			{
				onPressed = true;
				keyPressed = "Q";
			}
		}
	}
}
