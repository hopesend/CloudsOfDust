using UnityEngine;
using System.Collections;

public class LlamarCombo : MonoBehaviour //Test de como llamar a la clase combo
{
	public bool active;
	public string[] COMBO;
	// Use this for initialization
	void Start () 
	{
		if(active) GetComponent<Combo>().StartCombo(15, 30, 29, COMBO); //LLama a combo (Test)//Se es llamada asi desde la clase HABILIDAD
	}
}
