using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ControladoraTurno {


    float tiempoParaTurno = 100.0f;

    [SerializeField]
    public List<Turno> listaOrdenTurnos = new List<Turno>();
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
        foreach (Turno a in listaOrdenTurnos)
        {
            if (!a.listo)
            {
                a.Update();
                if (a.TiempoActual >= tiempoParaTurno)
                {
                    a.Listo();
                    turnosCompletos.Enqueue(a);
                } 
            }
        }
    }

}
