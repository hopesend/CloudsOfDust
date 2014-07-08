using UnityEngine;
using System.Collections;
using Pathfinding;

public class LevelerLowPath : MonoBehaviour {


	public Vector3 targetPosition;
	private Seeker seeker;
	private CharacterController controller;
	public Path path;

	public float speed;

	public float nextWaypointDistance = 10;

	private int currentWaypoint = 0;
	// Use this for initialization
	void Start () {
		targetPosition = GameObject.Find ("Target").transform.position;
		seeker = GetComponent<Seeker>();
		controller = GetComponent<CharacterController>();

		//Set path
		//seeker.StartPath(transform.position, targetPosition, OnPathComplete(path));
		seeker.StartPath(transform.position,targetPosition, OnPathComplete);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnPathComplete(Path p){
		if (!p.error){
			path = p;
			//Reset waypoint counter
			currentWaypoint = 0;
		}
	}

	public void FixedUpdate(){
		if(path == null){
			return;
		}
		if(currentWaypoint >= path.vectorPath.Count){
			Vector3 dir = (path.vectorPath[currentWaypoint] = transform.position).normalized;
			dir *= speed * Time.fixedDeltaTime;
			controller.SimpleMove (dir); //Unit moves here!

			//Check if close enough to the courrent waypoint, if we are, proceed to next waypoint
			if (Vector3.Distance (transform.position, path.vectorPath[currentWaypoint])<nextWaypointDistance) {
				currentWaypoint++;
				return;
			}

		}
	}
}
