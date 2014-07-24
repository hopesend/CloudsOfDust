using UnityEngine;
using System.Collections;

public enum NombreAtributo {
    Movimiento, 
    Vitalidad, 
    Estamina, 
    PM, 
    Fuerza, 
    Resistencia, 
    Concentracion, 
    Espiritu, 
    Evasion, 
    Punteria, 
    Rapidez, 
    Suerte
}

public static class NombreAtributoExtension
{
    public static string Descripcion(this NombreAtributo nombre)
    {
        switch (nombre)
        {
            case NombreAtributo.Movimiento:
                {
                    return "";
                }
                
            case NombreAtributo.Vitalidad:
                {
                    return "";
                }
            case NombreAtributo.Estamina:
                {
                    return "";
                }
            case NombreAtributo.PM:
                {
                    return "";
                }
            case NombreAtributo.Fuerza:
                {
                    return "";
                }
            case NombreAtributo.Resistencia:
                {
                    return "Representa el valor de la resistencia fisica";
                }
            case NombreAtributo.Concentracion:
                {
                    return "";
                }
            case NombreAtributo.Espiritu:
                {
                    return "Representa el valor de la resistencia fisica";
                }
            case NombreAtributo.Evasion:
                {
                    return "";
                }
            case NombreAtributo.Punteria:
                {
                    return "";
                }
            case NombreAtributo.Rapidez:
                {
                    return "";
                }
            case NombreAtributo.Suerte:
                {
                    return "";
                }
            default:
                return "";
        }

       
        
    }
}
