using UnityEngine;
using UnityEngine.UI;
public class InterfazCombate : MonoBehaviour
{
    [Header("Referencias")]
    public GestorCombate gestorCombate;

    [Header("Barras de vida")]
    public Slider barraVidaP1;
    public Slider barraVidaP2;

    [Header("Barras de energia")]
    public Slider barraEnergiaP1;
    public Slider barraEnergiaP2;

    [Header("Textos")]
    public Text textoVidaP1;
    public Text textoVidaP2;
    public Text textoEnergiaP1;
    public Text textoEnergiaP2;
    public Text textoTurno;
    public Text textoMensaje;
    public Text textoHabilidadesP1;
    public Text textoHabilidadesP2;

    void Start()
    {
        //configurar las barras al empezar para que tengan los valores maximos bien
        ConfigurarBarras();

        //actualizar la interfaz nada mas empezar la partida
        ActualizarInterfaz();
        //configurar los textos de habilidades de cada jugador
        ConfigurarTextosHabilidades();
    }

    void Update()
    {
        //actualizar la interfaz todo el rato para ver la vida y energia actual
        ActualizarInterfaz();
    }

    public void BotonPasoBasico()
    {
        //usar este metodo desde el boton del ataque basico
        gestorCombate.PasoBasico();
        MostrarMensaje("Paso basico");
    }

    public void BotonPasoEspecial()
    {
        //usar este metodo desde el boton del ataque especial
        gestorCombate.PasoEspecial();
        MostrarMensaje("Paso especial");
    }

    public void BotonDefender()
    {
        //usar este metodo desde el boton de defensa
        gestorCombate.Defender();
        MostrarMensaje("Defensa");
    }

    public void BotonUlti()
    {
        //usar este metodo desde el boton de la ulti
        gestorCombate.UsarUlti();
        MostrarMensaje("Subidon de Kumbia");
    }

    private void ConfigurarBarras()
    {
        //evitar errores si no esta puesto el gestor
        if (gestorCombate == null)
        {
            return;
        }

        //poner la vida maxima del jugador 1 en su barra
        if (barraVidaP1 != null)
        {
            barraVidaP1.maxValue = gestorCombate.luchadorP1.vidaMaxima;
        }

        //poner la vida maxima del jugador 2 en su barra
        if (barraVidaP2 != null)
        {
            barraVidaP2.maxValue = gestorCombate.luchadorP2.vidaMaxima;
        }

        //poner la energia maxima del jugador 1
        if (barraEnergiaP1 != null)
        {
            barraEnergiaP1.maxValue = gestorCombate.energiaMaxima;
        }

        //poner la energia maxima del jugador 2
        if (barraEnergiaP2 != null)
        {
            barraEnergiaP2.maxValue = gestorCombate.energiaMaxima;
        }
    }

    private void ActualizarInterfaz()
    {
        //no actualizar nada si falta el gestor
        if (gestorCombate == null)
        {
            return;
        }

        ActualizarVida();
        ActualizarEnergia();
        ActualizarTurno();
    }

    private void ConfigurarTextosHabilidades()
    {
        //poner las habilidades iniciales del jugador 1
        if (textoHabilidadesP1 != null)
        {
            textoHabilidadesP1.text = "1 - Paso basico\n2 - Paso fuerte\n3 - Defensa\n4 - Ulti";
        }

        //poner las habilidades iniciales del jugador 2
        if (textoHabilidadesP2 != null)
        {
            textoHabilidadesP2.text = "1 - Paso basico\n2 - Paso fuerte\n3 - Defensa\n4 - Ulti";
        }
    }

    private void ActualizarVida()
    {
        float vidaP1 = gestorCombate.luchadorP1.vidaActual;
        float vidaP2 = gestorCombate.luchadorP2.vidaActual;

        //actualizar la barra de vida del jugador 1
        if (barraVidaP1 != null)
        {
            barraVidaP1.value = vidaP1;
        }

        //actualizar la barra de vida del jugador 2
        if (barraVidaP2 != null)
        {
            barraVidaP2.value = vidaP2;
        }

        //actualizar el texto de vida del jugador 1
        if (textoVidaP1 != null)
        {
            textoVidaP1.text = vidaP1.ToString("0");
        }

        //actualizar el texto de vida del jugador 2
        if (textoVidaP2 != null)
        {
            textoVidaP2.text = vidaP2.ToString("0");
        }
    }

    private void ActualizarEnergia()
    {
        //actualizar la barra de energia del jugador 1
        if (barraEnergiaP1 != null)
        {
            barraEnergiaP1.value = gestorCombate.energiaP1;
        }

        //actualizar la barra de energia del jugador 2
        if (barraEnergiaP2 != null)
        {
            barraEnergiaP2.value = gestorCombate.energiaP2;
        }

        //actualizar el texto de energia del jugador 1
        if (textoEnergiaP1 != null)
        {
            textoEnergiaP1.text = gestorCombate.energiaP1.ToString("0");
        }

        //actualizar el texto de energia del jugador 2
        if (textoEnergiaP2 != null)
        {
            textoEnergiaP2.text = gestorCombate.energiaP2.ToString("0");
        }
    }

    private void ActualizarTurno()
    {
        //no hacer nada si no hay texto de turno
        if (textoTurno == null)
        {
            return;
        }

        //mostrar de quien es el turno
        if (gestorCombate.estadoActual == GestorCombate.EstadoJuego.TURNO_P1)
        {
            textoTurno.text = "Turno Jugador 1";
        }
        else if (gestorCombate.estadoActual == GestorCombate.EstadoJuego.TURNO_P2)
        {
            textoTurno.text = "Turno Jugador 2";
        }
        else
        {
            textoTurno.text = "Combate terminado";
        }
    }

    private void MostrarMensaje(string mensaje)
    {
        //mostrar un mensaje corto de la accion que se acaba de usar
        if (textoMensaje != null)
        {
            textoMensaje.text = mensaje;
        }
    }
}