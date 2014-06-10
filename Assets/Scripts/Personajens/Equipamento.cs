using UnityEngine;
using System.Collections;

public class Equipamento {

    public Equipamento()
    {
        accesorio = new Accesorio();
    }

	public Accesorio accesorio;

    public void CambiarAccesorio(Accesorio nuevo)
    {
        if (nuevo != null)
        {
            accesorio = nuevo;
        }
    }


}
