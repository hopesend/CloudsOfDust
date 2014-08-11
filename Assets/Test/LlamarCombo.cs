using UnityEngine;
using System.Collections;

public class LlamarCombo : MonoBehaviour //Test de como llamar a la clase combo
{
	public int suerte;
	public int evasion;
	public int punteria;
	public float time;
	public bool active;
	public string[] combo;


	// Use this for initialization
	void Start () 
	{
		if(active) GetComponent<Combo>().StartCombo(suerte, evasion, punteria, combo, time); //LLama a combo (Test)//Se es llamada asi desde la clase HABILIDAD
	}
}
