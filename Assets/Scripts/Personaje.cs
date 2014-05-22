using UnityEngine;
using System.Collections;

public class Personaje : MonoBehaviour {

	private string nombre;

	public struct parametro{
		public int valor;
		public int actual;
	}

	public parametro mov; //movimiento
	public parametro vit; //vitalidad
	public parametro esn; //estamina
	public parametro pm;	//puntos magicos

	public parametro fue; //fuerza
	public parametro res; //Resistencia
	public parametro con; //Concentracion
	public parametro esp; //Espiritu
	public parametro eva; //Evasion
	public parametro pnt; //punteria
	public parametro rap; //rapidez
	public parametro sue; //suerte


	// Use this for initialization
	void Start () {
		nombre = "Trasher";
		mov.valor = 10;
		mov.actual = mov.valor;
		vit.valor = 1000;
		vit.actual = vit.valor;
		esn.valor = 20;
		esn.actual = esn.valor;
		pm.valor = 10;
		pm.actual = pm.valor;
		fue.valor = 10;
		fue.actual = fue.valor;
		res.valor = 10;
		res.actual = res.valor;
		con.valor = 5;
		con.actual = con.valor;
		esp.valor = 5;
		esp.actual = esp.valor;
		eva.valor = 10;
		eva.actual = eva.valor;
		pnt.valor = 5;
		pnt.actual = pnt.valor;
		rap.valor = 10;
		rap.actual = rap.valor;
		sue.valor = 10;
		sue.actual = sue.valor;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	//Gets y Sets
		//Nombre
	public string Get_Nombre(){
		return (nombre);
	}

	public void Set_Nombre(string n){
		nombre = n;
	}


}
