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

	public void Start()
	{
		if(sizeTexure == 0) sizeTexure = 50;
		if(marge == 0) marge = 0.25f;
		HEIGHT = Screen.height;
		WIDTH = Screen.width;
	}
	public void OnGUI()
	{
        if(inCombo) //Muestra todos los botones
		{
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2, HEIGHT/2 - sizeTexure/2, sizeTexure, sizeTexure), imageCombo)) //S
			{
				GetComponent<Combo>().buttonPressed = "5";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2, HEIGHT/2 - sizeTexure/2 + (sizeTexure + marge), sizeTexure, sizeTexure), imageCombo)) //X
			{
				GetComponent<Combo>().buttonPressed = "2";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2, HEIGHT/2 - sizeTexure/2 - (sizeTexure + marge), sizeTexure, sizeTexure), imageCombo)) //W
			{
				GetComponent<Combo>().buttonPressed = "8";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2 - (sizeTexure + marge), HEIGHT/2 - sizeTexure/2, sizeTexure, sizeTexure), imageCombo)) //A
			{
				GetComponent<Combo>().buttonPressed = "4";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2 + (sizeTexure + marge), HEIGHT/2 - sizeTexure/2, sizeTexure, sizeTexure), imageCombo))//D
			{
				GetComponent<Combo>().buttonPressed = "6";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2 + (sizeTexure + marge), HEIGHT/2 - sizeTexure/2 + (sizeTexure + marge), sizeTexure, sizeTexure), imageCombo))//C
			{
				GetComponent<Combo>().buttonPressed = "3";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2 + (sizeTexure + marge), HEIGHT/2 - sizeTexure/2 - (sizeTexure + marge), sizeTexure, sizeTexure), imageCombo))//D
			{
				GetComponent<Combo>().buttonPressed = "9";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2 - (sizeTexure + marge), HEIGHT/2 - sizeTexure/2 + (sizeTexure + marge), sizeTexure, sizeTexure), imageCombo)) //Z
			{
				GetComponent<Combo>().buttonPressed = "1";
			}
			if(GUI.Button(new Rect(WIDTH/2 - sizeTexure/2 - (sizeTexure + marge), HEIGHT/2 - sizeTexure/2 - (sizeTexure + marge), sizeTexure, sizeTexure), imageCombo)) //Q
			{
				GetComponent<Combo>().buttonPressed = "7";
			}
		}
	}
}
