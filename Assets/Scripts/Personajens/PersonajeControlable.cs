using UnityEngine;
using System.Collections.Generic;


public class PersonajeControlable : PersonajeBase {

    public IDPersonajes ID;

    public Equipamento equipamento;
    
	  

	public List<GameObject> target = new List<GameObject>();
	int i; //auxiliar
	int movimiento; //maximo movimiento
	Plane planoBatalla;
	public LineRenderer lineRenderer;

    public int rapide;

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
        rapide = rap.Valor;
        if (GameMaster.instanceRef.EstadoActual == EstadoJuego.Batalla)
        {
            switch (comportamientoActual)
            {

                case ComportamientoJugador.MarcandoCamino:
                    { //Marcar camino
			
                        lineRenderer.SetVertexCount (target.Count+2);
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
			            Physics.Raycast (ray,out hit);
			            lineRenderer.SetPosition (target.Count+1,hit.point);
                        if (Input.GetKeyDown(KeyCode.Mouse1)){
                            GameObject t = new GameObject("Target" + target.Count.ToString());
                            t.transform.position = hit.point;
                            t.tag = "Target";
                            target.Add(t);
                            target[target.Count-1].transform.position = hit.point;
                        }
				        
                        
                        break;
                    }
                case ComportamientoJugador.Moviendo: //Movimiento
                    if (i < target.Count)
                    { //si estamos dentro del rango de target
                        lineRenderer.SetPosition (0,transform.position);
                        if (Vector3.Distance(transform.position, target[i].transform.position) > 0.01)
                        {//si no ha llegado al siguiente destino
                            //moverse hacia el destino
                            transform.LookAt(target[i].transform.position);
                            transform.position = Vector3.MoveTowards(transform.position, target[i].transform.position, 5 * Time.deltaTime);

                        }
                        else
                        {
					        target.RemoveAt(0);
                        }
                    }
                    else
                    { //significa que ha llegado al destino
                        comportamientoActual = ComportamientoJugador.MovimientoFinalizado;
                        
                        foreach (GameObject g in target){
                            Destroy (g);
                        }
                        target.Clear();
                    }

                    break;
            } 
        }
		for (int j=0;j<target.Count;j++){
			lineRenderer.SetPosition(j+1, target[j].transform.position);
		}
	


    }

	public void OnGUI(){

        if (GameMaster.instanceRef.EstadoActual == EstadoJuego.Batalla)
        {
            if (target.Count > 0)
            {
                switch (comportamientoActual)
                {
                    case ComportamientoJugador.MarcandoCamino:
                        {
                            HUDBatalla.InstanceRef().showMovConfPlayer = true;
                            break;
                        }
                }
            } 
        }

	}

    



	public void MoverBatalla(int CantidadMovimiento)
	{
        comportamientoActual = ComportamientoJugador.MarcandoCamino;
		movimiento = CantidadMovimiento;
		lineRenderer.SetPosition(0,transform.position);
		if (target.Count>0){
			target.Clear ();
		}
	}

    public void AceptarMovimiento()
    {
        comportamientoActual = ComportamientoJugador.Moviendo;
        lineRenderer.SetVertexCount(target.Count + 1);
        i = 0;
        HUDBatalla.InstanceRef().showMovConfPlayer = false ;
    }

    public void CancelarMovimiento() 
    {
        foreach (GameObject g in target)
        {
            Destroy(g);
        }
        lineRenderer.SetVertexCount(1);
        target.Clear();
        comportamientoActual = ComportamientoJugador.EsperandoComportamiento;
        HUDBatalla.InstanceRef().showMovConfPlayer = false;
    }

    

}
