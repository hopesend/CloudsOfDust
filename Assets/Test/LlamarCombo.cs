using UnityEngine;
using System.Collections;

public class LlamarCombo : MonoBehaviour //Test de como llamar a la clase combo
{
	public int SUERTE;
	public int EVASION;
	public int PUNTERIA;
	public bool active;
	public string[] COMBO;


	// Use this for initialization
	void Start () 
	{
		if(active) GetComponent<Combo>().StartCombo(SUERTE, EVASION, PUNTERIA, COMBO); //LLama a combo (Test)//Se es llamada asi desde la clase HABILIDAD
	}
}
