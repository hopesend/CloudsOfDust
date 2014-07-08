using UnityEngine;
using System.Collections;

public class SaltoEscena : MonoBehaviour {

	public int nivel;

	void OnTriggerEnter (Collider other)
	{
		if (other.transform.tag == "Player")
		{
			ControladorGlobal.instanceRef.Lanzar_Pantalla(nivel);
		}
	}
}
