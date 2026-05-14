using UnityEngine;

public class GestorCombate : MonoBehaviour
{
    public enum EstadoJuego { TURNO_P1, TURNO_P2, FIN_COMBATE}
    [Header("Luchadores")]
    public Luchador luchadorP1;
    public Luchador luchadorP2;
    [Header("Estado")]
    public EstadoJuego estadoActual;    

    [Header("Daño")]
    public float danoBasico = 10f;
    public float danoEspecial = 20f;
    public float danoUlti = 50f; // Variable para el daño de la ulti
    [Header("Energia")]
    public float energiaMaxima = 100f;
    public float energiaInicial = 0f;
    public float energiaPorTurno = 10f;
    public float energiaPorAtacar = 15f;
    public float energiaPorRecibirDaño = 15f;

    [Header("Defensa")]
    public float multiplicadorDefensa = 0.5f;

    public float energiaP1;
    public float energiaP2;

    private bool defensaP1;
    private bool defensaP2;

    void Start()
    {
        estadoActual = EstadoJuego.TURNO_P1;

        //Asignamos energia inicial al empezar la partida
        energiaP1 = energiaInicial;
        energiaP2 = energiaInicial;

        defensaP1 = false;
        defensaP2 = false;

        Debug.Log("Empieza el combate. Turno del jugador 1.");
    }

    // Update is called once per frame
    void Update()
    {
        //Si el combate termina no hacemos mas acciones.
       if (estadoActual == EstadoJuego.FIN_COMBATE)
       {
            return;
       }

       //Tecla 1: ataque basico
       if (Input.GetKeyDown(KeyCode.Alpha1))
       {
            PasoBasico();
       }
       // Tecla 2: ataque especial.
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PasoEspecial();
        }

        // Tecla 3: pose defensiva.
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Defender();
        }

        // Tecla 4: ulti / power-up.
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UsarUlti();
        }
    }

    public void PasoBasico()
    {
        //Ataque normal con daño bajo
        EjecutarAtaque(danoBasico);
    }

    public void PasoEspecial()
    {
        //Ataque mas fuerte
        //Hay que hacer que consuma energia IMPORTANTE
        EjecutarAtaque(danoEspecial);
    }

     public void Defender()
    {
        // El jugador actual queda defendiendo hasta recibir el siguiente ataque.
        if (estadoActual == EstadoJuego.TURNO_P1)
        {
            defensaP1 = true;
            Debug.Log("Jugador 1 usa pose defensiva.");
        }
        else
        {
            defensaP2 = true;
            Debug.Log("Jugador 2 usa pose defensiva.");
        }

        // Defender consume el turno.
        FinalizarTurno();
    }

    public void UsarUlti()
    {
        // La ulti solo puede usarse si la energía está al máximo.
        if (ObtenerEnergiaAtacante() < energiaMaxima)
        {
            Debug.Log("No tienes suficiente energía para usar la ulti.");
            return;
        }

        // Consumimos toda la energía.
        CambiarEnergiaAtacante(-energiaMaxima);

        Debug.Log("¡Subidón de Kumbia!");

        // La ulti es un ataque muy fuerte.
        EjecutarAtaque(danoUlti);
    }

    private void EjecutarAtaque(float dañoBase)
    {
        Luchador atacante = ObtenerAtacante();
        Luchador defensor = ObtenerDefensor();

        // Partimos del daño base del movimiento.
        float dañoFinal = dañoBase;

        // Si el defensor estaba defendiendo, recibe menos daño.
        if (DefensorEstaDefendiendo())
        {
            dañoFinal *= multiplicadorDefensa;
            QuitarDefensaDefensor();
        }

        // Aplicamos el daño al luchador defensor.
        defensor.RecibirDaño(dañoFinal);

        // Al atacar, el atacante gana energía.
        CambiarEnergiaAtacante(energiaPorAtacar);

        // Al recibir daño, el defensor también gana energía.
        CambiarEnergiaDefensor(energiaPorRecibirDaño);

        Debug.Log(atacante.nombrePersonaje + " hace " + dañoFinal + " de daño a " + defensor.nombrePersonaje + ".");

        // Si el defensor se queda sin vida, termina el combate.
        if (defensor.vidaActual <= 0)
        {
            TerminarCombate();
            return;
        }

        // Si nadie ha muerto, pasamos al siguiente turno.
        FinalizarTurno();
    }

    private void FinalizarTurno()
    {
        // El jugador que acaba de actuar gana energía extra por terminar su turno.
        CambiarEnergiaAtacante(energiaPorTurno);

        // Cambiamos el turno al otro jugador.
        if (estadoActual == EstadoJuego.TURNO_P1)
        {
            estadoActual = EstadoJuego.TURNO_P2;
            Debug.Log("Turno del jugador 2.");
        }
        else
        {
            estadoActual = EstadoJuego.TURNO_P1;
            Debug.Log("Turno del jugador 1.");
        }

        Debug.Log("Energía P1: " + energiaP1 + " / " + energiaMaxima);
        Debug.Log("Energía P2: " + energiaP2 + " / " + energiaMaxima);
    }

    private void TerminarCombate()
    {
        estadoActual = EstadoJuego.FIN_COMBATE;

        if (luchadorP1.vidaActual <= 0)
        {
            Debug.Log("Jugador 2 gana.");
        }
        else if (luchadorP2.vidaActual <= 0)
        {
            Debug.Log("Jugador 1 gana.");
        }
    }

    private Luchador ObtenerAtacante()
    {
        if (estadoActual == EstadoJuego.TURNO_P1)
        {
            return luchadorP1;
        }
        else
        {
            return luchadorP2;
        }
    }

    private Luchador ObtenerDefensor()
    {
        if (estadoActual == EstadoJuego.TURNO_P1)
        {
            return luchadorP2;
        }
        else
        {
            return luchadorP1;
        }
    }

    private float ObtenerEnergiaAtacante()
    {
        if (estadoActual == EstadoJuego.TURNO_P1)
        {
            return energiaP1;
        }
        else
        {
            return energiaP2;
        }
    }

    private void CambiarEnergiaAtacante(float cantidad)
    {
        if (estadoActual == EstadoJuego.TURNO_P1)
        {
            energiaP1 = Mathf.Clamp(energiaP1 + cantidad, 0f, energiaMaxima);
        }
        else
        {
            energiaP2 = Mathf.Clamp(energiaP2 + cantidad, 0f, energiaMaxima);
        }
    }

    private void CambiarEnergiaDefensor(float cantidad)
    {
        if (estadoActual == EstadoJuego.TURNO_P1)
        {
            energiaP2 = Mathf.Clamp(energiaP2 + cantidad, 0f, energiaMaxima);
        }
        else
        {
            energiaP1 = Mathf.Clamp(energiaP1 + cantidad, 0f, energiaMaxima);
        }
    }

    private bool DefensorEstaDefendiendo()
    {
        if (estadoActual == EstadoJuego.TURNO_P1)
        {
            return defensaP2;
        }
        else
        {
            return defensaP1;
        }
    }

    private void QuitarDefensaDefensor()
    {
        if (estadoActual == EstadoJuego.TURNO_P1)
        {
            defensaP2 = false;
        }
        else
        {
            defensaP1 = false;
        }
    }

}
