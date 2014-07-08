using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    Ray rayPrueba;
    RaycastHit hit = new RaycastHit();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        rayPrueba = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Input.GetMouseButtonUp(1))
        {
            if (Physics.Raycast(rayPrueba, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Arena")
                {
                    
                    //ControladorJugador.instanceRef.OrdenarMover(ControladorJugador.instanceRef.trasher, hit.point);
                }
            }
        }

	}
}
