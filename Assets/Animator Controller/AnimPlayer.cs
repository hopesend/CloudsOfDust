using UnityEngine;
using System.Collections;

public class AnimPlayer : MonoBehaviour {

	public Animator _animplayer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W)||Input.GetKey (KeyCode.A)||Input.GetKey(KeyCode.S)||Input.GetKey (KeyCode.D)){
			_animplayer.SetBool("mover",true);
			if (Input.GetKey (KeyCode.LeftShift)){
				_animplayer.SetBool("mayus",true);
			}else
			{
				_animplayer.SetBool("mayus",false);
			}
		}else
		{
			_animplayer.SetBool("mover",false);
			_animplayer.SetBool("mayus",false);
		}


	}
}
