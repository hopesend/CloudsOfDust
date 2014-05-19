using UnityEngine;
using System.Collections;

public class AnimLeveler : MonoBehaviour {
	
	public Animator anim;
	public GameObject leveler;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		/*if (leveler.GetComponent<Animator>().animation == "Aterrizado"){
			animacion= false;
			Camera.main.enabled = !animacion;
			GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterController>().enabled = !animacion;
			Destroy (gameObject);
		}*/
		//Debug.Log (""+anim.GetCurrentAnimatorStateInfo (0));
	}

	void OnTriggerEnter(){


		//GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterController>().enabled = !animacion;
		leveler.SetActive(true);

		anim.SetBool("llamar",true);

	}
	

}
