using UnityEngine;
using System.IO;

public class MenuPrincipal: IControlador
{
	private ControladorNiveles manager;

	public MenuPrincipal(ControladorNiveles managerRef)
	{
		manager = managerRef;

		if(Application.loadedLevelName != "Menu Principal")
		{
			Application.LoadLevel("Menu Principal");
		}
	}
	
	public void EstadoUpdate()
	{

	}
	
	public void Mostrar()
	{
		Rect menu = new Rect(Screen.width/2-50,Screen.height/2-80,100,400);
		if (GUI.Button(new Rect(menu.x, menu.y, 100,30), "Nuevo Juego"))
		{
           // manager.CambiarEstado(new EscenarioVecindario(manager));
			//Creamos los XML con valores Base
			if (ControladorJugador.instanceRef.Inicializar_Valores_XML("Trasher"))
			{
				if(ControladorGlobal.instanceRef.Inicializar_Valores_XML())
				{
					//Cambiamos de Escenario
					manager.CambiarEstado (new EscenarioVecindario(manager));
				}
				else
				{
					//TODO: hacer que salga un error ya que no ha podido crear el fichero...
				}
			}
			else
			{
				//TODO: hacer que salga un error ya que no ha podido crear el fichero...
			}
		}
		GUI.Button(new Rect(menu.x, menu.y+40, 100,30), "Cargar");
		GUI.Button(new Rect(menu.x, menu.y+80, 100,30), "Opciones");
		if(GUI.Button(new Rect(menu.x, menu.y+120, 100,30), "Salir")){
			Application.Quit();
		}
	}

	public void NivelCargado(int level)
	{
	}
}
