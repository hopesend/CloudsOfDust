﻿using UnityEngine;
using System.Collections;

public class SaltoEscena : MonoBehaviour {

	public int nivel;

	// Use this for initialization
	void Start () {

	}
	void OnTriggerEnter (Collider other){
		Application.LoadLevel(nivel);
	}
	// Update is called once per frame
	void Update () {

	}
}
