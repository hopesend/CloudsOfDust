using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controla todos los aspectos de las batallas.
/// </summary>
[System.Serializable]
public class ControladoraBaseBatalla{

    [SerializeField]
    public ControladoraTurno controladoraTurno;

    [SerializeField]
    public ControladoraCombo controladoraCombo;

    public ControladoraHabilidades controladoraHabilidad;

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
    /// Jugador que estoy posicionando.
    /// </summary>
    public PersonajeControlable jugadorEnPosActual;

    /// <summary>
    /// Index que usare en la lista de jugadores.
    /// </summary>
    public int indexPersonajeEnListaPosActual = 0;

    Ray rayPrueba;
    RaycastHit hit = new RaycastHit();

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
        controladoraCombo = new ControladoraCombo();
        controladoraHabilidad = new ControladoraHabilidades();
        controladoraCombo.Start();
    }
	
	// Update is called once per frame
	public void Update () {

        if (faseActual != FasesBatalla.PosicionandoJugadores)
        {
            controladoraTurno.Update();
        }

        switch (faseActual)
        {
            case FasesBatalla.PosicionandoJugadores:
                {
                    //Primero los enemigos.

                    //Ahora los jugadores
                    if (jugadorEnPosActual.enabled == false)
                    {
                        jugadorEnPosActual.enabled = true;
                    }

                    rayPrueba = Camera.main.ScreenPointToRay(Input.mousePosition);
                    
                    if (Physics.Raycast(rayPrueba, out hit))
                    {
                        if (hit.collider.gameObject.tag == "Respawn")
                        {
                            jugadorEnPosActual.transform.position = hit.point;
                            if (Input.GetMouseButton(0))
                            {
                                indexPersonajeEnListaPosActual++;
                                if (indexPersonajeEnListaPosActual < jugadores.Count)
                                {
                                    jugadorEnPosActual = jugadores[indexPersonajeEnListaPosActual];
                                }
                                else
                                {
                                    faseActual = FasesBatalla.EsperandoTurno;
                                }
                            }
                        }
                    }
                    break;
                }

            case FasesBatalla.EsperandoTurno:
                {
                    
                    if (controladoraTurno.turnosCompletos.Count > 0 && turnoActual == null)
                    {
                        faseActual = FasesBatalla.SeleccionandoHabilidad;
                        turnoActual = controladoraTurno.turnosCompletos.Peek();
                    }
                    break;
                }

            case FasesBatalla.MarcandoCaminoJugador:
                {
                    
                    ((PersonajeControlable)controladoraHabilidad.caster).PrepararParaMarcarCamino();
                    break;
                }

            case FasesBatalla.EjecutandoHabilidad:
                {
                    controladoraHabilidad.EjecutarHabilidad();
                    if (controladoraHabilidad.habilidadUsada.Tipo == TipoHabilidad.Movimiento)
                    {
                        faseActual = FasesBatalla.MarcandoCaminoJugador;
                    }
                    break;
                }

            case FasesBatalla.EjecutandoCombo:
                {
                    if (!controladoraCombo.inCombo)
                    {
                        controladoraCombo.StartCombo(controladoraHabilidad.habilidadUsada.ComboHabilidad); 
                    }
                    controladoraCombo.Update();
                    if (controladoraCombo.TerminoCombo)
                    {
                        if (controladoraHabilidad.habilidadUsada.Tipo == TipoHabilidad.Movimiento)
                        {
                            faseActual = FasesBatalla.EjecutandoHabilidad;
                        }
                    }
                    break;
                }

            case FasesBatalla.EjecutandoAnimacion:
                {
                    break;
                }
        }
	}
    
    public void MovimientoFinalizado()
    {
        turnoActual.seMovio = true;
        if (turnoActual.usoHabilidad)
        {
            FinTurnoActual();
        }
        faseActual = FasesBatalla.SeleccionandoHabilidad;
    }

    public void HabilidadFinalizada()
    {
        turnoActual.usoHabilidad = true;
        if (turnoActual.seMovio)
        {
            FinTurnoActual();
        }
        faseActual = FasesBatalla.SeleccionandoHabilidad;
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
            a.transform.position = new Vector3(0f, 1f, 0f);
            a.GetComponent<HAQPHUD>().enabled = false;
            a.GetComponent<Historia>().enabled = false;
            a.GetComponentInChildren<Camera>().enabled = false;
            a.GetComponent<AudioListener>().enabled = false;
            a.GetComponent<RPG_Controller>().enabled = false;
            a.GetComponentInChildren<RPG_Camera>().enabled = false;
        }
        
    }

    public void IniciarPelea(List<PersonajeControlable> listaPersonajesControlables,List<Enemigo> listaEnemigos)
    {
        faseActual = FasesBatalla.PosicionandoJugadores;
        SetPersonajesControlables(listaPersonajesControlables);
        jugadorEnPosActual = listaPersonajesControlables[indexPersonajeEnListaPosActual];
        SetEnemigos(listaEnemigos);
        controladoraTurno.CargarDatosParaTurnos(listaPersonajesControlables, listaEnemigos);
    }

    public void FinTurnoActual()
    {
        faseActual = FasesBatalla.EsperandoTurno;
        controladoraTurno.turnosCompletos.Dequeue();
        turnoActual = null;
    }

    public void FinTurnoActualEleccion()
    {
        //Cambiar esto por un metodo
        controladoraTurno.turnosCompletos.Peek().seMovio = false;
        controladoraTurno.turnosCompletos.Peek().usoHabilidad = false;
        controladoraTurno.turnosCompletos.Peek().personaje.comportamientoActual = ComportamientoJugador.EsperandoComportamiento;
        controladoraTurno.turnosCompletos.Enqueue(controladoraTurno.turnosCompletos.Dequeue());
        faseActual = FasesBatalla.EsperandoTurno;
        turnoActual = null;
    }



}
