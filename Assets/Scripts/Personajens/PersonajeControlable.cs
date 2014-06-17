using UnityEngine;
using System.Collections.Generic;


public class PersonajeControlable : PersonajeBase {

    public Equipamento equipamento;
    
	public int comportamiento; //cada numero supondra un comportamiento (Despues podemos sustituirlo por clases separadas)
	//1- Marcar camino
	//2- Movimiento

	private List<GameObject> target = new List<GameObject>();
	public CharacterController characterController;
	int i; //auxiliar
	int movimiento; //maximo movimiento
	Plane planoBatalla;

    public Equipamento Equipamento
    {
        get { return equipamento; }
        set { equipamento = value; }
    }

    public override void Awake(){

        ControladorJugador.instanceRef.Trasher = this;
        ControladorJugador.instanceRef.Cargar_Datos_XML(Get_Nombre());

        equipamento = new Equipamento();

		characterController= GetComponent<CharacterController>();
		planoBatalla = new Plane(Vector3.up,transform.position); //plano para el raycast
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
		lineRenderer.SetColors(Color.yellow, Color.red);
		lineRenderer.SetWidth(0.2F, 0.2F);

    }

    public override void Update()
    {
		switch(comportamiento){

		case 1:{ //Marcar camino
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			Physics.Raycast (ray,out hit);
			if (Input.GetKeyDown (KeyCode.Mouse1)){ //añade un nuevo target con cada click
				GameObject t = new GameObject("target"+target.Count);
				t.transform.position = hit.point;
				target.Add(t);
				target[target.Count-1].transform.position = hit.point;
			}
			if (target.Count > 0){

			}


			break;
		}

		case 2: //Movimiento
			if (i<target.Count){ //si estamos dentro del rango de target
				if (Vector3.Distance (transform.position,target[i].transform.position) >0.01){//si no ha llegado al siguiente destino
					//moverse hacia el destino
					transform.LookAt (target[i].transform.position);
					transform.position = Vector3.MoveTowards(transform.position,target[i].transform.position,5*Time.deltaTime);

				}else{
					GameObject.Destroy(target[i]);
					i++;
				}
			
			}else{ //significa que ha llegado al destino
				comportamiento = 0;
			}

			break;


		}

	


    }

	public void OnGUI(){
		switch (comportamiento){
		case 1:{
			Rect aceptar = new Rect(Screen.width*3/4,0,Screen.width/4,Screen.height/5);
			GUI.Box (aceptar,"");
			if (GUI.Button(new Rect(aceptar.x+aceptar.width/8,aceptar.height/2-10,aceptar.width/4,20),"Aceptar")){
				comportamiento = 2;
				i=0;
			}
			if (GUI.Button(new Rect(aceptar.x+aceptar.width*5/8,aceptar.height/2-10,aceptar.width/4,20),"Cancelar")){
				foreach (GameObject g in target){
					Destroy (g);
				}
				target.Clear ();
				comportamiento = 0;
			}
			break;
		}
		}

	}



	public void MoverBatalla(int mov)
	{
		comportamiento = 1;
		movimiento = mov;
		if (target.Count>0){
			target.Clear ();
		}
	}

    

}
