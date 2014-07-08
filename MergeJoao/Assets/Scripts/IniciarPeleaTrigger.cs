using UnityEngine;
using System.Collections;

public class IniciarPeleaTrigger : MonoBehaviour {


    void OnTriggerEnter(Collider col)
    {
        GameMaster.instanceRef.controladoraNiveles.IrSceneBatallaTutorial();
    }
}
