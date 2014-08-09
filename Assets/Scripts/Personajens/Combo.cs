using UnityEngine;
using System.Collections;

public class Combo : MonoBehaviour 
{
	private string[] listCombo; //lista del combo a seguir
	private int nextKey;
	private string keyCombo;  //tecla a presionar de la lista
	
	public string buttonPressed; //tecla clickeada
	public string keyPressed; // tecla presionada

	public bool inCombo;
	public float time;
	public float timeResponde;
	private float[] percentage = {100}; //test return 100%
	private HUDCombo HUDCombo; //declaramos el hud

	private void Start() //Default
	{
		if(timeResponde == 0) timeResponde = 3;
		buttonPressed = "";
		keyPressed = "";
	}	
	public void StartCombo(int SUERTE, int PUNTERIA, int EVASION, string[] COMBO)
	{
		GetComponent<HUDCombo> ().inCombo = true;
		inCombo = true; //activa Combo
		listCombo = COMBO;
	}
	public float[] SucessCombo() //funcion que retorna el porcentaje del combo
	{

		return percentage;
	}
	private void KeyPressed() //retorna la tecla presionada
	{
		Event e = new Event ();

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
		if((keyPressed != "") && (buttonPressed != ""))
		{
			Debug.Log(keyPressed + buttonPressed);
			keyPressed = "";
			buttonPressed = "";
		}

		if(inCombo)
		{
			time += Time.deltaTime;
			if(time < timeResponde) //si el tiempo es menor al tiempo disponible...
			{
			}
			else
			{
				time = 0; //reinciamos tiempo
				nextKey++;
			}
		}
		KeyPressed ();
	}


}
