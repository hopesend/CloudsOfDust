using UnityEngine;
using System.Collections;

[System.Serializable]
public class AtributosAlterados {

    public float montoModificado;

    public int duracionTotal;
    public int duracionRestante;
    public NombreAtributo atributoAfectado;
    public float cantidadAModificar;
    public TipoModificador modificador;

    public AtributosAlterados (int _duracionTot, NombreAtributo _attAfectado, float _cant, TipoModificador _tipo)
	{
        duracionTotal = _duracionTot;
        duracionRestante = _duracionTot;
        atributoAfectado = _attAfectado;
        cantidadAModificar = _cant;
        modificador = _tipo;
	}
    /// <summary>
    /// Se debe llamar cada vez que vuelve a ser su turno
    /// </summary>
    /// <returns></returns>
    public void TurnoAtributoAlterado()
    {
        duracionRestante--;
    }

    
}
