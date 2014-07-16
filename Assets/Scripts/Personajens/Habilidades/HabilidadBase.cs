using UnityEngine;
using System.Collections;

public abstract class HabilidadBase {
    private int id;

    public int ID
    {
        get { return id; }
        set { id = value; }
    }


    private string nombre;

    public string Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }
}
