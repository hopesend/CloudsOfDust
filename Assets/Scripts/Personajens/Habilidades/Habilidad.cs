using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Habilidad {
    public string nombre;

    public string Nombre
    {
        get { return nombre; }
        set { nombre = value; }
    }
    public string descripcion;


    public string Descripcion
    {
        get { return descripcion; }
        set { descripcion = value; }
    }
    public float costoPM;

    public float CostoPM
    {
        get { return costoPM; }
        set { costoPM = value; }
    }
    float costoPA;

    public float CostoPA
    {
        get { return costoPA; }
        set { costoPA = value; }
    }
    float costoEst;

    public float CostoEst
    {
        get { return costoEst; }
        set { costoEst = value; }
    }
    float rango;

    public float Rango
    {
        get { return rango; }
        set { rango = value; }
    }
    float probabilidad;

    public float Probabilidad
    {
        get { return probabilidad; }
        set { probabilidad = value; }
    }
    TipoHabilidad tipo;

    public TipoHabilidad Tipo
    {
        get { return tipo; }
        set { tipo = value; }
    }

    public List<EfectosHabilidad> efectos = new List<EfectosHabilidad>();

    public void AgregarEfecto(EfectosHabilidad nuevo)
    {
        if (nuevo != null)
        {
            efectos.Add(nuevo);
        }
    }

    public void EjecutarMovimiento(PersonajeBase caster)
    {
        if (this.tipo == TipoHabilidad.Movimiento)
        {
            caster.Estamina.ValorActual -= costoEst;
            caster.PM.ValorActual -= costoPM;
            if (this.efectos.Count > 0)
            {
                foreach (EfectosHabilidad efecto in efectos)
                {
                    caster.AgregarAtributoAlterado(new AtributosAlterados(efecto.Duracion, efecto.StatAfectado, efecto.cantidadAfectada, efecto.Modificador));
                }
            }

        }
    }
}
