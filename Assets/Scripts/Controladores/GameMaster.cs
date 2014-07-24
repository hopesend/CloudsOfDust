using UnityEngine;
using System.Collections;
using System.IO;
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

    private static GameMaster instanceRef;

    public static GameMaster InstanceRef
    {
        get 
        {
            if (instanceRef != null)
            {
                return instanceRef;
            }
            else
            {
                GameObject go = Resources.Load("GameMaster", typeof(GameObject)) as GameObject;
                GameObject instaciado = Object.Instantiate(go) as GameObject;

                instanceRef = instaciado.GetComponent<GameMaster>();
            }

            return instanceRef; 
        }
    }

    void Awake()
    {
        if (instanceRef == null)
        {
            instanceRef = this;
            DontDestroyOnLoad(this);
            InitHandlers();
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    void InitHandlers()
    {
        //-------------- Inicializo los Handlers ----------------\\
        controladoraMundo = ControladorBaseMundo.InstanceRef();
        controladoraBatalla = ControladoraBaseBatalla.InstanceRef();
        controladorJugador = ControladorJugador.InstanceRef();
        controladoraHUD = HUD.InstanceRef();
        hudBatalla = HUDBatalla.InstanceRef();
        hudBatalla.PrepararTexturas();
        pantallaInicial = pantallaInicial.SetEscenarioActual();
        
        

        controladoraNiveles.CambiarSceneSegunEnum(pantallaInicial);
        controladoraNiveles.estadoActivo.NivelCargado();

		Inicializar_Valores_XML ();
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

        BuscarOInstanciar(posJugador);
        
        
        
    }

    public void BuscarOInstanciar(Vector3 posJugador)
    {
        PersonajeControlable temp = GameObject.FindObjectOfType<PersonajeControlable>();
        if (temp == null)
        {
            controladoraMundo.AddPersonajeSeleccionado(InstanciarJugador(controladorDB.DBpersonajes.GetPersonajeByID(IDPersonajes.Trasher), posJugador), true);
        }
        else
        {
            controladorJugador.Cargar_Datos_XML(temp);
            temp.transform.position = posJugador;
            controladoraMundo.AddPersonajeSeleccionado(temp, true);
        }
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

			//Archivo para las conversaciones y textos que se mostraran en el juego
            origen = (TextAsset)Resources.Load("XML/spa/Conversaciones", typeof(TextAsset));
            destino = Path.Combine(UserPath, "Conversaciones.xml");
            if (!File.Exists(destino))
                Crear_Fichero(origen, destino);
			else
			{
				//TODO: quitar cuando se termine el juego
				File.Delete(destino);
				Crear_Fichero(origen, destino);
			}

			//Archivo para informacion sobre la HAPQ
			origen = (TextAsset)Resources.Load("XML/HAPQ", typeof(TextAsset));
			destino = Path.Combine(UserPath, "HAPQ.xml");
			if (!File.Exists(destino))
				Crear_Fichero(origen, destino);
			{
				//TODO: quitar cuando se termine el juego
				File.Delete(destino);
				Crear_Fichero(origen, destino);
			}
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
            //Console.WriteLine(ex.Message);
        }
    }

    public PersonajeControlable InstanciarJugador(DataDBPersonaje data, Vector3 pos)
    {
        PersonajeControlable temp = Instantiate(data.GO, pos, Quaternion.identity) as PersonajeControlable;
        temp.name = temp.Get_Nombre();
        controladorJugador.Cargar_Datos_XML(temp);
        return temp;
    }


}
