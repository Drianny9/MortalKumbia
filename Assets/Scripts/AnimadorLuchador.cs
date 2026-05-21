using System.Collections;
using UnityEngine;

public class AnimadorLuchador : MonoBehaviour
{
    [Header("Componentes")]
    public Animator animator;

    [Header("Estado por defecto")]
    public string estadoIdle = "idle";

    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
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

    public IEnumerator ReproducirAccion(string estadoAccion, float duracion)
    {
        ReproducirEstado(estadoAccion);

        if (duracion > 0f)
        {
            yield return new WaitForSeconds(duracion);
        }

        ReproducirIdle();
    }

    private void ReproducirEstado(string nombreEstado)
    {
        if (animator == null || string.IsNullOrEmpty(nombreEstado))
        {
            return;
        }

        animator.Play(nombreEstado, 0, 0f);
    }
}
