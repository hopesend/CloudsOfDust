using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ControladoraBaseBatalla{

    [SerializeField]
    public ControladoraTurno controladoraTurno;

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


    public static List<PersonajeControlable> jugadores= new List<PersonajeControlable>();
    public static List<Enemigo> enemigos = new List<Enemigo>();

    [SerializeField]
    public Turno turnoActual;

    float count = 0;

    void Awake()
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
            case FasesBatalla.Accion:
                {
                    if (Input.GetKeyUp(KeyCode.P))
                    {
                        turnoActual.UsarHabilidad(50);
                        faseActual = FasesBatalla.EjecutandoAccion;
                        
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
        faseActual = FasesBatalla.Accion;
    }

    private void SetPersonajesControlables(List<PersonajeControlable> personajes)
    {
        jugadores.AddRange(personajes);
    }

    private void SetEnemigos(List<Enemigo> enemi)
    {
        enemigos.AddRange(enemi);
    }

    public void PrepararJugadorScripts()
    {
        foreach (PersonajeControlable a in jugadores)
        {
            a.GetComponent<ThirdPersonController>().enabled = false;
            a.GetComponent<HAPQ>().enabled = false;
            a.GetComponent<Historia>().enabled = false;
            a.GetComponentInChildren<Camera>().enabled = false;
            a.GetComponentInChildren<MouseOrbitImproved>().enabled = false;
        }
        
    }

    public void IniciarPelea(List<PersonajeControlable> listaPersonajesControlables,List<Enemigo> listaEnemigos)
    {
        faseActual = FasesBatalla.PosicionandoJugadores;
        SetPersonajesControlables(listaPersonajesControlables);
        SetEnemigos(listaEnemigos);
        controladoraTurno.CargarDatosParaTurnos(listaPersonajesControlables, listaEnemigos);
    }



}
