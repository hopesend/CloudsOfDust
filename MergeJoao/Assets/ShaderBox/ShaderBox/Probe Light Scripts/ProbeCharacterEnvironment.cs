
using UnityEngine;
using ProbesEnvironment;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Obtain a probe light for the object material.
/// </summary>
[ExecuteInEditMode]
public class ProbeCharacterEnvironment : MonoBehaviour
{
    public float timeLerp = 0.2f;

    private ProbeManagerEnvironment _probeManager;
    private Material[] mats;
    private Color[] cubeColor = new Color[6];
    private Probe lastProbe = null, currProbe = null;
    private float lerpTime, lastTime;

    private string[] shadersCharacter = new string[3]
                                           {
                                               "ShaderBox/Probe/___Low", "ShaderBox/Probe/__Middle",
                                               "ShaderBox/Probe/_High"
                                           };

    /// <summary>
    ///  Search all the mats to the desired shading on this site
    /// </summary>
    public void Start()
    {
        List<MeshRenderer> meshRends = new List<MeshRenderer>();
        List<SkinnedMeshRenderer> meshSkinRends = new List<SkinnedMeshRenderer>();

        MeshRenderer mRenderer = renderer as MeshRenderer;
        if (mRenderer != null) meshRends.Add(mRenderer);
        SkinnedMeshRenderer mSkinRenderer = renderer as SkinnedMeshRenderer;
        if (mSkinRenderer != null) meshSkinRends.Add(mSkinRenderer);

        if (transform.root != null)
        {

            MeshRenderer[] renderers = transform.root.GetComponentsInChildren<MeshRenderer>(true);
            SkinnedMeshRenderer[] skinRenderers = transform.root.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            meshRends.AddRange(renderers);
            meshSkinRends.AddRange(skinRenderers);         
        }

        ArrayList m = new ArrayList();

        foreach (MeshRenderer meshRenderer in meshRends)
        {
            List<Material> findMats = new List<Material>();

            if ( Application.isPlaying )
                findMats.AddRange(meshRenderer.materials);
            else findMats.AddRange(meshRenderer.sharedMaterials);

            foreach (Material mat in findMats)
            {
                if (shadersCharacter[0] == mat.shader.name || shadersCharacter[1] == mat.shader.name || shadersCharacter[2] == mat.shader.name)
                    m.Add(mat);
            }
        }

        foreach (SkinnedMeshRenderer meshRenderer in meshSkinRends)
        {
            List<Material> findMats = new List<Material>();

            if (Application.isPlaying)
                findMats.AddRange(meshRenderer.materials);
            else findMats.AddRange(meshRenderer.sharedMaterials);

            foreach (Material mat in findMats)
            {
                if (shadersCharacter[0] == mat.shader.name || shadersCharacter[1] == mat.shader.name || shadersCharacter[2] == mat.shader.name)
                    m.Add(mat);
            }
        }

        mats = m.ToArray(typeof(Material)) as Material[];
    }


    // Update can be replaced by coroutine, enable / disable
    // depending on the distance of the object to the camera.
    protected void Update()
    {
        UpdateProbeColor();
    }

    public void UpdateProbeColor()
    {
        if (!_probeManager)
        {
            GameObject managerObject = GameObject.Find("ManagerProbsEnv");

            if (managerObject != null)
                _probeManager = managerObject.GetComponent<ProbeManagerEnvironment>();
        }

        if (!_probeManager)
        {
            Debug.Log("Not found manager Probe Environment!");
            return;
        }

        if (_probeManager.gridProbes == null) return;

        Vector3 pos = transform.position;
        float minDist = 50000f, dist;
        Probe nearProbe = null;

        // grid search in which we are
        Grid grid = GetGrid(pos);

        // find the nearest probe light
        if (grid != null)
        {
            foreach (Probe probe in grid.probes)
            {
                dist = Vector3.Distance(probe.position, pos);

                if (dist < minDist)
                {
                    minDist = dist;
                    nearProbe = probe;
                }
            }
        }

        // switch over between probe light during timeLerp
        if (currProbe == null)
            currProbe = nearProbe;

        if (nearProbe != currProbe)
        {
            lastProbe = currProbe;
            currProbe = nearProbe;
            lastTime = Time.realtimeSinceStartup;
        }

        if (currProbe != null)
        {
            Color[] lerpProbe = new Color[6];

            if (currProbe != lastProbe && lastProbe != null)
            {
                lerpTime = (Time.realtimeSinceStartup - lastTime)*(1f/timeLerp);

                lerpProbe[0] = Color.Lerp(cubeColor[0], currProbe.colorX, lerpTime);
                lerpProbe[1] = Color.Lerp(cubeColor[1], currProbe.colorY, lerpTime);
                lerpProbe[2] = Color.Lerp(cubeColor[2], currProbe.colorZ, lerpTime);
                lerpProbe[3] = Color.Lerp(cubeColor[3], currProbe.colorNX, lerpTime);
                lerpProbe[4] = Color.Lerp(cubeColor[4], currProbe.colorNY, lerpTime);
                lerpProbe[5] = Color.Lerp(cubeColor[5], currProbe.colorNZ, lerpTime);

                if (lerpTime >= 1f)
                {
                    lastProbe = currProbe;
                    lerpTime = 0f;
                    lastTime = 0f;
                }

                cubeColor = lerpProbe;

            }
            else
            {
                cubeColor[0] = currProbe.colorX;
                cubeColor[1] = currProbe.colorY;
                cubeColor[2] = currProbe.colorZ;
                cubeColor[3] = currProbe.colorNX;
                cubeColor[4] = currProbe.colorNY;
                cubeColor[5] = currProbe.colorNZ;
            }

            // set data to the shader
            if (mats != null)
                foreach (Material mat in mats)
                {
                    mat.SetColor("_ColorX", cubeColor[0]);
                    mat.SetColor("_ColorY", cubeColor[1]);
                    mat.SetColor("_ColorZ", cubeColor[2]);
                    mat.SetColor("_ColorNX", cubeColor[3]);
                    mat.SetColor("_ColorNY", cubeColor[4]);
                    mat.SetColor("_ColorNZ", cubeColor[5]);
                }
        }
    }

    // get a grid that contains this object
    public Grid GetGrid(Vector3 point)
    {
        if (_probeManager.gridProbes == null) return null;
        if (_probeManager.gridProbes.Length == 0) return null;
        Grid grid = _probeManager.gridProbes[0];
        float widthX = grid.size.x * 0.5f;
        float widthZ = grid.size.z * 0.5f;

        foreach (Grid g in _probeManager.gridProbes)
        {
            if ((g.center.x + widthX) > point.x)
                if ((g.center.x - widthX) < point.x)
                    if ((g.center.z + widthZ) > point.z)
                        if ((g.center.z - widthZ) < point.z)
                            return g;
        }

        return null;
    }
}

