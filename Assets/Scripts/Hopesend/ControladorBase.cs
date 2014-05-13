using UnityEngine;
using Oscuridad.Estados;

namespace Oscuridad.Estados
{
	public class ControladorBase : MonoBehaviour 
	{
		//Instancia de la clase (Singleton)
		[HideInInspector]
		public static ControladorBase instanceRef;

		//Variables publicas o privadas

		void Awake()
		{
			if(instanceRef == null)
			{
				instanceRef = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				DestroyImmediate(gameObject);
			}
		}
		
		void Start()
		{

		}
	}
}
