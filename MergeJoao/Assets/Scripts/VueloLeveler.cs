using UnityEngine;
using System.Collections;

public class VueloLeveler : MonoBehaviour {

	public GameObject target;
	public float speed;
	public Vector3 inicio;
	public float t;

	// Use this for initialization
	void Start () {
		inicio = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance(transform.position,target.transform.position)< 10f){
			t += Time.deltaTime;
			if (t>2f){
				transform.position = inicio;
				t = 0;
			}
		}else{
			//transform.LookAt(target.transform.position);
			Vector3.Lerp(transform.position, target.transform.position, speed*Time.deltaTime);
		}
	}
}
