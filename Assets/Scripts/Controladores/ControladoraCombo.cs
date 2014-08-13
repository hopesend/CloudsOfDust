using UnityEngine;
using System.Collections;

[System.Serializable]
public class ControladoraCombo
{
    private Combo comboActual;
	private int nextKey;
	
	public string buttonPressed; //tecla clickeada
	public string keyPressed; // tecla presionada

	private string buttonCombo;
	private string keyCombo;

	public bool inCombo;
	public float time;
	public float[] result; //return
	private float sizeTimeDiscount;
	private int SUE, PNT, EVA;

	public void Start() //Default
	{
		buttonPressed = "";
		keyPressed = "";
	}	
	public void StartCombo(int suerteAtacante, int punteriaAtacante, int evasionContrincante, Combo comboHabilidad)
	{
		HUDBatalla.InstanceRef().hudCombo.InCombo = true;
		inCombo = true; //activa Combo
        comboActual = comboHabilidad;
		nextKey = 0;
		SUE = suerteAtacante;
		PNT = punteriaAtacante;
		EVA = evasionContrincante;
		result = new float[comboActual.ListCombo.Length];
        sizeTimeDiscount = HUDBatalla.InstanceRef().hudCombo.sizeTime / comboActual.TimeCombo;

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
	public void Update()
	{
		if(inCombo)
		{
			KeyPressed ();

            if (nextKey > comboActual.ListCombo.Length - 1)
            {
                Reset();
                ControladoraBaseBatalla.InstanceRef().faseActual = FasesBatalla.EjecutandoAccion;
            }
			else
			{
                HUDBatalla.InstanceRef().hudCombo.key = comboActual.ListCombo[nextKey][0].ToString();
                HUDBatalla.InstanceRef().hudCombo.button = comboActual.ListCombo[nextKey][1].ToString();
			}
			if((keyPressed != "") && (buttonPressed != ""))
			{
                if (comboActual.ListCombo[nextKey] == keyPressed + buttonPressed) //combinacion correcta
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
            if (time > comboActual.TimeCombo) //si el tiempo es mayor al tiempo disponible...
			{
				Reset();
			}
			else
			{
                HUDBatalla.InstanceRef().hudCombo.sizeTime = sizeTimeDiscount * (comboActual.TimeCombo - Mathf.Round(time));
			}
		}
	}
	private void Reset()
	{
		time = 0; //reinciamos tiempo
        HUDBatalla.InstanceRef().hudCombo.InCombo = false;
		inCombo = false;
	}


}
