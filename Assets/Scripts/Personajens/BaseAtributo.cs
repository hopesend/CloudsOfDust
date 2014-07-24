using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public  class BaseAtributo{

    public NombreAtributo nombreAtr;

    public NombreAtributo NombreAtr
    {
        get { return nombreAtr; }
        set { nombreAtr = value; }
    }

    public  float valorMaximo;
    /// <summary>
    /// Monto maximo que puede tomar el atributo
    /// </summary>
    public float ValorMaximo
    {
        get { return valorMaximo; }
        set { valorMaximo = value; }
    }


    public float valorActual;
    /// <summary>
    /// Monto actual del atributo, es modificado en la batalla y por buffs y debuffs
    /// </summary>
    public float ValorActual
    {
        get { return valorActual; }
        set { valorActual = value; }
    }


    public float valorBase;
    /// <summary>
    /// Valor que tiene fuera de la batalla, al iniciar cada pelea se actualiza el <see cref="ValorActual"/>. 
    /// </summary>
    public float ValorBase
    {
        get { return valorBase; }
        set 
        { 
            valorBase = value;
            valorActual = valorBase;
        }
    }

    public BaseAtributo(NombreAtributo nombre)
    {
        nombreAtr = nombre;
    }

    public float GetSuerteRandom()
    {
        if (nombreAtr == NombreAtributo.Suerte)
        {
            return UnityEngine.Random.Range(valorActual, valorMaximo);
        }
        else
        {
            Debug.LogError("Este metodo debe ser llamado solo por el atributo suerte");
            return -1;
        }
    }


}
