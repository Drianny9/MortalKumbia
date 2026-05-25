using System.Collections;
using UnityEngine;

public class Luchador : MonoBehaviour
{
    [Header("Datos")]
    public DatosPersonaje datosPersonaje;
    public string nombrePersonaje;

    [Header("Vida")]
    public float vidaMaxima = 100f;
    public float vidaActual;

    [Header("Danos")]
    public float danoBasico = 10f;
    public float danoEspecial = 20f;
    public float danoUlti = 50f;

    [Header("Componentes")]
    public AnimadorLuchador animadorLuchador;

    private bool inicializado;

    void Awake()
    {
        // Buscamos el script que se encarga de reproducir animaciones.
        // Si no esta puesto pero hay Animator, lo anadimos para poder probar rapido.
        if (animadorLuchador == null)
        {
            animadorLuchador = GetComponent<AnimadorLuchador>();
        }

        if (animadorLuchador == null && GetComponent<Animator>() != null)
        {
            animadorLuchador = gameObject.AddComponent<AnimadorLuchador>();
        }
    }

    void Start()
    {
        if (!inicializado)
        {
            Inicializar(datosPersonaje);
        }
    }

    public void Inicializar(DatosPersonaje nuevosDatos)
    {
        datosPersonaje = nuevosDatos;

        // Si hay DatosPersonaje, copiamos su vida, dano y nombre.
        // Si no hay datos, se usan los valores escritos en este componente.
        if (datosPersonaje != null)
        {
            nombrePersonaje = datosPersonaje.nombrePersonaje;
            vidaMaxima = datosPersonaje.vidaMaxima;
            danoBasico = datosPersonaje.danoBasico;
            danoEspecial = datosPersonaje.danoEspecial;
            danoUlti = datosPersonaje.danoUlti;
        }
        else if (string.IsNullOrEmpty(nombrePersonaje))
        {
            nombrePersonaje = gameObject.name;
        }

        vidaActual = vidaMaxima;
        inicializado = true;

        if (animadorLuchador != null)
        {
            if (datosPersonaje == null)
            {
                // Para personajes puestos a mano, buscamos una animacion con "idle" en el nombre.
                animadorLuchador.estadoIdle = BuscarEstadoAnimacion("idle", "idle");
            }

            animadorLuchador.Configurar(datosPersonaje);
        }
    }

    public void RecibirDano(float cantidad)
    {
        vidaActual = Mathf.Clamp(vidaActual - cantidad, 0f, vidaMaxima);
        Debug.Log(nombrePersonaje + " tiene " + vidaActual + " de vida.");

        if (vidaActual <= 0f)
        {
            Derrota();
        }
    }

    public string ObtenerTextoHabilidades()
    {
        if (datosPersonaje != null)
        {
            return datosPersonaje.ObtenerTextoHabilidades();
        }

        return "1 - Paso basico\n2 - Paso fuerte\n3 - Defensa\n4 - Ulti";
    }

    public IEnumerator ReproducirBasico()
    {
        yield return ReproducirAccion(ObtenerEstadoBasico());
    }

    public IEnumerator ReproducirEspecial()
    {
        yield return ReproducirAccion(ObtenerEstadoEspecial());
    }

    public IEnumerator ReproducirDefensa()
    {
        yield return ReproducirAccion(ObtenerEstadoDefensa());
    }

    public IEnumerator ReproducirUlti()
    {
        yield return ReproducirAccion(ObtenerEstadoUlti());
    }

    private IEnumerator ReproducirAccion(string estado)
    {
        if (animadorLuchador == null)
        {
            // Si el personaje no tiene animador, el combate continua sin animacion.
            yield break;
        }

        yield return animadorLuchador.ReproducirAccion(estado);
    }

    private string ObtenerEstadoBasico()
    {
        return datosPersonaje != null ? datosPersonaje.estadoBasico : BuscarEstadoAnimacion("basico", "basico");
    }

    private string ObtenerEstadoEspecial()
    {
        return datosPersonaje != null ? datosPersonaje.estadoEspecial : BuscarEstadoAnimacion("especial", "especial");
    }

    private string ObtenerEstadoDefensa()
    {
        return datosPersonaje != null ? datosPersonaje.estadoDefensa : BuscarEstadoAnimacion("bloqueo", "bloqueo");
    }

    private string ObtenerEstadoUlti()
    {
        return datosPersonaje != null ? datosPersonaje.estadoUlti : BuscarEstadoAnimacion("ulti", "ulti");
    }

    private string BuscarEstadoAnimacion(string textoBuscado, string respaldo)
    {
        // Ayuda para pruebas: si existe "Jotaro_basico", buscando "basico" lo encuentra.
        if (animadorLuchador == null ||
            animadorLuchador.animator == null ||
            animadorLuchador.animator.runtimeAnimatorController == null)
        {
            return respaldo;
        }

        AnimationClip[] clips = animadorLuchador.animator.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] != null && clips[i].name.ToLower().Contains(textoBuscado))
            {
                return clips[i].name;
            }
        }

        return respaldo;
    }

    private void Derrota()
    {
        Debug.Log(nombrePersonaje + " ha perdido.");
    }
}
