using UnityEngine;
using System.Collections;

public class ArmaRanged : AbsItem, IArma {

    

    float damage;
    public float Damage
    {
        
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    public float rango;
    public float Rango
    {
        get
        {
            return rango;
        }
        set
        {
            rango = value;
        }
    }


    public void Upgrade()
    {
        throw new System.NotImplementedException();
    }
}
