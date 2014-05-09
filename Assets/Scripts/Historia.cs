using UnityEngine;
using System.Collections;

public class Historia : MonoBehaviour {
	public int estado;
	public int inicio;

	// Use this for initialization
	void Start () {
		inicio = PlayerPrefs.GetInt ("inicio");
		if (inicio ==0){
			estado = 0;
			PlayerPrefs.SetInt("estadoHistoria",0);
		}else{
			estado = PlayerPrefs.GetInt ("estadoHistoria");
		}
	}
	
	// Update is called once per frame
	void Update () {
		/*switch (estado){
		case 0:{ //Inicio del Juego
			inicio = 1;
			PlayerPrefs.SetInt("inicio",1);
			estado = 1;
			break;
		}
		case 1:{	//Llega un nuevo mensaje

		}
		}*/
	}


}
