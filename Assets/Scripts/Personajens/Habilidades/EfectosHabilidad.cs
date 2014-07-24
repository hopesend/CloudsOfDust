using UnityEngine;
using System.Collections;

[System.Serializable]
public class EfectosHabilidad {
    public TipoEfecto tipoEfecto;

    public TipoEfecto TipoEfecto
    {
        get { return tipoEfecto; }
        set { tipoEfecto = value; }
    }
    public int duracion;

    public int Duracion
    {
        get { return duracion; }
        set { duracion = value; }
    }
    public TipoObjetivoEfecto tipoObjEfecto;

    public TipoObjetivoEfecto TipoObjEfecto
    {
        get { return tipoObjEfecto; }
        set { tipoObjEfecto = value; }
    }
    public NombreAtributo statAfectado;

    public NombreAtributo StatAfectado
    {
        get { return statAfectado; }
        set { statAfectado = value; }
    }
    public float cantidadAfectada;

    public float CantidadAfectada
    {
        get { return cantidadAfectada; }
        set { cantidadAfectada = value; }
    }

    public TipoModificador modificador;

    public TipoModificador Modificador
    {
        get { return modificador; }
        set { modificador = value; }
    }

    public EstadosAlterados estadoAlterado;

    public EstadosAlterados EstadoAlterado
    {
        get { return estadoAlterado; }
        set { estadoAlterado = value; }
    }

    

}
