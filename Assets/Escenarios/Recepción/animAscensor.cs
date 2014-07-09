using UnityEngine;
using System.Collections;

public class animAscensor : MonoBehaviour {

	public Animator ascensor;

	bool animacion;
	int t;
	// Use this for initialization
	void Start () {
		t = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (ascensor.GetCurrentAnimatorStateInfo (0).IsName ("AscensorArriba")) 
		{
			ControladorGlobal.instanceRef.Lanzar_Pantalla(6);
		}
	}

	void OnTriggerEnter (Collider other){
		if (other.transform.tag == "Player") {
			ascensor.SetBool ("subir",true);
			animacion = true;

		}
	}
}
