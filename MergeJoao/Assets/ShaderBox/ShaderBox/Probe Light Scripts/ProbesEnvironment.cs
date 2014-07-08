
using UnityEngine;
using System.Collections;

namespace ProbesEnvironment
{

    [System.Serializable]
    public class Grid
    {
        public string name = "";
        public Probe[] probes = new Probe[0];
        public bool gizmo = false;
        public Vector3 center;
        public Vector3 size;

        public void AddProbe( Probe probe)
        {
            ArrayList arrayList = new ArrayList(probes);
            arrayList.Add(probe);

            probes = arrayList.ToArray(typeof(Probe)) as Probe[];
        }

        public void DeleteProbe(Probe probe)
        {
            ArrayList arrayList = new ArrayList(probes);
            arrayList.Remove(probe);

            probes = arrayList.ToArray(typeof(Probe)) as Probe[];
        }

    }


    [System.Serializable]
    public class Probe
    {
        public float brightnessProbe = 1f;
        public float saturationProbe = 1f;
        public Color colorX, colorY, colorZ, colorNX, colorNY, colorNZ;
        public Vector3 position = Vector3.zero;
    }
}

