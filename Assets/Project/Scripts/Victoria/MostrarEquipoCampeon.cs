using UnityEngine;
<<<<<<< HEAD
using UnityEngine.SceneManagement;
=======
>>>>>>> 033c59f40d396e0d669d3ab20d33b7081d5436fd
using UnityEngine.UI;

public class MostrarEquipoCampeon : MonoBehaviour
{
    [Header("Posiciones")]
    public Transform[] posicionesCampeones = new Transform[3];
    public bool invertirPersonajes;

    [Header("UI")]
<<<<<<< HEAD
    public Image imagenVictoria;
    public Sprite victoriaJugador1;
    public Sprite victoriaJugador2;
    public Button botonMenuInicial;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip audioVictoriaJugador1;
    public AudioClip audioVictoriaJugador2;

    [Header("Escenas")]
    public string escenaMenuInicial = "Menu Inicial";

    void Start()
    {
        MostrarImagenVictoria();
        MostrarCampeones();
        ReproducirAudioVictoria();
    }

    public void VolverAlMenuInicial()
    {
        if (!string.IsNullOrEmpty(escenaMenuInicial))
        {
            SceneManager.LoadScene(escenaMenuInicial);
        }
    }

    private void ReproducirAudioVictoria()
    {
        if (audioSource == null)
        {
            return;
        }

        if (DatosSeleccionCombate.jugadorGanador == 1 && audioVictoriaJugador1 != null)
        {
            audioSource.PlayOneShot(audioVictoriaJugador1);
        }
        else if (DatosSeleccionCombate.jugadorGanador == 2 && audioVictoriaJugador2 != null)
        {
            audioSource.PlayOneShot(audioVictoriaJugador2);
        }
    }

    private void MostrarImagenVictoria()
    {
        if (imagenVictoria == null)
        {
            imagenVictoria = CrearImagenVictoria();
        }

        if (imagenVictoria == null)
        {
            return;
        }

        Sprite imagenGanador = null;

        if (DatosSeleccionCombate.jugadorGanador == 1)
        {
            imagenGanador = victoriaJugador1;
        }
        else if (DatosSeleccionCombate.jugadorGanador == 2)
        {
            imagenGanador = victoriaJugador2;
        }

        if (imagenGanador == null)
        {
            Debug.LogWarning("Falta asignar la imagen de victoria del jugador " + DatosSeleccionCombate.jugadorGanador + ".");
            return;
        }

        imagenVictoria.sprite = imagenGanador;
        imagenVictoria.preserveAspect = true;
        imagenVictoria.gameObject.SetActive(true);
    }

    private Image CrearImagenVictoria()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("No hay Canvas en la escena Victoria.");
            return null;
        }

        GameObject objetoImagen = new GameObject("ImagenVictoria");
        objetoImagen.transform.SetParent(canvas.transform, false);

        Image imagen = objetoImagen.AddComponent<Image>();
        imagen.raycastTarget = false;
        imagen.preserveAspect = true;

        RectTransform rectTransform = objetoImagen.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new Vector2(0f, 260f);
        rectTransform.sizeDelta = new Vector2(760f, 300f);

        return imagen;
=======
    public Text textoGanador;

    void Start()
    {
        MostrarTextoGanador();
        MostrarCampeones();
    }

    void OnGUI()
    {
        if (textoGanador == null && DatosSeleccionCombate.jugadorGanador > 0)
        {
            GUI.Label(new Rect(20f, 20f, 300f, 40f), "Jugador " + DatosSeleccionCombate.jugadorGanador + " gana");
        }
    }

    private void MostrarTextoGanador()
    {
        if (textoGanador != null && DatosSeleccionCombate.jugadorGanador > 0)
        {
            textoGanador.text = "Jugador " + DatosSeleccionCombate.jugadorGanador + " gana";
        }
>>>>>>> 033c59f40d396e0d669d3ab20d33b7081d5436fd
    }

    private void MostrarCampeones()
    {
        DatosPersonaje[] equipoCampeon = DatosSeleccionCombate.equipoCampeon;

        if (equipoCampeon == null || equipoCampeon.Length == 0)
        {
            Debug.LogWarning("No hay equipo campeon guardado para mostrar.");
            return;
        }

        int cantidad = Mathf.Min(3, equipoCampeon.Length);
        for (int i = 0; i < cantidad; i++)
        {
            CrearCampeon(equipoCampeon[i], i);
        }
    }

    private void CrearCampeon(DatosPersonaje datosPersonaje, int indice)
    {
        if (datosPersonaje == null)
        {
            Debug.LogWarning("Falta el DatosPersonaje del campeon " + (indice + 1) + ".");
            return;
        }

        if (datosPersonaje.prefabPersonaje == null)
        {
            Debug.LogWarning(datosPersonaje.nombrePersonaje + " no tiene prefab asignado.");
            return;
        }

        Transform posicion = ObtenerPosicion(indice);
        Vector3 lugar = posicion != null ? posicion.position : ObtenerPosicionRespaldo(indice);
        Quaternion rotacion = posicion != null ? posicion.rotation : Quaternion.identity;

        GameObject instancia = Instantiate(datosPersonaje.prefabPersonaje, lugar, rotacion);

        if (posicion != null)
        {
            instancia.transform.localScale = posicion.localScale;
        }

        if (invertirPersonajes)
        {
            Vector3 escala = instancia.transform.localScale;
            escala.x = -Mathf.Abs(escala.x);
            instancia.transform.localScale = escala;
        }

        Luchador luchador = instancia.GetComponent<Luchador>();
        if (luchador != null)
        {
            luchador.Inicializar(datosPersonaje);
<<<<<<< HEAD
            StartCoroutine(luchador.ReproducirVictoria());
=======
            luchador.VolverAIdle();
>>>>>>> 033c59f40d396e0d669d3ab20d33b7081d5436fd
            return;
        }

        AnimadorLuchador animador = instancia.GetComponent<AnimadorLuchador>();
        if (animador != null)
        {
            animador.Configurar(datosPersonaje);
<<<<<<< HEAD
            StartCoroutine(animador.ReproducirVictoria(datosPersonaje.estadoWin));
=======
            animador.VolverAIdle();
>>>>>>> 033c59f40d396e0d669d3ab20d33b7081d5436fd
        }
    }

    private Transform ObtenerPosicion(int indice)
    {
        if (posicionesCampeones == null ||
            indice < 0 ||
            indice >= posicionesCampeones.Length)
        {
            return null;
        }

        if (posicionesCampeones[indice] == null)
        {
            Debug.LogWarning("Falta la posicion del campeon " + (indice + 1) + ".");
        }

        return posicionesCampeones[indice];
    }

    private Vector3 ObtenerPosicionRespaldo(int indice)
    {
        if (indice == 0)
        {
            return new Vector3(-3f, -1f, 0f);
        }

        if (indice == 1)
        {
            return new Vector3(0f, -1f, 0f);
        }

        return new Vector3(3f, -1f, 0f);
    }
}
