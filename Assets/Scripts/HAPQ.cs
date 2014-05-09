using UnityEngine;
using System.Collections;

public class HAPQ : MonoBehaviour {
	//int estado;
	public bool menu;
	// Use this for initialization
	void Start () {
		menu = false;

	}
	
	// Update is called once per frame
	void Update () {
		//estado = gameObject.GetComponent<Historia>().estado; //esto es para la cuando estemos programando la historia.
		gameObject.GetComponent<CharacterController>().enabled = !menu; //desactiva el movimiento del personaje cuando este activo el menu
		gameObject.GetComponent<AnimPlayer>().enabled = !menu; //desactiva la animacion del personaje cuando este activo el menu
		Camera.main.GetComponent<MouseOrbitImproved>().enabled = !menu; //desactiva el movimiento de la camara
		if (Input.GetKeyDown(KeyCode.Tab)){	
			menu = !menu;
		}

	}

	void OnGUI(){
		Rect M = new Rect(0,0,Screen.width,Screen.height);
		Rect mensajes = new Rect(M.width/2,0,M.width/2,100);
		if (menu){
			GUI.Box (M,"");
			GUI.Button (mensajes,"Mensajes");
		}

	}


}
