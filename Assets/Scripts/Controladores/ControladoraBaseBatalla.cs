using UnityEngine;
using System.Collections;

public class ControladoraBaseBatalla : MonoBehaviour {

    [SerializeField]
    public ControladoraTurno controladoraTurno;

    public static ControladoraBaseBatalla instanceRef;

    void Awake()
    {
        instanceRef = this;
        controladoraTurno = new ControladoraTurno();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        controladoraTurno.Update();
	}
}
