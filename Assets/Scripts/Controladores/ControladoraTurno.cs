using UnityEngine;
using System.Collections.Generic;

public class ControladoraTurno {


    float tiempoParaTurno = 100.0f;

    [SerializeField]
    public List<Turno> listaOrdenTurnos = new List<Turno>();
    public Queue<Turno> turnosCompletos = new Queue<Turno>();
    public ControladoraTurno()
    {
        listaOrdenTurnos.Add(new Turno(ControladorJugador.instanceRef.Trasher));
    }

    public void Update()
    {
        foreach (Turno a in listaOrdenTurnos)
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
