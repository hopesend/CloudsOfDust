using UnityEngine;
using System.Collections;

public class AnimLeveler : MonoBehaviour {

	public bool llamar;
	public Animator anim;
	bool inTrigger;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (inTrigger && Input.GetKeyDown (KeyCode.E)){
			llamar = !llamar;
		}
		anim.SetBool ("llamar",llamar);
	}

	void OnTriggerEnter(){
		inTrigger = true ;
	}
	
	void OnTriggerExit(){
		inTrigger = false ;
	}
}
