using UnityEngine;
using System.Collections.Generic;


public class PersonajeControlable : PersonajeBase {

    public IDPersonajes ID;

    public Equipamento equipamento;
    
	  

	public List<GameObject> target = new List<GameObject>();
	int i; //auxiliar
	Plane planoBatalla;
	public LineRenderer lineRenderer;
    public List<Vector3> listaPosicionLine = new List<Vector3>();

    public float gastoActual;

    public Equipamento Equipamento
    {
        get { return equipamento; }
        set { equipamento = value; }
    }

    public override void Awake(){

        DontDestroyOnLoad(this);

        equipamento = new Equipamento();

		planoBatalla = new Plane(Vector3.up,transform.position); //plano para el raycast
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(Color.blue, Color.cyan);
		lineRenderer.SetWidth(0.2F, 0.2F);
		lineRenderer.SetVertexCount(1);
        base.Awake();

    }

    public override void Update()
    {
        if (GameMaster.InstanceRef.EstadoActual == EstadoJuego.Batalla)
        {
            switch (comportamientoActual)
            {

                case ComportamientoJugador.MarcandoCamino:
                    { //Marcar camino
                        HUDBatalla.InstanceRef().showMovConfPlayer = true;
                        lineRenderer.SetVertexCount (target.Count+2);
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
			            Physics.Raycast (ray,out hit);
                        if (GetMovimientoFinal() > 0.5f)
                        {
                            
                            gastoActual = Vector3.Distance(listaPosicionLine[listaPosicionLine.Count - 1], hit.point);
                        }

                        if (gastoActual < GetMovimientoFinal())
                        {
                            lineRenderer.SetPosition(target.Count + 1, hit.point);
                            if (Input.GetKeyDown(KeyCode.Mouse1))
                            {
                                GameObject t = new GameObject("Target" + target.Count.ToString());
                                t.transform.position = hit.point;
                                t.tag = "Target";
                                target.Add(t);
                                listaPosicionLine.Add(hit.point);
                                Movimiento.ValorActual -=gastoActual;
                            } 
                        }
				        
                        
                        break;
                    }
                case ComportamientoJugador.Moviendo: //Movimiento
                    if (i < target.Count)
                    { //si estamos dentro del Rango de target
                        lineRenderer.SetPosition (0,transform.position);
                        if (Vector3.Distance(transform.position, target[i].transform.position) > 0.01)
                        {//si no ha llegado al siguiente destino
                            //moverse hacia el destino
                            transform.LookAt(target[i].transform.position);
                            transform.position = Vector3.MoveTowards(transform.position, target[i].transform.position, 5 * Time.deltaTime);

                        }
                        else
                        {
                            Destroy(target[0]);
					        target.RemoveAt(0);
                        }
                    }
                    else
                    { //significa que ha llegado al destino
                        comportamientoActual = ComportamientoJugador.MovimientoFinalizado;
                        ControladoraBaseBatalla.InstanceRef().MovimientoFinalizado();
                        target.Clear();
                    }

                    break;
            } 
        }
		for (int j=0;j<target.Count;j++){
			lineRenderer.SetPosition(j+1, target[j].transform.position);
		}
    }

	public void PrepararParaMarcarCamino()
	{
        if (comportamientoActual != ComportamientoJugador.MarcandoCamino && comportamientoActual != ComportamientoJugador.Moviendo)
        {
            comportamientoActual = ComportamientoJugador.MarcandoCamino;
            lineRenderer.SetPosition(0, transform.position);
            listaPosicionLine.Add(transform.position);
            if (target.Count > 0)
            {
                target.Clear();
            } 
        }
	}

    public void AceptarMovimiento()
    {
        comportamientoActual = ComportamientoJugador.Moviendo;
        lineRenderer.SetVertexCount(target.Count + 1);
        i = 0;
        HUDBatalla.InstanceRef().showMovConfPlayer = false ;
        gastoActual = 0;
    }

    public void CancelarMovimiento() 
    {
        foreach (GameObject g in target)
        {
            Destroy(g);
        }
        lineRenderer.SetVertexCount(1);
        target.Clear();
        listaPosicionLine.Clear();
        gastoActual = 0;
        Movimiento.ValorActual = Movimiento.ValorBase;
        ControladoraBaseBatalla.InstanceRef().controladoraHabilidad.CancelarUltimaHabilidadMovimientoUsada();
        comportamientoActual = ComportamientoJugador.EsperandoComportamiento;
        ControladoraBaseBatalla.InstanceRef().faseActual = FasesBatalla.SeleccionandoHabilidad;
        HUDBatalla.InstanceRef().showMovConfPlayer = false;
        
    }

    

}
