using UnityEngine;
using System.Collections;

public class Combo {

    private float timeCombo;

    public float TimeCombo
    {
        get { return timeCombo; }
        set { timeCombo = value; }
    }
    private string[] listCombo;

    public string[] ListCombo
    {
        get { return listCombo; }
        set { listCombo = value; }
    }

    public Combo(string[] teclas, float tiempo)
    {
        listCombo = teclas;
        timeCombo = tiempo;
    }

}
