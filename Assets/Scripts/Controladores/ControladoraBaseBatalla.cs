using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controla todos los aspectos de las batallas.
/// </summary>
[System.Serializable]
public class ControladoraBaseBatalla{

    [SerializeField]
    public ControladoraTurno controladoraTurno;

    /// <summary>
    /// La fase en que se encuentra la batalla.
    /// </summary>
    public FasesBatalla faseActual = FasesBatalla.EsperandoTurno;

    private static ControladoraBaseBatalla instanceRef;

    public static ControladoraBaseBatalla InstanceRef()
    {
        if (instanceRef == null)
        {
            instanceRef = new ControladoraBaseBatalla();
        }

        return instanceRef;
    }

    /// <summary>
    /// Lista de jugadores participantes en la batalla.
    /// </summary>
    public static List<PersonajeControlable> jugadores= new List<PersonajeControlable>();

    /// <summary>
    /// Lista de enemigos en la batalla.
    /// </summary>
    public static List<Enemigo> enemigos = new List<Enemigo>();

    /// <summary>
    /// Turno listo, sacado de la controladora de Turnos.
    /// </summary>
    private Turno turnoActual;

    public Turno TurnoActual
    {
        get { return turnoActual; }
    }

    float count = 0;

    public ControladoraBaseBatalla()
    {
        controladoraTurno = new ControladoraTurno();
    }
	
	// Update is called once per frame
	public void Update () {

        controladoraTurno.Update();


        switch (faseActual)
        {
            case FasesBatalla.EsperandoTurno:
                {
                    
                    if (controladoraTurno.turnosCompletos.Count > 0 && turnoActual == null)
                    {
                        faseActual = FasesBatalla.Estrategia;
                        turnoActual = controladoraTurno.turnosCompletos.Dequeue();
                    }
                    break;
                }
            case FasesBatalla.Estrategia:
                {
                    if (turnoActual.Personaje.comportamientoActual == ComportamientoJugador.MovimientoFinalizado)
                    {
                        MovimientoFinalizado();
                    }
                    break;
                }

            case FasesBatalla.EjecutandoAccion:
                {
                    if (count < 3)
                    {
                        count += Time.deltaTime;

                    }
                    else
                    {
                        turnoActual = null;
                        faseActual = FasesBatalla.EsperandoTurno;
                        count = 0;
                    }
                    break;
                }
        }
	}

    void MovimientoFinalizado()
    {
        faseActual = FasesBatalla.EjecutandoAccion;
    }

    private void SetPersonajesControlables(List<PersonajeControlable> personajes)
    {
        jugadores.AddRange(personajes);
    }

    private void SetEnemigos(List<Enemigo> enemi)
    {
        enemigos.AddRange(enemi);
    }


    /// <summary>
    /// Prepara los GO de la lista de Jugadores, para que funcionen correctamente en la scene de batalla
    /// </summary>
    public void PrepararJugadorScripts()
    {
        foreach (PersonajeControlable a in jugadores)
        {
            a.transform.position = new Vector3(0f, 0.5f, 0f);
            a.GetComponent<ThirdPersonController>().enabled = false;
            a.GetComponent<HAPQ>().enabled = false;
            a.GetComponent<Historia>().enabled = false;
            a.GetComponentInChildren<Camera>().enabled = false;
            a.GetComponentInChildren<MouseOrbitImproved>().enabled = false;
            a.GetComponent<AudioListener>().enabled = false;
        }
        
    }

    public void IniciarPelea(List<PersonajeControlable> listaPersonajesControlables,List<Enemigo> listaEnemigos)
    {
        faseActual = FasesBatalla.EsperandoTurno;
        SetPersonajesControlables(listaPersonajesControlables);
        SetEnemigos(listaEnemigos);
        controladoraTurno.CargarDatosParaTurnos(listaPersonajesControlables, listaEnemigos);
    }



}
