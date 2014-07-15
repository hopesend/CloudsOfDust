using UnityEngine;
using System.Collections;

[System.Serializable]
public class DBMaster{

    public DBPersonajens DBpersonajes;

    private static DBMaster instanceRef;

    public static DBMaster InstanceRef()
    {
        if (instanceRef == null)
        {
            instanceRef = new DBMaster();
        }

        return instanceRef;
     }

    private DBMaster()
    {
        DBpersonajes = new DBPersonajens();
    }
}