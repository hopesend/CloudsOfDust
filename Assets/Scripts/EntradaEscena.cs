using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class EntradaEscena : MonoBehaviour {

	//public int escenaAnterior;
	public int escenaActual;

	 //0 si no ha empezado, 1 si ha empezado
	public Vector3[] entrada;

	// Use this for initialization
	void Start () {
		/*inicio = PlayerPrefs.GetInt("inicio");
		if (inicio!=0){
			escenaAnterior=PlayerPrefs.GetInt("escena");
		}else{
			escenaAnterior=0;
			PlayerPrefs.SetInt ("inicio",1);
			inicio =1;
		}

		escenaActual = Application.loadedLevel;
		PlayerPrefs.SetInt("escena",escenaActual);

		entrada = new Vector3[5,5];

		entrada[0,0] = new Vector3(-5,0.23f,-25);
		entrada[1,0]= new Vector3(-0.8f,0.23f,-17.5f);
		entrada[0,1]= new Vector3(262,0.23f,81);
		entrada[2,1]= new Vector3(270,0.23f,33);
		entrada[1,2]= new Vector3(-55,0.1f,2);
		entrada[3,2]= new Vector3(-1,0.1f,62);
		entrada[2,3] = new Vector3(-26,0.23f,69);

		if (inicio!=0){
			transform.position = entrada[escenaAnterior,escenaActual];
		}*/


		//Vector3.Lerp(gameObject.transform.position,entrada[escenaAnterior,escenaActual],0);

		entrada = new Vector3[3];
		entrada[1] = new Vector3(173,0,2);
		entrada[2] = new Vector3(-6,0.8f,6);

		escenaActual = Application.loadedLevel;
		GameObject.FindGameObjectWithTag("Player").transform.position = entrada[escenaActual];
	
		/*switch (escenaActual){
		case 0:{	//escena casa
			switch (escenaAnterior){
			case 1:{	//viene del vecindario
				transform.position = entrada[escenaAnterior,escenaActual];
				break;
			}
			default:{
				transform.position = entrada[0,0];
				break;
			}
			}
			break;		
		}
		case 1:{	//escena vecindario
			switch (escenaAnterior){
			case 0:{ // viene de Escena Casa
				transform.position = entrada[escenaAnterior,escenaActual];
				break;
			}
			case 2:{ //viene de la estacion
				transform.position = entrada[escenaAnterior,escenaActual];
				break;
			}
			default:{
				transform.position = entrada[0,1];
				break;
			}
			}
			break;
		}
		case 2:{
			switch (escenaAnterior){
			case 1:{
				transform.position = entrada[1,2];
				break;
			}
			case 3:{
				transform.position = entrada[3,2];
				break;
			}
			default:{
				transform.position = entrada[1,2];
				break;
			}
			}
			break;

		}
		
		case 3:{
			transform.position = entrada[2,3];
			break;
		}
	}*/
	}

	/*void OnGUI (){
		if (GUI.Button (new Rect(0,0,100,100),"Reset")){
			inicio = 0;
			PlayerPrefs.SetInt ("inicio",0);
		}
	}*/

	
	// Update is called once per frame


	void Iniciar(){

	}

    
}
