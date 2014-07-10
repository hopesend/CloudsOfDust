using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ControladoraTurno {


    float tiempoParaTurno = 100.0f;

    [SerializeField]
    public List<Turno> listaOrdenTurnos = new List<Turno>();
    [SerializeField]
    public Queue<Turno> turnosCompletos = new Queue<Turno>();

    public ControladoraTurno()
    {
    }

    public void CargarDatosParaTurnos(List<PersonajeControlable> listaJugadores, List<Enemigo> listaEnemigos)
    {
        foreach (PersonajeControlable a in listaJugadores)
        {
            listaOrdenTurnos.Add(new Turno(a));
        }

        foreach (Enemigo b in listaEnemigos)
        {
            listaOrdenTurnos.Add(new Turno(b));
        }
    }


    public void Update()
    {
        foreach (Turno turno in listaOrdenTurnos)
        {
            if (!turno.listo)
            {
                turno.Update();
                if (turno.TiempoActual >= tiempoParaTurno)
                {
                    turno.Listo();
                    turnosCompletos.Enqueue(turno);
                } 
            }
        }
    }

}
