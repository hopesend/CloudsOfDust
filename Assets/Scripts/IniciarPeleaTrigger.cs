using UnityEngine;
using System.Collections;

public class IniciarPeleaTrigger : MonoBehaviour {


    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            GameMaster.instanceRef.controladoraNiveles.IrSceneBatallaTutorial();
        }
        
    }
}
