using System.Collections;
using UnityEngine;

public class GestorCombate : MonoBehaviour
{
    public enum EstadoJuego { TURNO_P1, TURNO_P2, FIN_COMBATE }

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

    [Header("Dano antiguo de respaldo")]
    public float danoBasico = 10f;
    public float danoEspecial = 20f;
    public float danoUlti = 50f;

    [Header("Energia")]
    public float energiaMaxima = 100f;
    public float energiaInicial = 0f;
    public float energiaPorTurno = 10f;
    public float energiaPorAtacar = 15f;
    public float energiaPorRecibirDano = 15f;

    [Header("Defensa")]
    public float multiplicadorDefensa = 0.5f;

    public float energiaP1;
    public float energiaP2;

    private DatosPersonaje[] equipoP1;
    private DatosPersonaje[] equipoP2;
    private int indiceP1;
    private int indiceP2;
    private bool defensaP1;
    private bool defensaP2;

    // Mientras esto esta activo no dejamos pulsar otro ataque.
    // Asi la animacion termina antes de cambiar de turno o recibir otro input.
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
            // StartCoroutine permite que el ataque espere a que acabe la animacion.
            // Si llamasemos a un metodo normal, el dano se aplicaria al instante.
            StartCoroutine(EjecutarAtaque(TipoAccion.Basico));
        }
    }

    public void PasoEspecial()
    {
        if (PuedeActuar())
        {
            StartCoroutine(EjecutarAtaque(TipoAccion.Especial));
        }
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

        StartCoroutine(EjecutarAtaque(TipoAccion.Ulti));
    }

    public Luchador ObtenerLuchadorP1()
    {
        return luchadorP1;
    }

    public Luchador ObtenerLuchadorP2()
    {
        return luchadorP2;
    }

    private IEnumerator EjecutarAtaque(TipoAccion tipoAccion)
    {
        accionEnCurso = true;

        Luchador atacante = ObtenerAtacante();
        Luchador defensor = ObtenerDefensor();

        if (atacante == null || defensor == null)
        {
            accionEnCurso = false;

            // yield break corta la corrutina aqui.
            // Lo usamos porque sin atacante o defensor no hay combate que ejecutar.
            yield break;
        }

        // Cada yield return espera a que el Luchador termine su animacion.
        // Despues de esto ya calculamos dano, energia y cambio de turno.
        if (tipoAccion == TipoAccion.Basico)
        {
            yield return atacante.ReproducirBasico();
        }
        else if (tipoAccion == TipoAccion.Especial)
        {
            yield return atacante.ReproducirEspecial();
        }
        else if (tipoAccion == TipoAccion.Ulti)
        {
            CambiarEnergiaAtacante(-energiaMaxima);
            yield return atacante.ReproducirUlti();
        }

        float danoFinal = ObtenerDano(atacante, tipoAccion);

        if (DefensorEstaDefendiendo())
        {
            // La defensa solo protege contra el siguiente golpe recibido.
            danoFinal *= multiplicadorDefensa;
            QuitarDefensaDefensor();
        }

        defensor.RecibirDano(danoFinal);
        CambiarEnergiaAtacante(energiaPorAtacar);
        CambiarEnergiaDefensor(energiaPorRecibirDano);

        Debug.Log(atacante.nombrePersonaje + " hace " + danoFinal + " de dano a " + defensor.nombrePersonaje + ".");

        if (defensor.vidaActual <= 0f)
        {
            bool defensorEsP1 = defensor == luchadorP1;
            bool hayRelevo = PasarAlSiguienteLuchador(defensorEsP1);

            if (!hayRelevo)
            {
                TerminarCombate(defensorEsP1 ? 2 : 1);
                accionEnCurso = false;

                // No cambiamos turno si la partida ya ha terminado.
                yield break;
            }
        }

        FinalizarTurno();
        accionEnCurso = false;
    }

    private IEnumerator EjecutarDefensa()
    {
        accionEnCurso = true;

        Luchador atacante = ObtenerAtacante();
        if (atacante == null)
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

        yield return atacante.ReproducirDefensa();

        FinalizarTurno();
        accionEnCurso = false;
    }

    private void PrepararEquipos()
    {
        // Si venimos del selector usamos esos equipos.
        // Si abrimos EscenaCombate directamente en Unity, usamos los equipos por defecto.
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

        // Si hay datos de equipo, instanciamos personajes desde prefab.
        // Si todavia estas probando con P1/P2 puestos a mano, los respetamos.
        if (EquipoTieneDatos(equipoP1))
        {
            luchadorP1 = InstanciarLuchador(equipoP1[indiceP1], spawnP1, false, luchadorP1);
        }
        else if (luchadorP1 != null)
        {
            luchadorP1.Inicializar(luchadorP1.datosPersonaje);
        }

        if (EquipoTieneDatos(equipoP2))
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

            // El personaje anterior ya murio: lo quitamos y ponemos el siguiente del equipo.
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
            // P2 mira hacia la izquierda. Por eso invertimos la escala en X.
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
        // Esta es la puerta de entrada para cualquier boton/tecla de accion.
        return estadoActual != EstadoJuego.FIN_COMBATE &&
               !accionEnCurso &&
               luchadorP1 != null &&
               luchadorP2 != null;
    }

    private float ObtenerDano(Luchador atacante, TipoAccion tipoAccion)
    {
        if (atacante == null)
        {
            return 0f;
        }

        if (tipoAccion == TipoAccion.Basico)
        {
            return atacante.danoBasico > 0f ? atacante.danoBasico : danoBasico;
        }

        if (tipoAccion == TipoAccion.Especial)
        {
            return atacante.danoEspecial > 0f ? atacante.danoEspecial : danoEspecial;
        }

        return atacante.danoUlti > 0f ? atacante.danoUlti : danoUlti;
    }

    private void FinalizarTurno()
    {
        CambiarEnergiaAtacante(energiaPorTurno);

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
        Debug.Log("Jugador " + jugadorGanador + " gana.");
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

    private bool EquipoTieneDatos(DatosPersonaje[] equipo)
    {
        return EquipoTienePersonajeEnIndice(equipo, 0);
    }

    private bool EquipoTienePersonajeEnIndice(DatosPersonaje[] equipo, int indice)
    {
        return equipo != null &&
               indice >= 0 &&
               indice < equipo.Length &&
               equipo[indice] != null;
    }

    private enum TipoAccion
    {
        Basico,
        Especial,
        Ulti
    }
}
