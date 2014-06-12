using UnityEngine;
using System.Collections.Generic;
using Pathfinding;

public class PersonajeControlable : PersonajeBase {


    bool move;
    CharacterController controller;

    public Equipamento equipamento;
    private List<EstadosAlterados> estadosAlterados = new List<EstadosAlterados>();

    public Seeker seeker;
    private float falta = 1;
    float movimientoPathFinding = 2;

    public Equipamento Equipamento
    {
        get { return equipamento; }
        set { equipamento = value; }
    }

    public override void Awake(){
        controller = GetComponent<CharacterController>();
        seeker = GetComponent<Seeker>();
        seeker.pathCallback += OnPathCalculado;

        ControladorJugador.instanceRef.Trasher = this;
        ControladorJugador.instanceRef.Cargar_Datos_XML(Get_Nombre());
        equipamento = new Equipamento();
    }

    public override void Update()
    {
        if (move)
        {

            Vector3 velocity = controller.velocity;

            if (velocity != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(velocity);

            Vector3 dir = Vector3.zero;

            Vector3 pos = transform.position;

            if (vectorPath != null && vectorPath.Count != 0)
            {
                Vector3 waypoint = vectorPath[wp];
                waypoint.y = pos.y;

                while ((pos - waypoint).sqrMagnitude < moveNextDist * moveNextDist && wp != vectorPath.Count - 1)
                {
                    wp++;
                    waypoint = vectorPath[wp];
                    waypoint.y = pos.y;
                }

                dir = waypoint - pos;
                float magn = dir.magnitude;
                if (magn > 0)
                {
                    float newmagn = Mathf.Min(magn, movimientoPathFinding);
                    dir *= newmagn / magn;
                }

                if (falta < 0.2f)
                {
                    LlegoDestino();

                }
                else
                {
                    controller.Move(dir * movimientoPathFinding * Time.deltaTime);
                    falta = path.GetTotalLength();
                }
            }
        }
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

    #region Pathfinding

    Path path;
    List<Vector3> vectorPath;
    int wp;
    public float moveNextDist = 1;

    public void OnPathCalculado(Path _p)
    {
        ABPath p = _p as ABPath;

        if (path != null) path.Release(this);
        path = p;
        p.Claim(this);

        if (p.error)
        {
            wp = 0;
            vectorPath = null;
            return;
        }


        Vector3 p1 = p.originalStartPoint;
        Vector3 p2 = transform.position;
        p1.y = p2.y;
        float d = (p2 - p1).magnitude;
        wp = 0;

        vectorPath = p.vectorPath;
        Vector3 waypoint;

        for (float t = 0; t <= d; t += moveNextDist * 0.6f)
        {
            wp--;
            Vector3 pos = p1 + (p2 - p1) * t;

            do
            {
                wp++;
                waypoint = vectorPath[wp];
                waypoint.y = pos.y;
            } while ((pos - waypoint).sqrMagnitude < moveNextDist * moveNextDist && wp != vectorPath.Count - 1);

        }

        move = true;
    }

    public void CalcularPath(Vector3 posicionMover)
    {
        seeker.StartPath(transform.position, posicionMover, OnPathCalculado);
    }

    private void LlegoDestino()
    {
        move = false;
        path = null;
    }

    #endregion

}
