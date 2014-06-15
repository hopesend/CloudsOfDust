using UnityEngine;
using ProbesEnvironment;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[System.Serializable]
public class ProbeManagerEnvironment : MonoBehaviour
{
    public Grid[] gridProbes = new Grid[0];
    public Transform floorForProbe;
    public float widhtCell = 1.2f;
    public float heightCell = 2.5f;
    public float splitGridX = 2f;
    public float splitGridZ = 2f;
    public LayerMask layerFloorProbe = 1 << 0;
    public bool showAllGrids = true;
    public float brightnessProbe = 1f;
    public float saturationProbe = 1f;
    public float radius = 0.05f;

    public string[] GetGridsName()
    {
        ArrayList temp = new ArrayList();

        if (gridProbes == null) return new string[1] { "empty" };

        foreach (Grid grid in gridProbes)
        {
            temp.Add(grid.name);
        }

        if (temp.Count != 0)
            return (string[])temp.ToArray(typeof(string));
        else return new string[1]{"empty"};
    }

    public int GetGridNum( Grid grid )
    {
        for (int i = 0; i < gridProbes.Length; i++)
        {
            if (gridProbes[i] == grid)
                return i;
        }
        return -1;
    }

    public void DeleteProbe( Probe probe )
    {
        if (probe == null ) return;

        for (int i = 0; i < gridProbes.Length; i++)
        {
            List<Probe> probes = new List<Probe>(gridProbes[i].probes);
            bool remove = probes.Remove(probe);
            gridProbes[i].probes = probes.ToArray();
            if (remove) break;
        }
    }

    protected void OnDrawGizmosSelected()
    {
        if (gridProbes == null) return;

        foreach (Grid grid in gridProbes)
        {
            if (grid.gizmo)
                Gizmos.DrawWireCube(grid.center, grid.size);
        }
    }
}

