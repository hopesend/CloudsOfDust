using UnityEngine;
using System.Collections;

public class ControllerSwitcher : MonoBehaviour 
{
    public GameObject controller1;
    public GameObject controller2;

	void Update () 
    {
	    if(Input.GetKeyDown(KeyCode.F2))
        {
            if (controller1.active)
            {
                controller1.SetActiveRecursively(false);
                controller2.SetActiveRecursively(true);
            }
            else
            {
                controller1.SetActiveRecursively(true);
                controller2.SetActiveRecursively(false);
            }
        }
	}
}
