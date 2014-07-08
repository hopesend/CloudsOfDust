using UnityEngine;
using System.Collections;
using System.IO;
using System;
using Clouds.xml;
using System.Xml;

public class GameMaster : MonoBehaviour {

    private EstadoJuego estadoActual;

    public EstadoJuego EstadoActual
    {
        get { return estadoActual; }
        set 
        {
            estadoActual = value;
        }
    }

    public ControladorBaseMundo controladoraMundo;

    public ControladoraBaseBatalla controladoraBatalla;

    public ControladorJugador controladorJugador;

    public ControladorNiveles controladoraNiveles;

    public DBMaster controladorDB;

    public HUD controladoraHUD;

    public static GameMaster instanceRef;

    void Awake()
    {
        if (instanceRef == null)
        {
            instanceRef = this;
            DontDestroyOnLoad(this);
            Init();
        }
        else
        {
            Destroy(this);
        }
        
    }

    void Init()
    {
        //-------------- Inicializo los Handlers ----------------\\
        controladoraMundo = ControladorBaseMundo.InstanceRef();
        controladoraBatalla = ControladoraBaseBatalla.InstanceRef();
        controladorJugador = ControladorJugador.InstanceRef();
        controladoraHUD = HUD.InstanceRef();

        controladoraNiveles.IrMenuPrincipal();
    }

    public void IniciarPelea()
    {
        estadoActual = EstadoJuego.Batalla;
        controladoraBatalla.IniciarPelea(controladoraMundo.personajesSeleccionados, new System.Collections.Generic.List<Enemigo>());
        
        //Instantiate(controladorDB.DBpersonajes.GetPersonajeByID(IDPersonajes.Trasher).GO);
        controladoraBatalla.PrepararJugadorScripts();
    }

    void OnGUI()
    {
        controladoraNiveles.OnGUI();
        
        
    }

    void Update()
    {
        if (estadoActual == EstadoJuego.Batalla)
        {
            controladoraBatalla.Update(); 
        }
    }

    public void InicializarMundo(Vector3 posJugador)
    {
        controladoraMundo.AddPersonajeSeleccionado(Instantiate(controladorDB.DBpersonajes.GetPersonajeByID(IDPersonajes.Trasher).GO, posJugador, Quaternion.identity) as PersonajeControlable, true);
        
    }

    void OnLevelWasLoaded(int level)
    {
        controladoraNiveles.OnLevelWasLoaded(level);
    }

    public bool Inicializar_Valores_XML()
    {
        string UserPath = Application.persistentDataPath + @"/GlobalData/XML";

        //Creamos los directorios
        if (!Directory.Exists(UserPath))
            Directory.CreateDirectory(UserPath);

        try
        {
            string destino = null;
            TextAsset origen = null;

            //Creacion del xml de los textos
            origen = (TextAsset)Resources.Load("XML/spa/Conversaciones", typeof(TextAsset));
            destino = Path.Combine(UserPath, "Conversaciones.xml");
            if (!File.Exists(destino))
                Crear_Fichero(origen, destino);
        }
        catch
        {
            return false;
        }

        return true;
    }

    private void Crear_Fichero(TextAsset nuevoOrigen, string nuevoDestino)
    {
        try
        {
            StreamWriter sw = new StreamWriter(nuevoDestino, false);
            sw.Write(nuevoOrigen.text);
            sw.Close();
        }
        catch (IOException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public void Lanzar_Pantalla(int id)
    {
        /*switch (id)
        {
            Debug.Log("S");
            //case 1: ControladorNiveles.instanceRef.CambiarEstado(new EscenarioVecindario(Manager));
                break;
        }*/
    }


}
