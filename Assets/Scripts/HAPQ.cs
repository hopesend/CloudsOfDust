using UnityEngine;
using System.Collections;

public class HAPQ : MonoBehaviour {
	//int estado;
	public bool menu;
	public GUISkin mySkin;
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
		Rect R_M = new Rect(0,0,Screen.width,Screen.height);
		Rect[] R_der = new Rect[8];
		string[] apartados = new string[8]{"Mensajes","Inventario","Equipo","Desarrollo","Estrategia","Condicion","Base de Datos","Opciones"};
		if (menu){
			GUI.Box (R_M,"");
			for (int i = 0;i<8;i++){
				R_der[i] = new Rect(Screen.width/2,i*Screen.height/8,Screen.width/2,Screen.height/8);
				GUI.Button(R_der[i],apartados[i]);
			}
		}


	}


}
