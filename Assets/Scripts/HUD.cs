using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	
	bool b_movimiento;
	bool b_fisico;
	bool b_magia;
	bool b_objeto;
	public Vector2 scrollPosition = Vector2.zero;

	Personaje[] personajes;


	

	// Use this for initialization
	void Start () {
		
		//h = GameObject.FindGameObjectWithTag("aliado").GetComponentInChildren<personaje>().h;	
		personajes = new Personaje[3];
		personajes[0] = GameObject.FindGameObjectWithTag("Player").GetComponent<Personaje>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		
		Rect HUD = new Rect(0,Screen.height*4/5,Screen.width,Screen.height/5);
		Rect Ataques = new Rect(0,0,Screen.width/3,Screen.height/2);
		
		GUIStyle style1 = new GUIStyle();
		GUIStyle style2 = new GUIStyle();
		GUIStyle style3 = new GUIStyle();
		GUIStyle style4 = new GUIStyle();
		Texture2D textura1= new Texture2D(1,1);
		Texture2D textura2= new Texture2D(1,1);
		Texture2D textura3= new Texture2D(1,1);
		Texture2D textura4= new Texture2D(1,1);
		
		
		
		scrollPosition = GUI.BeginScrollView(new Rect(Ataques.width/3,0,Ataques.width*2/3,Ataques.height),scrollPosition,new Rect(0,0,Ataques.width*2/3,Screen.height));
		
		GUI.EndScrollView ();
		

			
			//Ataques
			GUI.Box(Ataques,"");
			
			if (b_fisico = GUILayout.Toggle (b_fisico,"Fisico","Button",GUILayout.Height (Ataques.height/3-5),GUILayout.Width (Ataques.width/3))){
				b_magia = false;
				b_objeto = false;	
				
				scrollPosition = GUI.BeginScrollView(new Rect(Ataques.width/3,0,Ataques.width*2/3,Ataques.height),scrollPosition,new Rect(0,0,Ataques.width*2/3,Screen.height));
		
				GUI.EndScrollView ();
			}
		
			if (b_magia = GUILayout.Toggle (b_magia,"Magia","Button",GUILayout.Height (Ataques.height/3-5))){
				b_fisico = false;
				b_objeto = false;
				
				
				scrollPosition = GUI.BeginScrollView(new Rect(Ataques.width/3,0,Ataques.width*2/3,Ataques.height),scrollPosition,new Rect(0,0,Ataques.width*2/3,Screen.height));
				float altura =0;
				for (int i=0; i<3;i++ ){
					
					//GUI.Button (new Rect(i,0,Ataques.width*2/3,20),h[i].nombreHabilidad);
					//altura+=20;
				}
				GUI.EndScrollView ();
				
			}
		
			if (b_objeto = GUILayout.Toggle (b_objeto,"Objeto","Button",GUILayout.Height (Ataques.height/3-5))){
				b_fisico = false;
				b_magia = false;
				
				scrollPosition = GUI.BeginScrollView(new Rect(Ataques.width/3,0,Ataques.width*2/3,Ataques.height),scrollPosition,new Rect(0,0,Ataques.width*2/3,Screen.height));
		
				GUI.EndScrollView ();
			}
				
			
			
			//HUD
			GUI.Box(HUD,"");
			
			GUI.backgroundColor = Color.clear;
			GUI.TextArea(new Rect(HUD.x+5,HUD.y+5,100,30),personajes[0].Get_Nombre()); //Nombre
			GUI.TextArea(new Rect(HUD.x+5,HUD.y+35,100,30),"Vit: "+personajes[0].vit.actual+"/"+personajes[0].vit.valor); //vitalidad
			GUI.TextArea(new Rect(HUD.x+5,HUD.y+55,100,30),"Esn: "+personajes[0].esn.actual+"/"+personajes[0].esn.valor);	//estamina
			GUI.TextArea(new Rect(HUD.x+5,HUD.y+75,100,40),"PM: "+personajes[0].pm.actual+"/"+personajes[0].pm.valor);	//puntos magicos
			
			
			GUI.backgroundColor=Color.red;
			textura1.SetPixel(1,1,Color.red);
			style1.normal.background = textura1;
			GUI.Box (new Rect(HUD.x+105,HUD.y+40,personajes[0].vit.actual*(Screen.width/3-110)/personajes[0].vit.valor,10),"",style1);
			
			GUI.backgroundColor=Color.green;
			textura2.SetPixel(1,1,Color.green);
			style2.normal.background = textura2;
			GUI.Box (new Rect(HUD.x+105,HUD.y+60,personajes[0].esn.actual*(Screen.width/3-110)/personajes[0].esn.valor,10),"",style2);
			
			GUI.backgroundColor=Color.blue;
			textura3.SetPixel(1,1,Color.yellow);
			style3.normal.background = textura3;
			GUI.Box (new Rect(HUD.x+105,HUD.y+80,personajes[0].pm.actual*(Screen.width/3-110)/personajes[0].pm.valor,10),"",style3);
			
			
			
			
		
	}
}
