using UnityEngine;
using System.Collections;

public class AnimPuerta : MonoBehaviour {

	public AudioClip doorSwishClip;
	public Animator anim;



	// Use this for initialization
	void Start () {

	}

	void OnTriggerEnter (Collider other){
		anim.SetBool("Open",true);
	
	}
	void OnTriggerStay (Collider other){
		anim.SetBool("Open",true);
	}
	void OnTriggerExit (Collider other){
		anim.SetBool("Open",false);
	}

	// Update is called once per frame
	void Update () {

	}
}
