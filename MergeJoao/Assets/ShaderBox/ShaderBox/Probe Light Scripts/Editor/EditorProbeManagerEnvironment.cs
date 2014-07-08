using UnityEngine;
using UnityEditor;
using ProbesEnvironment;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Editor for creating and editing probes light for the characters.
/// </summary>

[CustomEditor(typeof(ProbeManagerEnvironment))]
public class EditorProbeManagerEnvironment : Editor
{
    private ProbeManagerEnvironment _probeManagerEnv;
    private Grid currentGrid;
    private Probe currentProbe;
    private Collider currentCollider;
    private int indxCurrentGrid = 0;
    private List<Collider> collidersProbes = new List<Collider>();

    private Material matProbe;
    private Shader shaderProbe;
    private Rect rectLastWin;

    #region "Initialization and disable of manager probes"

    protected void OnEnable() 
    {   
        _probeManagerEnv = target as ProbeManagerEnvironment;

        if ( _probeManagerEnv.gridProbes != null )
        if ( _probeManagerEnv.gridProbes.Length != 0 )
        {
            currentGrid = _probeManagerEnv.gridProbes[0];
            currentGrid.gizmo = true;
        }

        CreateSphereCollidersForProbs(false);

        shaderProbe = (Shader)Resources.Load("ProbePreview");
        matProbe = new Material(shaderProbe);

    }

    protected void OnDisable ()
    {
        DeleteColliders();
    }

    #endregion

    #region "GUI"

