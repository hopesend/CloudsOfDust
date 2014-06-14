using UnityEngine;
using System.Collections.Generic;
using Pathfinding;

public class PersonajeControlable : PersonajeBase {

    public Equipamento equipamento;
    


    public Equipamento Equipamento
    {
        get { return equipamento; }
        set { equipamento = value; }
    }

    public override void Awake(){

        ControladorJugador.instanceRef.Trasher = this;
        ControladorJugador.instanceRef.Cargar_Datos_XML(Get_Nombre());

        equipamento = new Equipamento();
    }

    public override void Update()
    {
        
    }



    

}
