using UnityEngine;
using System.Collections;

public class Equipamento {

    public IArma armaActual;

    public IArma ArmaActual
    {
        get { return armaActual; }
    }

    public Equipamento()
    {
        accesorio = new Accesorio();
        armaActual = new ArmaMele();
    }

	public Accesorio accesorio;

    public void CambiarAccesorio(Accesorio nuevo)
    {
        if (nuevo != null)
        {
            accesorio = nuevo;
        }
    }

    public void UpdateArmaPrincipal()
    {
    }




}
