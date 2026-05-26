using System.Collections;
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
    public Text textoTurno;
    public Text textoMensaje;
    public Text textoHabilidadesP1;
    public Text textoHabilidadesP2;

    [Header("Ajuste Textos Habilidades")]
    public TextAnchor alineacionHabilidades = TextAnchor.UpperLeft;
    public float espaciadoLineasHabilidades = 1f;

    [Header("Teclas Jugador 1")]
    public Image tecla1P1;
    public Image tecla2P1;
    public Image tecla3P1;
    public Image tecla4P1;

    [Header("Teclas Jugador 2")]
    public Image tecla1P2;
    public Image tecla2P2;
    public Image tecla3P2;
    public Image tecla4P2;

    [Header("Sprites Teclas")]
    public Sprite tecla1Normal;
    public Sprite tecla1Pulsada;
    public Sprite tecla2Normal;
    public Sprite tecla2Pulsada;
    public Sprite tecla3Normal;
    public Sprite tecla3Pulsada;
    public Sprite tecla4Normal;
    public Sprite tecla4Pulsada;

    [Header("Animacion Teclas")]
    public float tiempoTeclaPulsada = 0.12f;

    void Start()
    {
        ConfigurarBarras();
        ConfigurarEstiloTextosHabilidades();
        ConfigurarTeclasNormales();
        ActualizarInterfaz();
        ConfigurarTextosHabilidades();
    }

    void Update()
    {
        ActualizarInterfaz();
        ConfigurarEstiloTextosHabilidades();
        ConfigurarTextosHabilidades();
        DetectarTeclasPulsadas();
    }

    public void BotonPasoBasico()
    {
        AnimarTecla(1);
        gestorCombate.PasoBasico();
        MostrarMensaje("Paso basico");
    }

    public void BotonPasoEspecial()
    {
        AnimarTecla(2);
        gestorCombate.PasoEspecial();
        MostrarMensaje("Paso especial");
    }

    public void BotonDefender()
    {
        AnimarTecla(3);
        gestorCombate.Defender();
        MostrarMensaje("Defensa");
    }

    public void BotonUlti()
    {
        AnimarTecla(4);
        gestorCombate.UsarUlti();
        MostrarMensaje("Subidon de Kumbia");
    }

    private void DetectarTeclasPulsadas()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            AnimarTecla(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            AnimarTecla(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            AnimarTecla(3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            AnimarTecla(4);
        }
    }

    private void AnimarTecla(int numeroTecla)
    {
        if (numeroTecla == 1)
        {
            StartCoroutine(AnimarImagenTecla(ObtenerImagenTeclaActiva(tecla1P1, tecla1P2), tecla1Normal, tecla1Pulsada));
        }
        else if (numeroTecla == 2)
        {
            StartCoroutine(AnimarImagenTecla(ObtenerImagenTeclaActiva(tecla2P1, tecla2P2), tecla2Normal, tecla2Pulsada));
        }
        else if (numeroTecla == 3)
        {
            StartCoroutine(AnimarImagenTecla(ObtenerImagenTeclaActiva(tecla3P1, tecla3P2), tecla3Normal, tecla3Pulsada));
        }
        else if (numeroTecla == 4)
        {
            StartCoroutine(AnimarImagenTecla(ObtenerImagenTeclaActiva(tecla4P1, tecla4P2), tecla4Normal, tecla4Pulsada));
        }
    }

    private Image ObtenerImagenTeclaActiva(Image imagenP1, Image imagenP2)
    {
        if (gestorCombate != null && gestorCombate.estadoActual == GestorCombate.EstadoJuego.TURNO_P2)
        {
            return imagenP2;
        }

        return imagenP1;
    }

    private IEnumerator AnimarImagenTecla(Image imagen, Sprite spriteNormal, Sprite spritePulsada)
    {
        PonerSprite(imagen, spritePulsada);

        yield return new WaitForSeconds(tiempoTeclaPulsada);

        PonerSprite(imagen, spriteNormal);
    }

    private void ConfigurarTeclasNormales()
    {
        PonerSprite(tecla1P1, tecla1Normal);
        PonerSprite(tecla1P2, tecla1Normal);
        PonerSprite(tecla2P1, tecla2Normal);
        PonerSprite(tecla2P2, tecla2Normal);
        PonerSprite(tecla3P1, tecla3Normal);
        PonerSprite(tecla3P2, tecla3Normal);
        PonerSprite(tecla4P1, tecla4Normal);
        PonerSprite(tecla4P2, tecla4Normal);
    }

    private void PonerSprite(Image imagen, Sprite sprite)
    {
        if (imagen != null && sprite != null)
        {
            imagen.sprite = sprite;
            imagen.preserveAspect = true;
        }
    }

    private void ConfigurarBarras()
    {
        if (gestorCombate == null)
        {
            return;
        }

        if (barraVidaP1 != null && gestorCombate.luchadorP1 != null)
        {
            barraVidaP1.maxValue = gestorCombate.luchadorP1.vidaMaxima;
        }

        if (barraVidaP2 != null && gestorCombate.luchadorP2 != null)
        {
            barraVidaP2.maxValue = gestorCombate.luchadorP2.vidaMaxima;
        }

        if (barraEnergiaP1 != null)
        {
            barraEnergiaP1.maxValue = gestorCombate.energiaMaxima;
        }

        if (barraEnergiaP2 != null)
        {
            barraEnergiaP2.maxValue = gestorCombate.energiaMaxima;
        }
    }

    private void ActualizarInterfaz()
    {
        if (gestorCombate == null)
        {
            return;
        }

        ConfigurarBarras();
        ActualizarVida();
        ActualizarEnergia();
        ActualizarTurno();
    }

    private void ConfigurarEstiloTextosHabilidades()
    {
        ConfigurarEstiloTextoHabilidades(textoHabilidadesP1);
        ConfigurarEstiloTextoHabilidades(textoHabilidadesP2);
    }

    private void ConfigurarEstiloTextoHabilidades(Text texto)
    {
        if (texto == null)
        {
            return;
        }

        texto.alignment = alineacionHabilidades;
        texto.lineSpacing = espaciadoLineasHabilidades;
        texto.horizontalOverflow = HorizontalWrapMode.Overflow;
        texto.verticalOverflow = VerticalWrapMode.Overflow;
    }
    private void ConfigurarTextosHabilidades()
    {
        if (textoHabilidadesP1 != null && gestorCombate != null && gestorCombate.luchadorP1 != null)
        {
            textoHabilidadesP1.text = gestorCombate.luchadorP1.ObtenerTextoHabilidades();
        }

        if (textoHabilidadesP2 != null && gestorCombate != null && gestorCombate.luchadorP2 != null)
        {
            textoHabilidadesP2.text = gestorCombate.luchadorP2.ObtenerTextoHabilidades();
        }
    }

    private void ActualizarVida()
    {
        if (gestorCombate.luchadorP1 == null || gestorCombate.luchadorP2 == null)
        {
            return;
        }

        float vidaP1 = gestorCombate.luchadorP1.vidaActual;
        float vidaP2 = gestorCombate.luchadorP2.vidaActual;

        if (barraVidaP1 != null)
        {
            barraVidaP1.value = vidaP1;
        }

        if (barraVidaP2 != null)
        {
            barraVidaP2.value = vidaP2;
        }
    }

    private void ActualizarEnergia()
    {
        if (barraEnergiaP1 != null)
        {
            barraEnergiaP1.value = gestorCombate.energiaP1;
        }

        if (barraEnergiaP2 != null)
        {
            barraEnergiaP2.value = gestorCombate.energiaP2;
        }
    }

    private void ActualizarTurno()
    {
        if (textoTurno == null)
        {
            return;
        }

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
        if (textoMensaje != null)
        {
            textoMensaje.text = mensaje;
        }
    }
}