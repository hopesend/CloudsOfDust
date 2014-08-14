using UnityEngine;
using System.Collections.Generic;

public class ControladoraHabilidades {

    public PersonajeBase caster;

    public List<PersonajeBase> targets;

    public PersonajeBase target;

    public Habilidad habilidadUsada;


    public void CargarDatos(PersonajeBase _caster, List<PersonajeBase> _targets, Habilidad _habilidad)
    {
        caster = _caster;
        targets = _targets;
        habilidadUsada = _habilidad;
    }

    public void CargarDatos(PersonajeBase _caster, PersonajeBase _target, Habilidad _habilidad)
    {
        caster = _caster;
        target = _target;
        habilidadUsada = _habilidad;
    }

    public void CargarDatos(PersonajeBase _caster, Habilidad _habilidad)
    {
        caster = _caster;
        habilidadUsada = _habilidad;
    }

    public void CargarDatos(Habilidad _habilidad)
    {
        habilidadUsada = _habilidad;
    }

    public void EjecutarHabilidad()
    {
        if (habilidadUsada.Tipo == TipoHabilidad.Movimiento)
        {
            EjecutarMovimiento();
        }
    }

    private void EjecutarMovimiento()
    {
        caster.Estamina.ValorActual -= habilidadUsada.CostoEst;
        caster.PM.ValorActual -= habilidadUsada.CostoPM;
        if (habilidadUsada.efectos.Count > 0)
        {
            foreach (EfectosHabilidad efecto in habilidadUsada.efectos)
            {
                caster.AgregarAtributoAlterado(new AtributosAlterados(efecto.Duracion, efecto.StatAfectado, efecto.cantidadAfectada, efecto.Modificador));
            }
        }
    }

    public void CancelarUltimaHabilidadMovimientoUsada()
    {
        caster.Estamina.ValorActual += habilidadUsada.CostoEst;
        caster.PM.ValorActual += habilidadUsada.CostoPM;
        if (habilidadUsada.efectos.Count > 0)
        {
            for (int i = habilidadUsada.efectos.Count; i > 0; i--)
            {
                caster.listaAtributosAlterados.RemoveAt(caster.listaAtributosAlterados.Count-1);
            }
        }
    }
}
