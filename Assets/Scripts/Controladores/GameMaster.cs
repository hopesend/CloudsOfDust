using UnityEngine;
using System.Collections;
using System.IO;
using System;
using Clouds.xml;
using System.Xml;

public class GameMaster : MonoBehaviour {
    //Esto sirve para debug nomas
    public ScenesParaCambio pantallaInicial;

    public EstadoJuego estadoActual;

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

    public HUDBatalla hudBatalla;

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
        hudBatalla = HUDBatalla.InstanceRef();
        hudBatalla.PrepararTexturas();

        controladoraNiveles.CambiarSceneSegunEnum(pantallaInicial);
        controladoraNiveles.estadoActivo.NivelCargado();
    }

    public void IniciarPelea()
    {
        estadoActual = EstadoJuego.Batalla;
        controladoraBatalla.IniciarPelea(controladoraMundo.personajesSeleccionados, new System.Collections.Generic.List<Enemigo>());
        

        controladoraBatalla.PrepararJugadorScripts();
    }

    void OnGUI()
    {
        controladoraNiveles.OnGUI();
		controladoraHUD.OnGUI ();   
        if (estadoActual == EstadoJuego.Batalla)
        {
            hudBatalla.OnGUI();
        }

    }

    void Update()
    {
        if (estadoActual == EstadoJuego.Batalla)
        {
            controladoraHUD.Update();
            controladoraBatalla.Update();
            hudBatalla.Update();
            
        }
    }

    /// <summary>
    /// Llamar este metodo solo una vez en todo el juego.
    /// </summary>
    /// <param name="posJugador"></param>
    public void InicializarMundo(Vector3 posJugador)
    {
        controladoraMundo.dinero = 100;
        controladoraMundo.AddPersonajeSeleccionado(InstanciarJugador(controladorDB.DBpersonajes.GetPersonajeByID(IDPersonajes.Trasher), posJugador), true); ;
        
        
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
    }

    private PersonajeControlable InstanciarJugador(DataDBPersonaje data, Vector3 pos)
    {
        PersonajeControlable temp = Instantiate(data.GO, pos, Quaternion.identity) as PersonajeControlable;
        controladorJugador.Cargar_Datos_XML(temp);
        return temp;
    }


}
