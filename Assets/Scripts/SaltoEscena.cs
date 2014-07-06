using UnityEngine;
using System.Collections;

public class SaltoEscena : MonoBehaviour {

	private ControladorNiveles manager;
	public string nivel;

	// Use this for initialization
	void Start () {

	}
	void OnTriggerEnter (Collider other){
		if (other.transform.tag == "Player"){
			ControladorNiveles.instanceRef.CambiarEstado (new EscenarioVecindario(manager));

		}
	}
	// Update is called once per frame
	void Update () {

	}
}
