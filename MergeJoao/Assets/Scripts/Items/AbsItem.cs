using System;

public abstract class AbsItem
{
    private string nombre;

    public string Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }
    private string desc;

    public string Desc
    {
        get { return desc; }
        set { desc = value; }
    }
}
