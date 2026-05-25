using System.Collections;
using UnityEngine;

public class AnimadorLuchador : MonoBehaviour
{
    [Header("Componentes")]
    public Animator animator;

    [Header("Estado por defecto")]
    public string estadoIdle = "idle";

    // Guardamos la posicion para que una animacion no mueva al personaje de sitio.
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
        // Esto se ejecuta despues del Animator, asi podemos corregir la posicion.
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

        // Esperamos lo que dura el clip real, no un numero inventado.
        float duracion = ObtenerDuracionClip(estadoAccion);
        if (duracion > 0f)
        {
            float tiempo = 0f;
            while (tiempo < duracion)
            {
                transform.position = posicionBloqueada;
                tiempo += Time.deltaTime;

                // Espera un frame y sigue. El juego no se queda bloqueado.
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

        // Reproduce el estado desde el principio.
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

        // Primero buscamos el clip con el nombre exacto.
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i] != null && clips[i].name == nombreEstado)
            {
                return clips[i].length;
            }
        }

        // Si no aparece, probamos con una busqueda mas flexible.
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
