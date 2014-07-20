using UnityEngine;
using System.Collections;

public class ObjetoConversacion : MonoBehaviour 
{
	public string idTexto;

	void OnTriggerEnter (Collider objeto)
	{
		if (objeto.transform.tag == "Player")
		{
			HUD.instanceRef.cabecera = this.gameObject.tag.ToString();

			Informacion nuevaInformacion = new Informacion();
			HUD.instanceRef.cuerpo = nuevaInformacion.Devolver_Texto(idTexto, this.gameObject.tag.ToString());

			if(HUD.instanceRef.cuerpo[0].Conversacion)
				HUD.instanceRef.preguntas = nuevaInformacion.Devolver_Texto(HUD.instanceRef.cuerpo[0].CodigoReceptor, HUD.instanceRef.cuerpo[0].Receptor);

			HUD.instanceRef.mostrarInteraccion = true;
		}
	}

	void OnTriggerExit (Collider objeto)
	{
		HUD.instanceRef.mostrarInteraccion = false;
	}
}
