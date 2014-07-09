using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DBPersonajens {

    public DBPersonajens()
    {
    }


    public DataDBPersonaje[] lista;

    public DataDBPersonaje GetPersonajeByID(IDPersonajes nombre)
    {
        for (int i = 0; i < lista.Length; i++)
        {
            if (lista[i].ID == nombre)
            {
				
                return lista[i];
            }
        }
        return null;
    }

    void IniciarlizarLista()
    {

    }
}
