using UnityEngine;
using System.Collections;

public class LlamarCombo : MonoBehaviour //Test de como llamar a la clase combo
{
	public bool active;
	// Use this for initialization
	void Start () 
	{
		if(active) GetComponent<Combo>().StartCombo(0, 0, 0, new string[]{"A1", "W8", "E3", "Q3"}); //LLama a combo (Test)//Se es llamada asi desde la clase HABILIDAD
	}
}
