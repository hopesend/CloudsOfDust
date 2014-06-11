using UnityEngine;
using System.Collections;

public class ArmaMele : AbsItem, IArma {

    public ArmaMele()
    {
        rango = 0.5f;
    }

    private float damage;
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
}
