using UnityEngine;
using System.Collections;

public class animPuerta : MonoBehaviour {

	public Animator anim;
	bool abrir;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		anim.SetBool ("abrir", abrir);
	}

	void OnTriggerEnter (){
		abrir = true;
	}

	void OnTriggerExit (){
		abrir = false;
	}
}
