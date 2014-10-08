using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ControladoraHAQP : MonoBehaviour 
{
	void Start()
	{
		AddMsj("Bienvenido", "Bienvenido Trasher a Clouds of Dust..."); //Test
		AddMsj("Titulo n2", "Mensaje n2"); //Test
		AddMsj("Titulo n3", "Mensaje n3"); //Test
		AddMsj("Titulo n4", "Mensaje n4"); //Test
		AddMsj("Titulo n5", "Mensaje n5"); //Test

		AddItem ("Magnanita"); //test
		AddItem ("Runa de poder"); // test

		SetCharacter ();
	}
	#region DEFAULT
	public List<PersonajeControlable> Character; //setearlo desde gamemaster
	public void SetCharacter()
	{
		for(int count = 0; count < Character.Count; count ++) 
		{
			GameObject.Find("Charactes").transform.GetChild(count).gameObject.name = Character[count].name;
		}
	}
	PersonajeControlable characterSelected; //para utilizar los botones mov, fisico y magia
	public void buttonCharacter(GameObject character)
	{
		for(int count = 0; count < Character.Count; count ++) 
		{
			if(character.name == Character[count].name)
			{
				characterSelected = Character[count];
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
	public void SeeStats(int stat)
	{

	}
	#endregion
	#region MSJ
	IList<message> listMsj = new List<message> ();
	GameObject textNewMessage;
	GameObject viewMessageRect;
	GameObject messageButton;
	GameObject messageRect;
	int count = -1; 
	public int status { get; set; }
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
	public void SetPage()
	{

	}
	public void UpdateMessage(int id)
	{
		ResetViewMsj ();
		textNewMessage = GameObject.Find("TextNew");
		int count = 0;
		if(listMsj.Count > 0)
		{
			for(int index = 0; index < listMsj.Count; index++)
			{
				if(listMsj[index].id == id)
				{
					GameObject message = GameObject.Find("MessageRect").transform.GetChild(count).gameObject;
					message.SetActive(true);
					message.transform.GetChild(0).GetComponent<Text>().text = listMsj[index].title;
					count++;
				}
				else
				{
					GameObject message = GameObject.Find("MessageRect").transform.GetChild(index).gameObject;
					message.SetActive(false);
				}
			}
		}
    }
	public void ResetViewMsj()
	{
		for(int index = 0; index < GameObject.Find("MessageRect").transform.childCount; index++)
		{
		    GameObject message = GameObject.Find("MessageRect").transform.GetChild(index).gameObject;
			message.SetActive(false);
		    count++;
		}
	}
	public void printTextOfMsj(Text text)
	{
		for(int index = 0; index < listMsj.Count; index++)
		{
			if(listMsj[index].title == text.text)
			{
				GameObject.Find("TextOfMsj").GetComponent<Text>().text = listMsj[index].text;

				if(listMsj[index].id != 2)
				{
				    message sendMsj = listMsj[index];
				    sendMsj.id = 1; //vistos
				    listMsj[index] = sendMsj;
				    break;
				}
			}
		}
	}
	public void AddFav(Text text)
	{
		for(int index = 0; index < listMsj.Count; index++)
		{
			if(listMsj[index].text == text.text)
			{
				message addFav = listMsj[index];
				addFav.id = 2; //favoritos
				listMsj[index] = addFav;
				UpdateMessage(status);
				break;   
			}
		}
	}
	public void DelFav(Text text)
	{
		for(int index = 0; index < listMsj.Count; index++)
		{
			if(listMsj[index].text == text.text)
			{
				message delFav = listMsj[index];
				delFav.id = 1; //favoritos
				listMsj[index] = delFav;
				break;   
			}
		}
	}
	#endregion
	#region ITEM
	int countItem = -1;
	public struct Item
	{
		public int id;
		public int count;
		public string name;
		public string content;
		public Texture2D image;
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
	#endregion

}
