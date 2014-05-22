using UnityEngine;
using System.Collections;

public class MenuPrincipal : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		Rect menu = new Rect(Screen.width/2-50,Screen.height/2-80,100,400);
		if (GUI.Button(new Rect(menu.x, menu.y, 100,30), "Nuevo Juego")){
			Application.LoadLevel(1);
		}
		GUI.Button(new Rect(menu.x, menu.y+40, 100,30), "Cargar");
		GUI.Button(new Rect(menu.x, menu.y+80, 100,30), "Opciones");
		if(GUI.Button(new Rect(menu.x, menu.y+120, 100,30), "Salir")){
			Application.Quit();
		}
	}
}
