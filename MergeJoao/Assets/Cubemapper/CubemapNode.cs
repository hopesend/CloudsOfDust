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

public class CubemapNode : MonoBehaviour
{
	public bool allowGeneratePNG = true; // allows PNG generation on this node
	public bool allowCubemapGeneration = true; // allows Cubemap generation on this node
	public bool allowAssign = true; // will update this node on one-click building
	public bool overrideResolution = false; // can override resolution picked on cubemap manager
	public int resolution = 32; // the resolution for this node
	
	public Cubemap cubemap; // stores an corresponding cubemap in this variable for access by other scripts
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "cNode.png"); 
	}
}