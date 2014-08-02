using UnityEngine;
using System.Collections;

/// <summary>
/// Encargada de animar los ascensores en el juego.
/// </summary>
public class animAscensor : MonoBehaviour {

    /// <summary>
    /// Cache de Animator
    /// </summary>
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
            GameMaster.InstanceRef.controladoraNiveles.IrSceneSalaActualizacion();
		}
	}

	void OnTriggerEnter (Collider other){
		if (other.transform.tag == "Player") {
			ascensor.SetBool ("subir",true);
			animacion = true;

		}
	}
}
