using UnityEngine;
using System.Collections.Generic;

public class PersonajeControlable : PersonajeBase {

    public Equipamento equipamento;
    private List<EstadosAlterados> estadosAlterados = new List<EstadosAlterados>();

    public Equipamento Equipamento
    {
        get { return equipamento; }
        set { equipamento = value; }
    }

    public override void Awake(){
        equipamento = new Equipamento();
    }

    public bool AplicarEstadoAlterado(EstadosAlterados estado)
    {
        if (estadosAlterados.Contains(estado))
        {
            return false;
        }
        else
        {
            estadosAlterados.Add(estado);
        }
        return false;
    }

    public bool CurarEstadoAlterado(EstadosAlterados estadoCurar)
    {
        return estadosAlterados.Remove(estadoCurar);
    }

}
