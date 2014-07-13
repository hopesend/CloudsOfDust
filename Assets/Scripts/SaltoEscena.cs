using UnityEngine;
using System.Collections;

public class SaltoEscena : MonoBehaviour {

    public ScenesParaCambio cambiarA;

	void OnTriggerEnter (Collider other)
	{
		if (other.transform.tag == "Player")
		{
            GameMaster.instanceRef.controladoraNiveles.CambiarSceneSegunEnum(cambiarA);
		}
	}
}
