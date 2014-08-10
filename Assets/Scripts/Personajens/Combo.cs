using UnityEngine;
using System.Collections;

public class Combo : MonoBehaviour 
{
	private string[] listCombo; //lista del combo a seguir
	private int nextKey;
	
	public string buttonPressed; //tecla clickeada
	public string keyPressed; // tecla presionada

	private string buttonCombo;
	private string keyCombo;

	public bool inCombo;
	public float time;
	public float timeResponde;
	public float[] result; //return
	private float sizeTimeDiscount;
	private int SUE, PNT, EVA;

	private void Start() //Default
	{
		if(timeResponde == 0) timeResponde = 3;
		buttonPressed = "";
		keyPressed = "";
	}	
	public void StartCombo(int suerteAtacante, int punteriaAtacante, int evasionContrincante, string[] COMBO)
	{
		GetComponent<HUDCombo> ().inCombo = true;
		inCombo = true; //activa Combo
		listCombo = COMBO;
		nextKey = 0;
		sizeTimeDiscount = GetComponent<HUDCombo> ().sizeTime / timeResponde;
		SUE = suerteAtacante;
		PNT = punteriaAtacante;
		EVA = evasionContrincante;
		result = new float[COMBO.Length];
	}
	public float[] SucessCombo() { return result;} //funcion que retorna el porcentaje del combo
	private void KeyPressed() //retorna la tecla presionada
	{
		if(Input.GetKey(KeyCode.Q)) keyPressed = "Q";
		if(Input.GetKey(KeyCode.W)) keyPressed = "W";
		if(Input.GetKey(KeyCode.E)) keyPressed = "E";
		if(Input.GetKey(KeyCode.A)) keyPressed = "A";
		if(Input.GetKey(KeyCode.S)) keyPressed = "S";
		if(Input.GetKey(KeyCode.D)) keyPressed = "D";
		if(Input.GetKey(KeyCode.Z)) keyPressed = "Z";
		if(Input.GetKey(KeyCode.X)) keyPressed = "X";
		if(Input.GetKey(KeyCode.C)) keyPressed = "C";

		if(Input.GetKeyUp(KeyCode.Q)) keyPressed = "";
		if(Input.GetKeyUp(KeyCode.W)) keyPressed = "";
		if(Input.GetKeyUp(KeyCode.E)) keyPressed = "";
		if(Input.GetKeyUp(KeyCode.A)) keyPressed = "";
		if(Input.GetKeyUp(KeyCode.S)) keyPressed = "";
		if(Input.GetKeyUp(KeyCode.D)) keyPressed = "";
		if(Input.GetKeyUp(KeyCode.Z)) keyPressed = "";
		if(Input.GetKeyUp(KeyCode.X)) keyPressed = "";
		if(Input.GetKeyUp(KeyCode.C)) keyPressed = "";
	}
	void Update()
	{
		if(inCombo)
		{
			KeyPressed ();

			if(nextKey > listCombo.Length - 1) Reset();

			if((keyPressed != "") && (buttonPressed != ""))
			{
				if(listCombo[nextKey] == keyPressed+buttonPressed) //combinacion correcta
				{
					result[nextKey] = (1+(SUE/100)*(1+((PNT - EVA)/100)))*Random.Range(0.0f,1.0f);
					nextKey++;
				}
				else //combianacion incorrecta
				{
					nextKey++;
				}
				keyPressed = "";
				buttonPressed = "";
			}

			/////<>///////
			time += Time.deltaTime;
			if(time > timeResponde) //si el tiempo es mayor al tiempo disponible...
			{
				Reset();
			}
			else
			{
				GetComponent<HUDCombo>().sizeTime = sizeTimeDiscount * (timeResponde - Mathf.Round(time));
			}
		}
	}
	private void Reset()
	{
		time = 0; //reinciamos tiempo
		GetComponent<HUDCombo>().inCombo = false;
		inCombo = false;
	}


}
