/*------------------------------------------------
// CUBEMAPPER Version 1.4.2
// Created by: Rainer Liessem
// Website: http://www.spreadcamp.com
//
// PLEASE RESPECT THE LICENSE TERMS THAT YOU
// AGREED UPON WITH YOUR PURCHASE OF THIS ASSET
------------------------------------------------*/
using UnityEngine;
using System.Collections;

public class CubemapUser : MonoBehaviour
{
	public bool startupSwap = false;
	public bool realtimeSwitching = false; // this user will always use the nearest node to fetch a cubemap
	[HideInInspector] public bool softBlending = false; // TODO mark as SLOW, because it is	
	[HideInInspector] public float fadeTime = 0.5f;
	
	// PRIVATE MEMBERS
	bool isAlive = false;
	CubemapNode[] nodes = null; // stores a reference to all the nodes in the scene
	
	CubemapNode _currentNode = null;
	CubemapNode _nearestNode = null;
	
	/*
	// TODO Storing of cubemaps used in soft blending switch
	Cubemap _curCubemap;
	Cubemap _targetCubemap;
	Cubemap _tmpCubemap;
	*/
	
	void Start()
	{
		// Verify and abort if no Material on this object supports Cubemaps
		VerifyShaderCubemapSupport();
		
		// Store reference to nodes, abort if none present
		GetNodes();
		
		// If required we will first assign the nearest node's cubemap and set it as current
		if(realtimeSwitching || startupSwap)
		{
			_currentNode = FindNearestNode();
			StartCoroutine(SetCubemap(_currentNode));
		}
		
		// Start processing cubemaps via co-routine if we are supposed to do real-time switching
		if(realtimeSwitching)
		{
			isAlive = true;
			StartCoroutine("WatchEnviroment");
		}
	}
		
	
	IEnumerator WatchEnviroment()
	{
		// If we are supposed to switch...
		while(isAlive)
		{			
			_nearestNode = FindNearestNode();
			
			// Nearest Node differs from current node
			if(_nearestNode != _currentNode)
			{
				// Make nearest node our new current
				_currentNode = _nearestNode;
				
				// Assign Cubemap for our Node
				yield return StartCoroutine(SetCubemap(_currentNode));
			}
			
			yield return null;
		}	
		
		yield return null;
	}

	CubemapNode FindNearestNode()
	{		
		CubemapNode cn = null;		
		float minDist = Mathf.Infinity;
		Vector3 pos = transform.position;		
		
		// Find nearest node
		for(int i=0; i<nodes.Length; i++)
		{
			float dist = Vector3.Distance(nodes[i].gameObject.transform.position, pos);
			
			if(dist < minDist)
			{
				cn = nodes[i];
				minDist = dist;
			}
		}
		
		//Debug.Log("Nearest Node: " + cn.name + "(Distance: " + minDist + ")");
		return cn;
	}
	
	// Stores a reference to all nodes in the scene
	// Also kills the script if there are no nodes
	void GetNodes()
	{
		nodes = FindObjectsOfType( typeof(CubemapNode) ) as CubemapNode[];
		//Debug.Log(nodes.Length + " | " + nodes);
		
		// Kill this script if there are no nodes found anywhere in our scene
		if(nodes.Length == 0)
		{
			Debug.LogError("There are no cubemap nodes in your scene! Please add one or more cubemap nodes to your scene.");
			Destroy(gameObject.GetComponent<CubemapUser>());
		}
	}

	
	// Switch Cubemap based on Node parameter
	IEnumerator SetCubemap(CubemapNode targetNode)
	{
		Cubemap c = targetNode.cubemap;
		
		// Don't proceed if there is no cubemap
		if(c == null) Debug.LogError("Failed to assign Cubemap to \" " + gameObject.name + " \" because Node " + targetNode.name + " seems to have not stored a corresponding Cubemap in cubemap variable.");
		else
		{
			// Instant switching
			SetCubemapToMaterials(c);
			Debug.Log("Assigned Cubemap " + c.name + " successfully to \"" + gameObject.name + "\" Shader!");
			
			/*
			// TODO Soft-blend cubemap over time
			if(softBlending)
			{
				_targetCubemap = c;
				
				// Get the base reflection color to go back to after fade
				Color colorOriginal = renderer.sharedMaterial.GetColor("_ReflectColor");
			
				// Counter
				float elapsedTime = 0f;
				float startTime = Time.time;
				bool isCounting = false;
				
				isCounting = true;
				while(isCounting)
				{
					elapsedTime = Time.time - startTime;
					
					if(elapsedTime < fadeTime)
					{
						// TODO Fade
						Debug.Log(elapsedTime + " - still fading...");
					}
					// Stop Timer and assign final texture
					else
					{
						SetCubemapToMaterials(c);
						isCounting = false;
					}
					
					yield return null;
				}
				
				//Debug.Log("Timer expired, outside Loop!"); 
				
				// After Fade set target to current
				_curCubemap = _targetCubemap;
			}
			// Instant switching
			else
			{
				SetCubemapToMaterials(c);
				Debug.Log("Assigned Cubemap " + c.name + " successfully to \"" + gameObject.name + "\" Shader!");
			}
			*/
		}
		
		yield return null;
	}
	
	
	// Verifies that Cubemaps are applicable to at least one Material found on this game object
	void VerifyShaderCubemapSupport()
	{
		Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
		
		bool hasCubemapSupport = false; // this will set true in the loop later eventually, if it doesn't we know there's no cubemap support anywhere on this object
		
		if(renderers != null)
		{
		    // Iterate through all renderers
		    foreach(Renderer r in renderers)
		    {
		         foreach(Material mat in r.materials)
		         {
					if(mat.HasProperty("_Cube")) hasCubemapSupport = true;
		         }
		    }
			
			// If it turns out we don't have any material with cubemap support, we delete this component
			if(!hasCubemapSupport)
			{
				Debug.LogError(gameObject.name + " is a Cubemap User, but no Materials have a _Cube property for attaching Cubemaps!");
				Destroy(gameObject.GetComponent<CubemapUser>());
			}			
		} else {
			Debug.LogError("No renderer found for object" + gameObject.name.ToString());
			Destroy(gameObject.GetComponent<CubemapUser>());
		}
	}
	
	// Goes unconditionally through all Materials and Children to attach Cubemap where it can
	void SetCubemapToMaterials(Cubemap c)
	{
		Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
		
		if(renderers != null)
		{
		    // Iterate through all renderers
		    foreach(Renderer r in renderers)
		    {
		         foreach(Material mat in r.materials)
		         {
					//Debug.Log(gameObject.name + " set cubemap " + c.name);
					
					if(mat.HasProperty("_Cube"))
					{
						mat.SetTexture("_Cube", c);
						//_curCubemap = _targetCubemap = c;
						//Debug.Log(gameObject.name + " set cubemap " + c.name);
					}
		         }
		    }
		}
	}
}