    public override void OnInspectorGUI()
    {
        if (Application.isPlaying && collidersProbes.Count > 0)DeleteColliders();

        if (_probeManagerEnv == null) return;
        GUIContent content = new GUIContent("");

        GUIStyle style = GUI.skin.GetStyle("Box");
        style.normal.textColor = Color.gray;

        EditorGUILayout.Separator();

        content.text = "Generator Grids";
        content.tooltip = "The generation of grids ambient samples, on the basis of mesh.";
        GUILayout.Label(content, style, GUILayout.ExpandWidth(true));

        content.text = "    Floor Mesh ";
        content.tooltip = "Specify the GameObject from the scene, the mesh GO should be only where the character can walk, the mesh must have a Layer.";
        _probeManagerEnv.floorForProbe = (Transform)EditorGUILayout.ObjectField(content, _probeManagerEnv.floorForProbe, typeof(Transform), true);

        content.text = "    Width Probe";
        content.tooltip = "Step size of the cell probe, the size of the axis X and Z, it can only be square.";
        _probeManagerEnv.widhtCell = EditorGUILayout.FloatField(content, _probeManagerEnv.widhtCell);
        content.text = "    Height Probe";
        content.tooltip = "The height of the cell probe.";
        _probeManagerEnv.heightCell = EditorGUILayout.FloatField(content, _probeManagerEnv.heightCell);
        content.text = "    Count GridXZ";
        content.tooltip = "For a number of nets split mesh for axis XZ.";
        _probeManagerEnv.splitGridX = EditorGUILayout.FloatField(content, _probeManagerEnv.splitGridX);
        content.text = "    Count GridY";
        content.tooltip = "For a number of nets split mesh for axis Z.";
        _probeManagerEnv.splitGridZ = EditorGUILayout.FloatField(content, _probeManagerEnv.splitGridZ);
        content.text = "    Layer Mask";
        content.tooltip = "Indicate a layer of mesh.";
        _probeManagerEnv.layerFloorProbe = EditorGUILayout.LayerField(content, _probeManagerEnv.layerFloorProbe);

        content.text = " Build Grids ";
        content.tooltip = "Divide the mesh to mesh.";
        if (GUILayout.Button(content))
        {
            BuildGrids(_probeManagerEnv.floorForProbe);
            GizmoAllShow(_probeManagerEnv.showAllGrids);
        }

        if (_probeManagerEnv == null) return;
        if (_probeManagerEnv.gridProbes == null) return;
        if (_probeManagerEnv.gridProbes.Length == 0) return;

        content.text = " Remove all grids ";
        content.tooltip = "Delete all grids.";
        if (GUILayout.Button(content))
            RemoveAllGrids();

        EditorGUILayout.Separator();

        content.text = "Bake Probes ";
        content.tooltip = "Bake environment light.";
        if (GUILayout.Button(content, GUILayout.Height(40f)))
            if (_probeManagerEnv.gridProbes.Length > indxCurrentGrid)
                if (_probeManagerEnv.showAllGrids)
            {
                foreach (Grid grid in _probeManagerEnv.gridProbes)
                    CreateGridProbes(grid);
                SceneView.RepaintAll();
            } else
            {
                CreateGridProbes(_probeManagerEnv.gridProbes[indxCurrentGrid]);
                SceneView.RepaintAll();
            }


        EditorGUILayout.Separator();

        GUILayout.Label("   Edit Grids", style, GUILayout.ExpandWidth(true));

        EditorGUILayout.Separator();
        /*
        if (GUILayout.Button(" Add Grid "))
        {
            AddGrid();
            currentGrid.AddProbe(new Probe());
        }
        */
        content.text = "Delete Grid  ";
        content.tooltip = "Deletes the selected grid.";
        if (GUILayout.Button(content))
        {
            DeleteGrid();
        }

        EditorGUILayout.Separator();
        content.text = "Current Grids ";
        content.tooltip = "Choosing the grid, if you choose Show All Grid, it will only display this grid.";
        indxCurrentGrid = EditorGUILayout.Popup("   Current Grids ", indxCurrentGrid, _probeManagerEnv.GetGridsName());

        content.text = "   Show All Grid ";
        content.tooltip = "If the mark is, it displays all the nets with the probes.";
        _probeManagerEnv.showAllGrids = EditorGUILayout.Toggle(content, _probeManagerEnv.showAllGrids);

        EditorGUILayout.TextField("   Count probe", CountProbes().ToString());

        if (indxCurrentGrid < 0) return;        

        // change grid
        if (_probeManagerEnv.gridProbes != null)
            if (indxCurrentGrid < _probeManagerEnv.gridProbes.Length)
                if (_probeManagerEnv.gridProbes[indxCurrentGrid] != currentGrid)
                {
                    //CreateSphereCollidersForProbs(true);
                    currentGrid = _probeManagerEnv.gridProbes[indxCurrentGrid];
                }

        content.text = "   Name Curr Grid ";
        content.tooltip = "Here you can change the name of the selected grid.";
        if (currentGrid != null)
            currentGrid.name = EditorGUILayout.TextField(content, currentGrid.name);

        EditorGUILayout.Separator();

        content.text = "    Probe Edit";
        content.tooltip = "";
        GUILayout.Label(content, style, GUILayout.ExpandWidth(true));

        _probeManagerEnv.brightnessProbe = EditorGUILayout.FloatField("   Brightness Probe", _probeManagerEnv.brightnessProbe);
        _probeManagerEnv.saturationProbe = EditorGUILayout.FloatField("   Saturation Probe", _probeManagerEnv.saturationProbe);

        EditorGUILayout.Separator();
        content.text = "Reset local parameters of probe";
        content.tooltip = "Make the local parameters of probe by default.";
        if (GUILayout.Button(content))
            ResetParametersProbe();

        if (GUI.changed)
            GizmoAllShow(_probeManagerEnv.showAllGrids);
    }

