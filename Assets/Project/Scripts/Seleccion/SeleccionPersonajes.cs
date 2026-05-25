using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeleccionPersonajes : MonoBehaviour
{
    [Header("Configuracion")]
    public int personajesPorEquipo = 3;
    public string escenaCombate = "EscenaCombate";

    [Header("UI")]
    public Text textoEstado;
    public RectTransform marcoSeleccion;
    public Button botonComenzar;
    public Image panelTransicion;
    public float duracionTransicion = 0.8f;
    public Image[] slotsP1;
    public Image[] slotsP2;
    public BotonPersonaje[] botonesPersonaje;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sonidoDesplazar;
    public AudioClip sonidoSeleccion;
    private BotonPersonaje botonConFoco;
    private bool puedeSonarDesplazamiento;

    private readonly List<DatosPersonaje> equipoP1 = new List<DatosPersonaje>();
    private readonly List<DatosPersonaje> equipoP2 = new List<DatosPersonaje>();
    private int jugadorActual = 1;
    private int ultimoFrameSeleccion = -1;
    private bool seleccionCompleta;
    private bool cargandoCombate;

    void Start()
    {
        LimpiarSlots();
        PrepararBotonComenzar();
        PrepararPanelTransicion();
        ActualizarTextoEstado();
        ActualizarBotones();
        SeleccionarPrimerBotonDisponible();
        puedeSonarDesplazamiento = true;
    }

    void Update()
    {
        AnimarMarcoSeleccion();
    }

    public void SeleccionarPersonaje(DatosPersonaje datosPersonaje)
    {
        // Evita que una misma pulsacion de boton cuente dos veces.
        if (ultimoFrameSeleccion == Time.frameCount)
        {
            return;
        }

        ultimoFrameSeleccion = Time.frameCount;

        if (!PuedeSeleccionar(datosPersonaje))
        {
            ActualizarTextoEstado();
            ActualizarBotones();
            return;
        }

        ReproducirSonido(sonidoSeleccion);

        if (datosPersonaje.audioSeleccion != null)
        {
            ReproducirSonido(datosPersonaje.audioSeleccion);
        }

        if (jugadorActual == 1)
        {
            equipoP1.Add(datosPersonaje);

            if (equipoP1.Count >= personajesPorEquipo)
            {
                jugadorActual = 2;
            }
        }
        else
        {
            equipoP2.Add(datosPersonaje);
        }

        ActualizarSlots();
        ActualizarTextoEstado();
        ActualizarBotones();

        if (equipoP1.Count >= personajesPorEquipo && equipoP2.Count >= personajesPorEquipo)
        {
            CompletarSeleccion();
            return;
        }

        SeleccionarPrimerBotonDisponible();
    }

    public void ReiniciarSeleccion()
    {
        equipoP1.Clear();
        equipoP2.Clear();
        jugadorActual = 1;
        seleccionCompleta = false;
        cargandoCombate = false;
        DatosSeleccionCombate.Limpiar();
        LimpiarSlots();
        ActualizarTextoEstado();
        ActualizarBotones();
        if (botonComenzar != null)
        {
            botonComenzar.interactable = false;
        }
        SeleccionarPrimerBotonDisponible();
    }

    public void ComenzarPartida()
    {
        if (!seleccionCompleta || cargandoCombate)
        {
            return;
        }

        cargandoCombate = true;
        DatosSeleccionCombate.GuardarEquipos(equipoP1.ToArray(), equipoP2.ToArray());
        StartCoroutine(CargarCombateConTransicion());
    }

    public bool PuedeSeleccionar(DatosPersonaje datosPersonaje)
    {
        if (datosPersonaje == null || seleccionCompleta)
        {
            return false;
        }

        List<DatosPersonaje> equipoActual = ObtenerEquipoActual();
        return equipoActual.Count < personajesPorEquipo && !equipoActual.Contains(datosPersonaje);
    }

    public void NotificarFoco(BotonPersonaje botonPersonaje)
    {
        if (botonPersonaje == null || marcoSeleccion == null)
        {
            return;
        }

        RectTransform botonRect = botonPersonaje.GetComponent<RectTransform>();
        if (botonRect == null)
        {
            return;
        }

        marcoSeleccion.gameObject.SetActive(true);
        marcoSeleccion.position = botonRect.position;
        marcoSeleccion.sizeDelta = botonRect.sizeDelta + new Vector2(4f, 4f);
        marcoSeleccion.SetAsLastSibling();
        ActualizarColorMarco();

        if (botonConFoco != botonPersonaje)
        {
            botonConFoco = botonPersonaje;
            
            if (puedeSonarDesplazamiento)
            {
                ReproducirSonido(sonidoDesplazar);
            }
        }
    }

    private void PrepararBotonComenzar()
    {
        if (botonComenzar == null)
        {
            GameObject objetoBoton = GameObject.Find("BotonComenzar");
            if (objetoBoton != null)
            {
                botonComenzar = objetoBoton.GetComponent<Button>();
            }
        }

        if (botonComenzar != null)
        {
            botonComenzar.interactable = false;
            botonComenzar.onClick.RemoveListener(ComenzarPartida);
            botonComenzar.onClick.AddListener(ComenzarPartida);
        }
    }

    private void PrepararPanelTransicion()
    {
        if (panelTransicion == null)
        {
            return;
        }

        panelTransicion.color = new Color(0f, 0f, 0f, 0f);
        panelTransicion.raycastTarget = true;
        panelTransicion.gameObject.SetActive(false);
    }

    private void CompletarSeleccion()
    {
        seleccionCompleta = true;
        DatosSeleccionCombate.GuardarEquipos(equipoP1.ToArray(), equipoP2.ToArray());
        ActualizarTextoEstado();
        ActualizarBotones();

        if (marcoSeleccion != null)
        {
            marcoSeleccion.gameObject.SetActive(false);
        }

        if (botonComenzar != null)
        {
            botonComenzar.interactable = true;
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(botonComenzar.gameObject);
            }
        }
    }

    private IEnumerator CargarCombateConTransicion()
    {
        if (botonComenzar != null)
        {
            botonComenzar.interactable = false;
        }

        Image panel = panelTransicion;
        if (panel != null)
        {
            panel.gameObject.SetActive(true);
            panel.transform.SetAsLastSibling();

            float tiempo = 0f;
            while (tiempo < duracionTransicion && panel != null)
            {
                tiempo += Time.unscaledDeltaTime;
                float alpha = Mathf.Clamp01(tiempo / duracionTransicion);
                panel.color = new Color(0f, 0f, 0f, alpha);
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
        }

        if (this != null)
        {
            SceneManager.LoadScene(escenaCombate);
        }
    }
    private void ActualizarTextoEstado()
    {
        if (textoEstado == null)
        {
            return;
        }

        if (seleccionCompleta)
        {
            textoEstado.text = "Equipos listos. Pulsa comenzar";
            return;
        }

        if (jugadorActual == 1)
        {
            textoEstado.text = "Jugador 1 elige " + (equipoP1.Count + 1) + " / " + personajesPorEquipo;
        }
        else
        {
            textoEstado.text = "Jugador 2 elige " + (equipoP2.Count + 1) + " / " + personajesPorEquipo;
        }
    }

    private void ActualizarBotones()
    {
        if (botonesPersonaje == null)
        {
            return;
        }

        for (int i = 0; i < botonesPersonaje.Length; i++)
        {
            if (botonesPersonaje[i] != null)
            {
                botonesPersonaje[i].ActualizarInteractividad(PuedeSeleccionar(botonesPersonaje[i].datosPersonaje));
            }
        }

        ActualizarColorMarco();
    }

    private void ActualizarSlots()
    {
        ActualizarSlotsEquipo(equipoP1, slotsP1);
        ActualizarSlotsEquipo(equipoP2, slotsP2);
    }

    private void ActualizarSlotsEquipo(List<DatosPersonaje> equipo, Image[] slots)
    {
        LimpiarSlots(slots);

        if (slots == null)
        {
            return;
        }

        for (int i = 0; i < equipo.Count && i < slots.Length; i++)
        {
            if (slots[i] != null && equipo[i] != null)
            {
                slots[i].sprite = equipo[i].iconoSelector;
                slots[i].color = Color.white;
            }
        }
    }

    private void LimpiarSlots()
    {
        LimpiarSlots(slotsP1);
        LimpiarSlots(slotsP2);
    }

    private void LimpiarSlots(Image[] slots)
    {
        if (slots == null)
        {
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
            {
                slots[i].sprite = null;
                slots[i].color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }

    private void SeleccionarPrimerBotonDisponible()
    {
        if (EventSystem.current == null || botonesPersonaje == null)
        {
            return;
        }

        for (int i = 0; i < botonesPersonaje.Length; i++)
        {
            BotonPersonaje boton = botonesPersonaje[i];
            if (boton != null && PuedeSeleccionar(boton.datosPersonaje))
            {
                EventSystem.current.SetSelectedGameObject(boton.gameObject);
                NotificarFoco(boton);
                return;
            }
        }
    }

    private void AnimarMarcoSeleccion()
    {
        if (marcoSeleccion == null || !marcoSeleccion.gameObject.activeSelf)
        {
            return;
        }

        float escala = 1f + Mathf.Sin(Time.unscaledTime * 8f) * 0.018f;
        marcoSeleccion.localScale = new Vector3(escala, escala, 1f);
    }

    private void ActualizarColorMarco()
    {
        if (marcoSeleccion == null)
        {
            return;
        }

        Image imagenMarco = marcoSeleccion.GetComponent<Image>();
        Outline bordeMarco = marcoSeleccion.GetComponent<Outline>();
        Color colorJugador = jugadorActual == 1 ? new Color(0f, 0.85f, 1f, 1f) : new Color(1f, 0.15f, 0.75f, 1f);

        if (imagenMarco != null)
        {
            imagenMarco.color = new Color(colorJugador.r, colorJugador.g, colorJugador.b, 0.015f);
        }

        if (bordeMarco != null)
        {
            bordeMarco.effectColor = colorJugador;
        }
    }

    private void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private List<DatosPersonaje> ObtenerEquipoActual()
    {
        return jugadorActual == 1 ? equipoP1 : equipoP2;
    }
}
