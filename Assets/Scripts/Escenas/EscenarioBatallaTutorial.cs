using UnityEngine;
using System.Collections;

public class EscenarioBatallaTutorial : IControlador {

    private ControladorNiveles manager;



    public EscenarioBatallaTutorial(ControladorNiveles managerRef)
    {
        manager = managerRef;

        if (Application.loadedLevelName != "BatallaTutorial")
        {
            Application.LoadLevel("BatallaTutorial");
        }
    }

    public void EstadoUpdate()
    {
        
    }

    public void Mostrar()
    {
        
    }

    public void NivelCargado()
    {
        GameMaster.instanceRef.IniciarPelea();
    }



}
