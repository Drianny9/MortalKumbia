using UnityEngine;
using UnityEngine.UI;

public class MostrarEquipoCampeon : MonoBehaviour
{
    [Header("Posiciones")]
    public Transform[] posicionesCampeones = new Transform[3];
    public bool invertirPersonajes;

    [Header("UI")]
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
            luchador.VolverAIdle();
            return;
        }

        AnimadorLuchador animador = instancia.GetComponent<AnimadorLuchador>();
        if (animador != null)
        {
            animador.Configurar(datosPersonaje);
            animador.VolverAIdle();
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
