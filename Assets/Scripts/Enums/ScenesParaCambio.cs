using UnityEngine;
using System;

public enum ScenesParaCambio{

    MenuPrincipal, Vecindario, BatallaTutorial, Batalla, EdificioOficial, CasaInterior, InteriorLeveler, Recepcion, SalaActualizacion
}


public static class EnumExtension
{
    public static ScenesParaCambio SetEscenarioActual(this ScenesParaCambio cambio)
    {
        cambio = (ScenesParaCambio) Enum.Parse(typeof(ScenesParaCambio), Application.loadedLevelName);
        
        return cambio;
    }
}
