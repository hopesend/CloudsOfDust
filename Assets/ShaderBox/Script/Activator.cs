using UnityEngine;
using System.Collections;

public class Activator : MonoBehaviour {

    public GameObject[] super_parents;

    public bool autorun = true;
	public bool turn_on = false;

    bool on = false;
	void Start() 
    {
        if (autorun)
        {
			if(turn_on)
			{
				TurnOn();
			}
			else
			{
            	TurnOff();
			}
        }
	}

    public void TurnOn()
    {
        for (int i = 0; i < super_parents.Length; i++)
        {
            super_parents[i].SetActiveRecursively(true);
        }
    }

    public void TurnOff()
    {
        for (int i = 0; i < super_parents.Length; i++)
        {
            super_parents[i].SetActiveRecursively(false);
        }
    }
}
