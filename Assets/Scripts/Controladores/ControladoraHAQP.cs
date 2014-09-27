using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ControladoraHAQP : MonoBehaviour 
{
	void Start()
	{
		AddMsj("Bienvenido", "Bienvenido Trasher a Clouds of Dust..."); //Test
		AddMsj("Instrucciones", "Para ...."); //Test
		AddItem ("Magnanita");
		AddItem ("Runa de poder");

		SetCharacter ();
	}
	/// <summary>
	/// Initial
	/// </summary>
	public List<PersonajeControlable> Character; //setearlo desde gamemaster
	public void SetCharacter()
	{
		for(int count = 0; count < Character.Count; count ++) 
		{
			GameObject.Find("Charactes").transform.GetChild(count).gameObject.name = Character[count].name;
		}
	}
	public void buttonCharacter(GameObject character)
	{
		for(int count = 0; count < Character.Count; count ++) 
		{
			if(character.name == Character[count].name)
			{
				GameObject.Find ("Vitalidad").GetComponent<Text>().text = Character[count].Vitalidad.ToString();
				GameObject.Find ("Estamina").GetComponent<Text>().text = Character[count].Estamina.ToString();
				GameObject.Find ("PM").GetComponent<Text>().text = Character[count].PM.ToString();
				GameObject.Find ("Fuerza").GetComponent<Text>().text = Character[count].Fuerza.ToString();
				GameObject.Find ("Resistencia").GetComponent<Text>().text = Character[count].Resistencia.ToString();
				GameObject.Find ("Concentracion").GetComponent<Text>().text = Character[count].Concentracion.ToString();
				GameObject.Find ("Esp").GetComponent<Text>().text = Character[count].Espiritu.ToString();
				GameObject.Find ("Evasion").GetComponent<Text>().text = Character[count].Evasion.ToString();
				GameObject.Find ("Velocidad").GetComponent<Text>().text = Character[count].Rapidez.ToString();
				GameObject.Find ("Suerte").GetComponent<Text>().text = Character[count].Suerte.ToString();
				GameObject.Find ("Movimiento").GetComponent<Text>().text = Character[count].Movimiento.ToString();
			}
		}
	}
	/// <summary>
	/// Message
	/// </summary>
	IList<message> listMsj = new List<message> ();
	GameObject textNewMessage;
	GameObject viewMessageRect;
	GameObject messageButton;
	GameObject messageRect;
	int count = -1;

	public struct message
	{
		public int id;
		public int count;
		public string title;
		public string text;
	}
	public void AddMsj(string title, string text)
	{
		listMsj.Add (new message {id = 0, count = count++, title = title, text = text});
	}
	public void UpdateMessage()
	{
		textNewMessage = GameObject.Find("TextNew");
		int count = 0;
		if(listMsj.Count > 0)
		{
			for(int index = 0; index < listMsj.Count; index++)
			{
				if(listMsj[index].id == 0)
				{
					GameObject message = GameObject.Find("MessageRect").transform.GetChild(index).gameObject;
					message.SetActive(true);
					message.transform.GetChild(0).GetComponent<Text>().text = listMsj[index].title;
					count++;
				}
			}
		}
	}
	public void printTextOfMsj(Text text)
	{
		for(int index = 0; index < listMsj.Count; index++)
		{
			if(listMsj[index].title == text.text)
			{
				GameObject.Find("TextOfMsj").GetComponent<Text>().text = listMsj[index].text;

				message sendMsj = listMsj[index];
				sendMsj.id = 1; //?
				listMsj[index] = sendMsj;
				Debug.Log(listMsj[index].id);
				break;   
			}
		}
	}
	/// <summary>
	/// Inventary
	/// </summary>
	int countItem = -1;
	public struct Item
	{
		public int id;
		public int count;
		public string name;
	}
	IList<Item> Items = new List<Item> ();
	public void AddItem(string name)
	{
		Items.Add (new Item {id = 0, count = countItem++, name = name});
	}
	public void SetInventary()
	{
		if (Items.Count > 0)
		{
			for(int index = 0; index < Items.Count; index++)
			{
				GameObject items = GameObject.Find("Items").transform.GetChild(index).gameObject;
				items.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = Items[index].name;

			}
	    }
	}


}
