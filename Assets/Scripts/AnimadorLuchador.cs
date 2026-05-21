using System.Collections;
using UnityEngine;

public class AnimadorLuchador : MonoBehaviour
{
    [Header("Componentes")]
    public Animator animator;

    [Header("Estado por defecto")]
    public string estadoIdle = "idle";

    // Algunas animaciones tienen keyframes de posicion.
    // Bloqueamos la posicion para que el personaje no se vaya al centro al atacar.
    private bool bloquearPosicion;
    private Vector3 posicionBloqueada;

    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void LateUpdate()
    {
        // LateUpdate corre despues de que el Animator haya aplicado sus cambios.
        // Por eso sirve para devolver el personaje a su sitio si una animacion lo movio.
        if (bloquearPosicion)
        {
            transform.position = posicionBloqueada;
        }
    }

    public void Configurar(DatosPersonaje datosPersonaje)
    {
        if (datosPersonaje != null && !string.IsNullOrEmpty(datosPersonaje.estadoIdle))
        {
            estadoIdle = datosPersonaje.estadoIdle;
        }

        ReproducirIdle();
    }

    public void ReproducirIdle()
    {
        ReproducirEstado(estadoIdle);
    }

    public IEnumerator ReproducirAccion(string estadoAccion)
    {
        posicionBloqueada = transform.position;
        bloquearPosicion = true;

        ReproducirEstado(estadoAccion);

        // No usamos una duracion escrita a mano:
        // miramos cuanto dura el clip real y esperamos eso.
        float duracion = ObtenerDuracionClip(estadoAccion);
        if (duracion > 0f)
        {
            float tiempo = 0f;
            while (tiempo < duracion)
            {
                transform.position = posicionBloqueada;
                tiempo += Time.deltaTime;

                // yield return null significa "sigue en el siguiente frame".
                // Asi la animacion puede avanzar sin congelar el juego.
                yield return null;
            }
        }

        bloquearPosicion = false;
        transform.position = posicionBloqueada;
        ReproducirIdle();
    }

    private void ReproducirEstado(string nombreEstado)
    {
        if (animator == null || string.IsNullOrEmpty(nombreEstado))
        {
            return;
        }

        // Reproduce el estado desde el inicio.
        // El nombre debe coincidir con un estado del Animator.
        animator.Play(nombreEstado, 0, 0f);
    }

    private float ObtenerDuracionClip(string nombreEstado)
    {
        if (animator == null ||
            animator.runtimeAnimatorController == null ||
            string.IsNullOrEmpty(nombreEstado))
        {
            return 0f;
        }

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        // Primero intentamos coincidencia exacta.
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] != null && clips[i].name == nombreEstado)
            {
                return clips[i].length;
            }
        }

        // Y si no, una busqueda mas flexible para pruebas.
        // Por ejemplo, "basico" puede encontrar "Jotaro_basico".
        string nombreNormalizado = nombreEstado.ToLower();
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] != null && clips[i].name.ToLower().Contains(nombreNormalizado))
            {
                return clips[i].length;
            }
        }

        return 0f;
    }
}