    protected void OnSceneGUI()
    {

        if (_probeManagerEnv == null) return;

        if (currentProbe != null)
        if (currentCollider != null)
            currentCollider.transform.position = currentProbe.position;

        if (Event.current.type == EventType.mouseDown && Event.current.button == 1)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                
                if (hit.transform.name == "ProbeCollider")
                {
                    if (_probeManagerEnv.gridProbes == null) return;
                    if (_probeManagerEnv.gridProbes.Length == 0) return;

                    foreach (Grid grid in _probeManagerEnv.gridProbes)
                    foreach (Probe probe in grid.probes)
                    {
                        if (probe.position == hit.transform.position)
                        {
                            currentProbe = probe;
                            currentGrid = GetGrid(currentProbe.position);
                            indxCurrentGrid = _probeManagerEnv.GetGridNum(currentGrid);
                            currentCollider = hit.collider;
                            break;
                        }
                    }
                }
            }
        }


        Shader defaultShader = HandleUtility.handleMaterial.shader;

        int i = 0;
        if (_probeManagerEnv.gridProbes != null)
        foreach (Grid gridProbe in _probeManagerEnv.gridProbes)
        {
            if (gridProbe.gizmo)
            {
                
                foreach (Probe probe in gridProbe.probes)
                {
                    if (matProbe)
                    {
                        Material mat = HandleUtility.handleMaterial;
                        mat.shader = matProbe.shader;
                        mat.SetColor("_ColorX", probe.colorX);
                        mat.SetColor("_ColorY", probe.colorY);
                        mat.SetColor("_ColorZ", probe.colorZ);
                        mat.SetColor("_ColorNX", probe.colorNX);
                        mat.SetColor("_ColorNY", probe.colorNY);
                        mat.SetColor("_ColorNZ", probe.colorNZ);
                        mat.SetFloat("_Amount", _probeManagerEnv.radius);
                    }

                    Handles.SphereCap(i, probe.position, Quaternion.identity, _probeManagerEnv.radius);

                    i++;
                }
            }
        }

        HandleUtility.handleMaterial.shader = defaultShader;
        
        if (currentProbe != null)
            currentProbe.position = Handles.PositionHandle(currentProbe.position, Quaternion.identity);

        Handles.BeginGUI();

        GUILayout.Window(0, new Rect(0f, 17f, 210f, 165f), ParametersProbeLight, "Setting selected Light Probe", "Window", GUILayout.MaxWidth(150f), GUILayout.MaxHeight(150f));

        Handles.EndGUI();
    }

    /// <summary>
    ///  window with parameters of the probe
    /// </summary>
    /// <param name="idWindow"></param>
    private void ParametersProbeLight(int idWindow)
    {

        GUI.depth = 1000;
        if (GUILayout.Button("Copy Probe"))
        {
            Vector3 pos = currentProbe.position;
            currentProbe = new Probe();
            currentProbe.position = pos;
            currentGrid = GetGrid(currentProbe.position);
            currentGrid.AddProbe(currentProbe);
            currentCollider = null;
        }

        if (GUILayout.Button("Delete Probe"))
        {
            _probeManagerEnv.DeleteProbe(currentProbe);
            currentProbe = null;
        }

        _probeManagerEnv.radius = Mathf.Clamp(EditorGUILayout.FloatField("Radius Sphere", _probeManagerEnv.radius), -0.25f, 100f);
        if (currentProbe != null)
        {
            currentProbe.brightnessProbe = EditorGUILayout.FloatField("Brightness Probe", currentProbe.brightnessProbe);
            currentProbe.saturationProbe = EditorGUILayout.FloatField("Saturation Probe", currentProbe.saturationProbe);
        }
        else
        {
            EditorGUILayout.FloatField("Brightness Probe", 0);
            EditorGUILayout.FloatField("Saturation Probe", 0);
        }


        if (GUILayout.Button("Bake light probe"))
            CreateGridProbes(null, currentProbe);

        GUILayout.Label("Selection a light probe, right-click.");
        // the button need for clickable the windows
        GUI.Button(new Rect(0f, 0f, 215f, 180f), "", "Box");
    }

    #endregion

    #region "Create of grids probes"

    protected void CreateGridProbes( Grid gridProbes = null, Probe probeBake = null )
    {
        Camera cam = null;
        Cubemap cubeMap = null;

        if (!cam)
        {
            var go = new GameObject("CubemapCamera", typeof(Camera));
            go.hideFlags = HideFlags.HideAndDontSave;
            go.transform.rotation = Quaternion.identity;
            cam = go.camera;

            cam.nearClipPlane = 0.01f;
            cam.farClipPlane = 100f;
            cam.enabled = false;

            cam.clearFlags = CameraClearFlags.Skybox;
            cam.backgroundColor = Color.green;

        }

        if (!cubeMap)
        {
            cubeMap = new Cubemap(64, TextureFormat.ARGB32, true);
            cubeMap.hideFlags = HideFlags.HideAndDontSave;
            cubeMap.Apply();
        }

        if (probeBake != null)
            ProbeBake(cam, cubeMap, probeBake);

       if (gridProbes != null)
       foreach (Probe probe in gridProbes.probes)
           ProbeBake(cam, cubeMap, probe);
    }

    private void ProbeBake(Camera cam, Cubemap cubeMap, Probe probe)
    {
        cam.transform.position = probe.position;
        cam.RenderToCubemap(cubeMap, 63);

        probe.colorX = GetFaceColor(cubeMap, CubemapFace.PositiveX, probe);
        probe.colorY = GetFaceColor(cubeMap, CubemapFace.PositiveY, probe);
        probe.colorZ = GetFaceColor(cubeMap, CubemapFace.PositiveZ, probe);
        probe.colorNX = GetFaceColor(cubeMap, CubemapFace.NegativeX, probe);
        probe.colorNY = GetFaceColor(cubeMap, CubemapFace.NegativeY, probe);
        probe.colorNZ = GetFaceColor(cubeMap, CubemapFace.NegativeZ, probe);

        float ambientPower = 1.5f;

        probe.colorX = probe.colorX * ambientPower;
        probe.colorY = probe.colorX * ambientPower;
        probe.colorZ = probe.colorZ * ambientPower;
        probe.colorNX = probe.colorNX * ambientPower;
        probe.colorNY = probe.colorNY * ambientPower;
        probe.colorNZ = probe.colorNZ * ambientPower;
    }

    private Color GetFaceColor(Cubemap cubeMap, CubemapFace face, Probe probe)
    {
        Color[] colors = cubeMap.GetPixels(face);
        Color final = Color.white;

        if (colors.Length > 0)
        {
            final = colors[0];
            for (int i = 1; i < colors.Length; i++)
            {
                final += (colors[i] * (colors[i].a * 2f));
            }
        }


        return ColorHelper.Saturation((final / colors.Length), _probeManagerEnv.saturationProbe * probe.saturationProbe) * (_probeManagerEnv.brightnessProbe * probe.brightnessProbe);
    }

    public void ResetParametersProbe()
    {
        foreach (Grid grid in _probeManagerEnv.gridProbes)
        {
            foreach (Probe probe in grid.probes)
            {
                probe.brightnessProbe = 1f;
                probe.saturationProbe = 1f;
                CreateGridProbes(null, probe);
            }
        } 
    }

    public void CreateSphereCollidersForProbs( bool onlyCurrGrid )
    {
        if (_probeManagerEnv == null) return;
        if (_probeManagerEnv.gridProbes == null) return;
        if (_probeManagerEnv.gridProbes.Length == 0) return;

        if ( onlyCurrGrid )
        {
            if (currentGrid != null)
                foreach (Probe probe in currentGrid.probes)
                {
                    GameObject go = new GameObject("ProbeCollider");
                    go.hideFlags = HideFlags.HideAndDontSave;
                    go.transform.position = probe.position;
                    collidersProbes.Add(go.AddComponent<SphereCollider>());
                }
        } else
        {
            foreach (Grid currGrid in _probeManagerEnv.gridProbes)
                foreach (Probe probe in currGrid.probes)
                {
                    GameObject go = new GameObject("ProbeCollider");
                    go.hideFlags = HideFlags.HideAndDontSave;
                    go.transform.position = probe.position;
                    collidersProbes.Add(go.AddComponent<SphereCollider>());
                }
        }
    }

    public void RemoveAllGrids()
    {
        _probeManagerEnv.gridProbes = null;
        currentGrid = null;
        indxCurrentGrid = 0;
    }

    /// <summary>
    /// Counting the number of probes in this system of grids
    /// </summary>
    /// <returns></returns>
    public int CountProbes()
    {
        int totalCountProbes = 0;

        if (_probeManagerEnv.gridProbes != null)
        {
            if (_probeManagerEnv.showAllGrids)
            {
                foreach (Grid grid in _probeManagerEnv.gridProbes)
                    totalCountProbes += grid.probes.Length;
            }
            else
            {
                totalCountProbes = currentGrid.probes.Length;
            }
        }

        return totalCountProbes;
    }

    public void GizmoAllShow( bool show )
    {
        if (_probeManagerEnv.gridProbes == null ) return;

        foreach (Grid g in _probeManagerEnv.gridProbes)
        {
            g.gizmo = show;
        }

        if( currentGrid != null )
            currentGrid.gizmo = true;
    }

    public void DeleteColliders()
    {
        for (int i = 0; i < collidersProbes.Count; i++)
        {
            if (collidersProbes[i] != null) DestroyImmediate(collidersProbes[i].gameObject);
        }

        collidersProbes.Clear();
    }

    private Grid  AddGrid()
    {
        return AddGrid( "Name grid", Vector3.zero, Vector3.zero);
    }

    private Grid AddGrid( string name, Vector3 center, Vector3 size)
    {
        if (currentGrid != null)
            currentGrid.gizmo = false;

        ArrayList grids = new ArrayList(0);

        if (_probeManagerEnv.gridProbes != null)
            grids = new ArrayList(_probeManagerEnv.gridProbes);

        Grid newGrid = new Grid();
        newGrid.name = name;
        newGrid.center = center;
        newGrid.size = size;

        grids.Add(newGrid);

        _probeManagerEnv.gridProbes = grids.ToArray(typeof(Grid)) as Grid[];
        currentGrid = newGrid;
        currentGrid.gizmo = true;

        return currentGrid;
    }

    private void DeleteGrid()
    {
        DeleteGrid(currentGrid);
    }

    private void DeleteGrid( Grid grid )
    {
        ArrayList listProbes = new ArrayList(_probeManagerEnv.gridProbes);
        listProbes.Remove(grid);
        _probeManagerEnv.gridProbes = listProbes.ToArray(typeof(Grid)) as Grid[];

        if (_probeManagerEnv.gridProbes.Length != 0)
        {
            currentGrid = _probeManagerEnv.gridProbes[0] as Grid;
            currentGrid.gizmo = true;
        }
    }

    private Matrix4x4 matx;

    /// <summary>
    /// Retrace collider "floorMesh". Creating a grid of probes on based trace data.
    /// </summary>
    /// <param name="floorMesh"></param>
    private void BuildGrids( Transform floorMesh )
    {
        if ( floorMesh == null ) return;

        //floorMesh.gameObject.layer = LayerMask.NameToLayer("FloorProbe");
        floorMesh.gameObject.layer = _probeManagerEnv.layerFloorProbe;

        if (floorMesh.GetComponent<MeshCollider>() == null)
            floorMesh.gameObject.AddComponent<MeshCollider>();

        matx = Matrix4x4.TRS(floorMesh.transform.position, floorMesh.rotation, Vector3.one);

        MeshFilter filterMesh = floorMesh.GetComponent<MeshFilter>();
        Bounds boundsFloor = filterMesh.sharedMesh.bounds;

        Vector3 extents = new Vector3(Mathf.Abs(matx.MultiplyVector(boundsFloor.extents).x), Mathf.Abs(matx.MultiplyVector(boundsFloor.extents).y), Mathf.Abs(matx.MultiplyVector(boundsFloor.extents).z));
        Vector3 center = matx.MultiplyPoint(boundsFloor.center);
        Vector3 size = new Vector3(Mathf.Abs(matx.MultiplyVector(boundsFloor.size).x), Mathf.Abs(matx.MultiplyVector(boundsFloor.size).y), Mathf.Abs(matx.MultiplyVector(boundsFloor.size).z));
        float yBounds = center.y + _probeManagerEnv.heightCell * 0.5f;

        Vector3 startPoint = new Vector3(center.x + extents.x - (_probeManagerEnv.widhtCell * 0.5f), 3000f, center.z+ extents.z - (_probeManagerEnv.widhtCell * 0.5f));

        int xSizeFloor = (int)(size.x / _probeManagerEnv.widhtCell);
        int zSizeFloor = (int)(size.z / _probeManagerEnv.widhtCell);

        float widthX = size.x / _probeManagerEnv.splitGridX;
        float widthZ = size.z / _probeManagerEnv.splitGridZ;

        int xNumGrid = Mathf.CeilToInt( size.x / widthX );
        int zNumGrid = Mathf.CeilToInt( size.z / widthZ );

        float currWidthX = center.x + extents.x, currWidthZ = center.z + extents.z;

        currWidthX -= widthX;

        for (int i = 0; i < xNumGrid; i++)
        {
            for (int j = 0; j < zNumGrid; j++)
            {
                currWidthZ -= widthZ;
                AddGrid("_GenerateGrid" + i.ToString() + j.ToString(), new Vector3(currWidthX + (widthX * 0.5f), yBounds, currWidthZ + (widthZ * 0.5f)), new Vector3(widthX, size.y + _probeManagerEnv.heightCell, widthZ));

            }

            currWidthX -= widthX;
            currWidthZ = center.z + extents.z;
        }

        List<Vector3> points = new List<Vector3>();
        Vector3 pos = startPoint;

        for (int i = 0; i < xSizeFloor; i++)
        {
            for (int j = 0; j < zSizeFloor; j++)
            {
                pos.z -= _probeManagerEnv.widhtCell;
                Vector3[] posProbs = RecastFloor(pos);
                if (posProbs != null)
                    points.AddRange(posProbs);
            }
            pos.z = startPoint.z;
            pos.x -= _probeManagerEnv.widhtCell;
        }
        if (_probeManagerEnv.gridProbes == null)
        {
            Debug.LogWarning("Has not been created not one light probe.");
            return;
        }
        if (_probeManagerEnv.gridProbes.Length == 0)
        {
            Debug.LogWarning("Has not been created not one light probe.");
            return;
        }
        
        Grid grid = _probeManagerEnv.gridProbes[0];

        foreach (Vector3 point in points)
        {
            Probe probe = new Probe();
            probe.position = point;

            grid = GetGrid(point);

            if (grid == null) continue;

            grid.AddProbe(probe);
        }

        for (int i = 0; i < _probeManagerEnv.gridProbes.Length; i++)
        {
            if (_probeManagerEnv.gridProbes[i].probes.Length == 0)
            {
                DeleteGrid(_probeManagerEnv.gridProbes[i]);
                i--;
            }
        }

        CreateSphereCollidersForProbs(false);
    
    }

    public Grid GetGrid( Vector3 point)
    {
        Grid grid = _probeManagerEnv.gridProbes[0];
        float widthX = grid.size.x * 0.5f;
        float widthZ = grid.size.z * 0.5f;

        foreach (Grid g in _probeManagerEnv.gridProbes)
        {
            if ((g.center.x + widthX) > point.x)
                if ((g.center.x - widthX) < point.x)
                    if ((g.center.z + widthZ) > point.z)
                        if ((g.center.z - widthZ) < point.z)
                            return g;
        }

        return null;
    }

    private Vector3[] RecastFloor( Vector3 point )
    {
        Ray ray = new Ray(point,Vector3.down);
        
        List<RaycastHit> recastHit = new List<RaycastHit>();

        for (int i = 0; i < 10; i++)
        {
            RaycastHit hit = new RaycastHit();

            if (!Physics.Raycast(ray, out hit, 4000f, 1 << _probeManagerEnv.layerFloorProbe.value))
                break;
            else
            {
                recastHit.Add(hit);
                Vector3 _point = hit.point;
                _point.y -= _probeManagerEnv.heightCell * 0.6f;
                ray.origin = _point;
            }
        }
        
        if (recastHit.Count == 0)
            return null;

        List<Vector3> pointsHit = new List<Vector3>();
        Vector3 lastPoint = Vector3.zero;

        foreach (RaycastHit rhit in recastHit)
        {
            if (Vector3.Distance(lastPoint, rhit.point) > 0f)
            {
                lastPoint = new Vector3(rhit.point.x, rhit.point.y + (_probeManagerEnv.heightCell * 0.5f), rhit.point.z);
                pointsHit.Add( lastPoint );
            }
        }

        return pointsHit.ToArray();
    }

   [MenuItem("Window/Editor Probs Environment")]
   static void CreateEditorProbs()
   {
       GameObject editor = new GameObject("ManagerProbsEnv");
       editor.AddComponent("ProbeManagerEnvironment");
   }
    
    #endregion
}

