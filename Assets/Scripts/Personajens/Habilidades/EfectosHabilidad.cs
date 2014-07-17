using UnityEngine;
using System.Collections;

public class EfectosHabilidad {
    TipoEfecto tipo;

    public TipoEfecto Tipo
    {
        get { return tipo; }
        set { tipo = value; }
    }
    int duracion;

    public int Duracion
    {
        get { return duracion; }
        set { duracion = value; }
    }
    TipoObjetivoEfecto tipoObjEfecto;

    public TipoObjetivoEfecto TipoObjEfecto
    {
        get { return tipoObjEfecto; }
        set { tipoObjEfecto = value; }
    }
    PersonajeBase.parametro statAfectado;

    public PersonajeBase.parametro StatAfectado
    {
        get { return statAfectado; }
        set { statAfectado = value; }
    }
    float cantidadAfectada;

    public float CantidadAfectada
    {
        get { return cantidadAfectada; }
        set { cantidadAfectada = value; }
    }

}
