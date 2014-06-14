using UnityEngine;
using System.Collections.Generic;

public class PersonajeBase : MonoBehaviour {

	public string nombre;

    private List<EstadosAlterados> estadosAlterados = new List<EstadosAlterados>();

   
	public struct parametro{
		/// <summary>
		/// Valor absoluto del parametro
		/// </summary>
        private int valor;

        /// <summary>
        /// Al setear esta variable, cargo el valor actual con la misma
        /// </summary>
        public int Valor
        {
            get { return valor; }
            set 
            { 
                valor = value;
                actual = valor;
            }
        }

		/// <summary>
		/// Valor Actual, modificado mediante accesorios y buffs
		/// </summary>
        private int actual;

        public int Actual
        {
            get { return actual; }
            set { actual = value; }
        }
	}
     [SerializeField]
    /// <summary>
    /// Movimiento
    /// </summary>
	public parametro mov; 

    /// <summary>
    /// Vitalidad
    /// </summary>
	public parametro vit;

    /// <summary>
    /// Estamina
    /// </summary>
	public parametro esn;

    /// <summary>
    /// Puntos Magicos
    /// </summary>
    public parametro pm;

    /// <summary>
    /// Fuerza
    /// </summary>
	public parametro fue; 

    /// <summary>
    /// Resistencia
    /// </summary>
	public parametro res; 

    /// <summary>
    /// Concentracion
    /// </summary>
	public parametro con;

    /// <summary>
    /// Espiritu
    /// </summary>
	public parametro esp;

    /// <summary>
    /// Evasion
    /// </summary>
	public parametro eva;

    /// <summary>
    /// Punteria
    /// </summary>
	public parametro pnt;

    /// <summary>
    /// Rapidez
    /// </summary>
	public parametro rap;

    /// <summary>
    /// Suerte
    /// </summary>
	public parametro sue; 


	// Use this for initialization
	public virtual void Start () {
	}
	
	// Update is called once per frame
	public virtual void Update () {
	
	}

    public virtual void Awake()
    {
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


}
