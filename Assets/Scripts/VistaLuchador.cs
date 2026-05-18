using UnityEngine;

public class VistaLuchador : MonoBehaviour
{
    [Header("Componentes")]
    public SpriteRenderer spriteRenderer;

    [Header("Colores")]
    public Color colorNormal = Color.white;
    public Color colorDaño = Color.red;
    public Color colorDefensa = Color.cyan;
    public Color colorUlti = Color.yellow;

    [Header("Tiempo")]
    public float duracionEfecto = 0.2f;

    private Vector3 escalaInicial;

    void Start()
    {
        //guardar la escala inicial para volver a dejar el personaje igual
        escalaInicial = transform.localScale;

        //buscar el SpriteRenderer en el mismo objeto si no esta puesto a mano
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        //dejar el personaje en su estado normal al empezar
        VolverANormal();
    }

    public void MostrarAtaque()
    {
        //hacer el personaje un poco mas grande para que parezca que ataca
        transform.localScale = escalaInicial * 1.15f;

        //volver a la normalidad despues de un momento
        Invoke(nameof(VolverANormal), duracionEfecto);
    }

    public void MostrarDaño()
    {
        //poner rojo para que se note que ha recibido daño
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorDaño;
        }

        Invoke(nameof(VolverANormal), duracionEfecto);
    }

    public void MostrarDefensa()
    {
        //poner otro color para indicar que esta defendiendo
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorDefensa;
        }

        Invoke(nameof(VolverANormal), duracionEfecto);
    }

    public void MostrarUlti()
    {
        //poner color especial para que se note que usa la ulti
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorUlti;
        }

        //hacer mas grande que con un ataque normal
        transform.localScale = escalaInicial * 1.3f;

        Invoke(nameof(VolverANormal), duracionEfecto);
    }

    private void VolverANormal()
    {
        //devolver el tamaño original
        transform.localScale = escalaInicial;

        //devolver el color original
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorNormal;
        }
    }
}