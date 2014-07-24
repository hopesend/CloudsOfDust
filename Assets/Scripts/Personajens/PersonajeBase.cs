using UnityEngine;
using System.Collections.Generic;

public class PersonajeBase : MonoBehaviour {

	public string nombre;

    private List<EstadosAlterados> estadosAlterados = new List<EstadosAlterados>();

    public ComportamientoJugador comportamientoActual;

    public List<Habilidad> listaHabilidades;

    public List<AtributosAlterados> listaAtributosAlterados = new List<AtributosAlterados>();

    public BaseAtributo Movimiento;
    public BaseAtributo Vitalidad;
    public BaseAtributo Estamina;
    public BaseAtributo PM;
    public BaseAtributo Fuerza;
    public BaseAtributo Resistencia;
    public BaseAtributo Concentracion;
    public BaseAtributo Espiritu;
    public BaseAtributo Evasion;
    public BaseAtributo Punteria;
    public BaseAtributo Rapidez;
    public BaseAtributo Suerte;

    public float GetMovimientoFinal()
    {
        float temp = 0;
        temp = Movimiento.ValorActual;
        temp += listaAtributosAlterados.ReturnValorModificaciones(NombreAtributo.Movimiento);
        return temp;
    }


    // Use this for initialization
	public virtual void Start () {
	}
	
	// Update is called once per frame
	public virtual void Update () {
	
	}

    public virtual void Awake()
    {
        comportamientoActual = ComportamientoJugador.EsperandoComportamiento;
        InicializarAtributos();
        listaHabilidades = new List<Habilidad>();
        
    }
	//Gets y Sets
		//Nombre
	public string Get_Nombre(){
		return (nombre);
	}

	public void Set_Nombre(string n){
		nombre = n;
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

    public void AgregarHabilidad(Habilidad nuevahabilidad)
    {
        if (nuevahabilidad != null)
        {
            listaHabilidades.Add(nuevahabilidad);
        }
    }

    private void InicializarAtributos()
    {
        Movimiento = new BaseAtributo(NombreAtributo.Movimiento);
        Vitalidad = new BaseAtributo(NombreAtributo.Vitalidad);
        Estamina = new BaseAtributo(NombreAtributo.Estamina);
        PM = new BaseAtributo(NombreAtributo.PM);
        Fuerza = new BaseAtributo(NombreAtributo.Fuerza);
        Resistencia = new BaseAtributo(NombreAtributo.Resistencia);
        Concentracion = new BaseAtributo(NombreAtributo.Concentracion);
        Espiritu = new BaseAtributo(NombreAtributo.Espiritu);
        Evasion = new BaseAtributo(NombreAtributo.Evasion);
        Punteria = new BaseAtributo(NombreAtributo.Punteria);
        Rapidez = new BaseAtributo(NombreAtributo.Rapidez);
        Suerte = new BaseAtributo(NombreAtributo.Suerte);
    }

    public void AgregarAtributoAlterado(AtributosAlterados nuevo)
    {
        listaAtributosAlterados.Add(CalcularModificacionAtributo(nuevo));
        
    }

    private AtributosAlterados CalcularModificacionAtributo(AtributosAlterados nuevo)
    {
        switch (nuevo.atributoAfectado)
        {
            case NombreAtributo.Movimiento:
                {
                    if (nuevo.modificador == TipoModificador.Multiplicador)
                    {
                        nuevo.montoModificado = (nuevo.cantidadAModificar * Movimiento.ValorBase)-Movimiento.ValorBase;
                    }

                    if (nuevo.modificador == TipoModificador.Unidad)
                    {
                        //temp += att.cantidad;
                    }
                    break;
                }
                
            case NombreAtributo.Vitalidad:
                break;
            case NombreAtributo.Estamina:
                break;
            case NombreAtributo.PM:
                break;
            case NombreAtributo.Fuerza:
                break;
            case NombreAtributo.Resistencia:
                break;
            case NombreAtributo.Concentracion:
                break;
            case NombreAtributo.Espiritu:
                break;
            case NombreAtributo.Evasion:
                break;
            case NombreAtributo.Punteria:
                break;
            case NombreAtributo.Rapidez:
                break;
            case NombreAtributo.Suerte:
                break;
            default:
                break;
        }

        return nuevo;
    }

    public void VerificarAtributosAlterados()
    {
        listaAtributosAlterados.RemoveAll(item => item.duracionRestante == 0);
    }

    public void RestarTurno()
    {
        foreach (AtributosAlterados item in listaAtributosAlterados)
        {
            item.TurnoAtributoAlterado();
        }
    }


}

public static class ExtensionList
{
    public static float ReturnValorModificaciones(this List<AtributosAlterados> att, NombreAtributo nombre)
    {
        float temp = 0;
        foreach (AtributosAlterados item in att)
        {
            if (item.atributoAfectado == nombre)
            {
                temp += item.montoModificado;
            }
        }
        return temp;
    }
}
