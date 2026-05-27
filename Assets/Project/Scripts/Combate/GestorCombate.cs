using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
<<<<<<< HEAD
using UnityEngine.UI;
=======
>>>>>>> 033c59f40d396e0d669d3ab20d33b7081d5436fd

public class GestorCombate : MonoBehaviour
{
    public enum EstadoJuego { TURNO_P1, TURNO_P2, FIN_COMBATE }
    private enum TipoAtaque { Basico, Especial, Ulti }

    [Header("Luchadores activos")]
    public Luchador luchadorP1;
    public Luchador luchadorP2;

    [Header("Spawns")]
    public Transform spawnP1;
    public Transform spawnP2;
    public bool invertirP2 = true;

    [Header("Equipos por defecto")]
    public DatosPersonaje[] equipoPorDefectoP1 = new DatosPersonaje[3];
    public DatosPersonaje[] equipoPorDefectoP2 = new DatosPersonaje[3];

    [Header("Estado")]
    public EstadoJuego estadoActual;
    public string escenaVictoria = "Victoria";

    [Header("Dano por defecto")]
    public float danoBasico = 10f;
    public float danoEspecial = 20f;
    public float danoUlti = 50f;

    [Header("Energia")]
    public float energiaMaxima = 100f;
    public float energiaInicial = 0f;
    public float energiaPorTurno = 10f;
    public float energiaPorAtacar = 15f;
    public float energiaPorRecibirDano = 15f;
    public float costeEnergiaEspecial = 25f;

    [Header("Defensa")]
    public float multiplicadorDefensaBasico = 0.5f;
    public float multiplicadorDefensaEspecial = 0.4f;
    public float multiplicadorDefensaUlti = 0.25f;
<<<<<<< HEAD

    [Header("Audio voces")]
    public AudioSource audioSourceCombate;
    public AudioClip[] gritosAtaqueP1;
    public AudioClip[] gritosGolpeP1;
    public AudioClip[] gritosAtaqueP2;
    public AudioClip[] gritosGolpeP2;

    [Header("Audio impactos")]
    public AudioClip[] sonidosImpacto;
    public AudioClip[] sonidosImpactoCritico;

    [Header("Muerte")]
    public float tiempoMostrarMuerto = 1.2f;

    [Header("Final combate")]
    public GameObject animacionFinish;
    public float esperaAntesVictoria = 2f;
=======
>>>>>>> 033c59f40d396e0d669d3ab20d33b7081d5436fd

    public float energiaP1;
    public float energiaP2;

    private DatosPersonaje[] equipoP1;
    private DatosPersonaje[] equipoP2;
    private int indiceP1;
    private int indiceP2;
    private bool defensaP1;
    private bool defensaP2;

    // Evita que se pueda pulsar otra accion mientras una animacion esta en marcha.
    private bool accionEnCurso;

    void Start()
    {
        estadoActual = EstadoJuego.TURNO_P1;
        energiaP1 = energiaInicial;
        energiaP2 = energiaInicial;
        defensaP1 = false;
        defensaP2 = false;
        accionEnCurso = false;

        PrepararEquipos();
        PrepararLuchadoresIniciales();

        Debug.Log("Empieza el combate. Turno del jugador 1.");
    }

