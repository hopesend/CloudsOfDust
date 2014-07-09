using UnityEngine;
using System.Collections;

public class EscenarioBatallaTutorial : IControlador {

    private ControladorNiveles manager;



    public EscenarioBatallaTutorial(ControladorNiveles managerRef)
    {
        manager = managerRef;

        if (Application.loadedLevelName != ScenesParaCambio.BatallaTutorial.ToString())
        {
            Application.LoadLevel(ScenesParaCambio.BatallaTutorial.ToString());
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
