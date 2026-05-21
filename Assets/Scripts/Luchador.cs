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
        yield return ReproducirAccion(ObtenerEstadoBasico(), ObtenerDuracionBasico());
    }

    public IEnumerator ReproducirEspecial()
    {
        yield return ReproducirAccion(ObtenerEstadoEspecial(), ObtenerDuracionEspecial());
    }

    public IEnumerator ReproducirDefensa()
    {
        yield return ReproducirAccion(ObtenerEstadoDefensa(), ObtenerDuracionDefensa());
    }

    public IEnumerator ReproducirUlti()
    {
        yield return ReproducirAccion(ObtenerEstadoUlti(), ObtenerDuracionUlti());
    }

    private IEnumerator ReproducirAccion(string estado, float duracion)
    {
        if (animadorLuchador == null)
        {
            yield break;
        }

        yield return animadorLuchador.ReproducirAccion(estado, duracion);
    }

    private string ObtenerEstadoBasico()
    {
        return datosPersonaje != null ? datosPersonaje.estadoBasico : "basico";
    }

    private string ObtenerEstadoEspecial()
    {
        return datosPersonaje != null ? datosPersonaje.estadoEspecial : "especial";
    }

    private string ObtenerEstadoDefensa()
    {
        return datosPersonaje != null ? datosPersonaje.estadoDefensa : "bloqueo";
    }

    private string ObtenerEstadoUlti()
    {
        return datosPersonaje != null ? datosPersonaje.estadoUlti : "ulti";
    }

    private float ObtenerDuracionBasico()
    {
        return datosPersonaje != null ? datosPersonaje.duracionBasico : 0.6f;
    }

    private float ObtenerDuracionEspecial()
    {
        return datosPersonaje != null ? datosPersonaje.duracionEspecial : 0.8f;
    }

    private float ObtenerDuracionDefensa()
    {
        return datosPersonaje != null ? datosPersonaje.duracionDefensa : 0.5f;
    }

    private float ObtenerDuracionUlti()
    {
        return datosPersonaje != null ? datosPersonaje.duracionUlti : 1.2f;
    }

    private void Derrota()
    {
        Debug.Log(nombrePersonaje + " ha perdido.");
    }
}