    void Update()
    {
        if (estadoActual == EstadoJuego.FIN_COMBATE || accionEnCurso)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PasoBasico();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PasoEspecial();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Defender();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UsarUlti();
        }
    }

    public void PasoBasico()
    {
        if (PuedeActuar())
        {
            StartCoroutine(EjecutarBasico());
        }
    }

    public void PasoEspecial()
    {
        if (!PuedeActuar())
        {
            return;
        }

        if (ObtenerEnergiaAtacante() < costeEnergiaEspecial)
        {
            Debug.Log("No tienes suficiente energia para usar el ataque especial.");
            return;
        }

        StartCoroutine(EjecutarEspecial());
    }

    public void Defender()
    {
        if (PuedeActuar())
        {
            StartCoroutine(EjecutarDefensa());
        }
    }

    public void UsarUlti()
    {
        if (!PuedeActuar())
        {
            return;
        }

        if (ObtenerEnergiaAtacante() < energiaMaxima)
        {
            Debug.Log("No tienes suficiente energia para usar la ulti.");
            return;
        }

        StartCoroutine(EjecutarUlti());
    }

    public Luchador ObtenerLuchadorP1()
    {
        return luchadorP1;
    }

    public Luchador ObtenerLuchadorP2()
    {
        return luchadorP2;
    }

    private IEnumerator EjecutarBasico()
    {
        accionEnCurso = true;

        Luchador atacante = ObtenerAtacante();
        if (atacante == null)
        {
            accionEnCurso = false;
            yield break;
        }

        QuitarDefensaAtacante();
<<<<<<< HEAD
        ReproducirGritoAtaque(atacante);
=======
>>>>>>> 033c59f40d396e0d669d3ab20d33b7081d5436fd
        yield return atacante.ReproducirBasico();

        AplicarAtaque(atacante, ObtenerDefensor(), ObtenerDanoBasico(atacante), TipoAtaque.Basico, true);
    }

    private IEnumerator EjecutarEspecial()
    {
        accionEnCurso = true;

        Luchador atacante = ObtenerAtacante();
        if (atacante == null)
        {
            accionEnCurso = false;
            yield break;
        }

        QuitarDefensaAtacante();
        CambiarEnergiaAtacante(-costeEnergiaEspecial);
        ReproducirGritoAtaque(atacante);
        yield return atacante.ReproducirEspecial();

        AplicarAtaque(atacante, ObtenerDefensor(), ObtenerDanoEspecial(atacante), TipoAtaque.Especial, false);
    }

    private IEnumerator EjecutarUlti()
    {
        accionEnCurso = true;

        Luchador atacante = ObtenerAtacante();
        if (atacante == null)
        {
            accionEnCurso = false;
            yield break;
        }

        QuitarDefensaAtacante();
        CambiarEnergiaAtacante(-energiaMaxima);
        ReproducirGritoAtaque(atacante);
        yield return atacante.ReproducirUlti();

        AplicarAtaque(atacante, ObtenerDefensor(), ObtenerDanoUlti(atacante), TipoAtaque.Ulti, true);
    }

    private IEnumerator EjecutarDefensa()
    {
        accionEnCurso = true;

        Luchador luchadorActual = ObtenerAtacante();
        if (luchadorActual == null)
        {
            accionEnCurso = false;
            yield break;
        }

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

        luchadorActual.ReproducirDefensa();

        FinalizarTurnoSinEnergia();
        accionEnCurso = false;
    }

    private void AplicarAtaque(Luchador atacante, Luchador defensor, float dano, TipoAtaque tipoAtaque, bool darEnergiaAtacante)
    {
        if (atacante == null || defensor == null)
        {
            accionEnCurso = false;
            return;
        }

        float danoFinal = dano;
        bool defensorEstabaDefendiendo = DefensorEstaDefendiendo();

        if (defensorEstabaDefendiendo)
        {
            danoFinal *= ObtenerMultiplicadorDefensa(tipoAtaque);
        }

        defensor.RecibirDano(danoFinal);
<<<<<<< HEAD
        defensor.MostrarSpriteDano(0.25f);
        ReproducirAudioAleatorio(sonidosImpacto);
        ReproducirGritoGolpe(defensor);
=======
>>>>>>> 033c59f40d396e0d669d3ab20d33b7081d5436fd

        if (defensorEstabaDefendiendo)
        {
            QuitarDefensaDefensor();

            if (defensor.vidaActual > 0f)
            {
                defensor.VolverAIdle();
            }
        }
        if (darEnergiaAtacante)
        {
            CambiarEnergiaAtacante(energiaPorAtacar);
        }

        CambiarEnergiaDefensor(energiaPorRecibirDano);

        Debug.Log(atacante.nombrePersonaje + " hace " + danoFinal + " de dano a " + defensor.nombrePersonaje + ".");

        if (defensor.vidaActual <= 0f)
        {
            StartCoroutine(ProcesarMuerte(defensor));
            return;
        }

        FinalizarTurno();
        accionEnCurso = false;
    }

    private IEnumerator ProcesarMuerte(Luchador defensor)
    {
        defensor.MostrarSpriteMuerto();

        yield return new WaitForSeconds(tiempoMostrarMuerto);

        bool defensorEsP1 = defensor == luchadorP1;
        bool hayRelevo = PasarAlSiguienteLuchador(defensorEsP1);

        if (!hayRelevo)
        {
            ReproducirAudioAleatorio(sonidosImpactoCritico);
            TerminarCombate(defensorEsP1 ? 2 : 1);
            accionEnCurso = false;
            yield break;
        }

        FinalizarTurno();
        accionEnCurso = false;
    }

    private void PrepararEquipos()
    {
        // Si venimos del selector usamos sus equipos. Si no, usamos lo puesto en el Inspector.
        if (DatosSeleccionCombate.HaySeleccionCompleta)
        {
            equipoP1 = DatosSeleccionCombate.equipoP1;
            equipoP2 = DatosSeleccionCombate.equipoP2;
        }
        else
        {
            equipoP1 = equipoPorDefectoP1;
            equipoP2 = equipoPorDefectoP2;
        }
    }

    private void PrepararLuchadoresIniciales()
    {
        indiceP1 = 0;
        indiceP2 = 0;

        if (EquipoTienePersonajeEnIndice(equipoP1, indiceP1))
        {
            luchadorP1 = InstanciarLuchador(equipoP1[indiceP1], spawnP1, false, luchadorP1);
        }
        else if (luchadorP1 != null)
        {
            luchadorP1.Inicializar(luchadorP1.datosPersonaje);
        }

        if (EquipoTienePersonajeEnIndice(equipoP2, indiceP2))
        {
            luchadorP2 = InstanciarLuchador(equipoP2[indiceP2], spawnP2, invertirP2, luchadorP2);
        }
        else if (luchadorP2 != null)
        {
            luchadorP2.Inicializar(luchadorP2.datosPersonaje);
        }
    }

    private bool PasarAlSiguienteLuchador(bool esP1)
    {
        if (esP1)
        {
            indiceP1++;

            if (!EquipoTienePersonajeEnIndice(equipoP1, indiceP1))
            {
                return false;
            }

            DestruirLuchadorSeguro(luchadorP1);
            energiaP1 = energiaInicial;
            defensaP1 = false;
            luchadorP1 = InstanciarLuchador(equipoP1[indiceP1], spawnP1, false, null);
            return luchadorP1 != null;
        }

        indiceP2++;

        if (!EquipoTienePersonajeEnIndice(equipoP2, indiceP2))
        {
            return false;
        }

        DestruirLuchadorSeguro(luchadorP2);
        energiaP2 = energiaInicial;
        defensaP2 = false;
        luchadorP2 = InstanciarLuchador(equipoP2[indiceP2], spawnP2, invertirP2, null);
        return luchadorP2 != null;
    }

    private Luchador InstanciarLuchador(DatosPersonaje datos, Transform spawn, bool invertir, Luchador luchadorAnterior)
    {
        if (datos == null || datos.prefabPersonaje == null)
        {
            return luchadorAnterior;
        }

        DestruirLuchadorSeguro(luchadorAnterior);

        Vector3 posicion = spawn != null ? spawn.position : Vector3.zero;
        Quaternion rotacion = spawn != null ? spawn.rotation : Quaternion.identity;
        GameObject instancia = Instantiate(datos.prefabPersonaje, posicion, rotacion);

        if (spawn != null)
        {
            instancia.transform.localScale = spawn.localScale;
        }

        if (invertir)
        {
            Vector3 escala = instancia.transform.localScale;
            escala.x = -Mathf.Abs(escala.x);
            instancia.transform.localScale = escala;
        }

        Luchador luchador = instancia.GetComponent<Luchador>();
        if (luchador == null)
        {
            luchador = instancia.AddComponent<Luchador>();
        }

        if (luchador.animadorLuchador == null)
        {
            luchador.animadorLuchador = instancia.GetComponent<AnimadorLuchador>();
        }

        if (luchador.animadorLuchador == null)
        {
            luchador.animadorLuchador = instancia.AddComponent<AnimadorLuchador>();
        }

        luchador.Inicializar(datos);
        return luchador;
    }

    private void DestruirLuchadorSeguro(Luchador luchador)
    {
        if (luchador != null)
        {
            Destroy(luchador.gameObject);
        }
    }

    private bool PuedeActuar()
    {
        return estadoActual != EstadoJuego.FIN_COMBATE &&
               !accionEnCurso &&
               luchadorP1 != null &&
               luchadorP2 != null;
    }

    private float ObtenerDanoBasico(Luchador atacante)
    {
        return atacante.danoBasico > 0f ? atacante.danoBasico : danoBasico;
    }

    private float ObtenerDanoEspecial(Luchador atacante)
    {
        return atacante.danoEspecial > 0f ? atacante.danoEspecial : danoEspecial;
    }

    private float ObtenerDanoUlti(Luchador atacante)
    {
        return atacante.danoUlti > 0f ? atacante.danoUlti : danoUlti;
    }

    private void FinalizarTurno()
    {
        CambiarEnergiaAtacante(energiaPorTurno);
        CambiarTurno();
    }

    private void FinalizarTurnoSinEnergia()
    {
        CambiarTurno();
    }

    private void CambiarTurno()
    {
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

        Debug.Log("Energia P1: " + energiaP1 + " / " + energiaMaxima);
        Debug.Log("Energia P2: " + energiaP2 + " / " + energiaMaxima);
    }

    private void TerminarCombate(int jugadorGanador)
    {
        estadoActual = EstadoJuego.FIN_COMBATE;
        DatosPersonaje[] equipoGanador = jugadorGanador == 1 ? equipoP1 : equipoP2;
        DatosSeleccionCombate.GuardarEquipoCampeon(equipoGanador, jugadorGanador);
        Debug.Log("Jugador " + jugadorGanador + " gana.");

<<<<<<< HEAD
        StartCoroutine(TransicionVictoria());
    }

    private IEnumerator TransicionVictoria()
    {
        if (animacionFinish != null)
        {
            animacionFinish.SetActive(true);
        }

        yield return new WaitForSeconds(esperaAntesVictoria);

=======
>>>>>>> 033c59f40d396e0d669d3ab20d33b7081d5436fd
        if (!string.IsNullOrEmpty(escenaVictoria))
        {
            SceneManager.LoadScene(escenaVictoria);
        }
    }

    private Luchador ObtenerAtacante()
    {
        return estadoActual == EstadoJuego.TURNO_P1 ? luchadorP1 : luchadorP2;
    }

    private Luchador ObtenerDefensor()
    {
        return estadoActual == EstadoJuego.TURNO_P1 ? luchadorP2 : luchadorP1;
    }

    private float ObtenerEnergiaAtacante()
    {
        return estadoActual == EstadoJuego.TURNO_P1 ? energiaP1 : energiaP2;
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
        return estadoActual == EstadoJuego.TURNO_P1 ? defensaP2 : defensaP1;
    }

    private float ObtenerMultiplicadorDefensa(TipoAtaque tipoAtaque)
    {
        if (tipoAtaque == TipoAtaque.Especial)
        {
            return multiplicadorDefensaEspecial;
        }

        if (tipoAtaque == TipoAtaque.Ulti)
        {
            return multiplicadorDefensaUlti;
        }

        return multiplicadorDefensaBasico;
    }

    private void QuitarDefensaAtacante()
    {
        if (estadoActual == EstadoJuego.TURNO_P1)
        {
            defensaP1 = false;
        }
        else
        {
            defensaP2 = false;
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

    private bool EquipoTienePersonajeEnIndice(DatosPersonaje[] equipo, int indice)
    {
        return equipo != null &&
               indice >= 0 &&
               indice < equipo.Length &&
               equipo[indice] != null;
    }

    private void ReproducirGritoAtaque(Luchador atacante)
    {
        if (atacante == luchadorP1)
        {
            ReproducirAudioAleatorio(gritosAtaqueP1);
        }
        else
        {
            ReproducirAudioAleatorio(gritosAtaqueP2);
        }
    }

    private void ReproducirGritoGolpe(Luchador defensor)
    {
        if (defensor == luchadorP1)
        {
            ReproducirAudioAleatorio(gritosGolpeP1);
        }
        else
        {
            ReproducirAudioAleatorio(gritosGolpeP2);
        }
    }

    private void ReproducirAudioAleatorio(AudioClip[] clips)
    {
        if (audioSourceCombate == null || clips == null || clips.Length == 0)
        {
            return;
        }

        int indice = Random.Range(0, clips.Length);
        AudioClip clip = clips[indice];

        if (clip != null)
        {
            audioSourceCombate.PlayOneShot(clip);
        }
    }
}
