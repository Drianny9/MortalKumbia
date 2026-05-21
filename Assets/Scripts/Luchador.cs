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
