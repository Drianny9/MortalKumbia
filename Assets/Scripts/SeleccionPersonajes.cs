using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeleccionPersonajes : MonoBehaviour
{
    [Header("Configuracion")]
    public int personajesPorEquipo = 3;
    public string escenaCombate = "EscenaCombate";

    [Header("UI")]
    public Text textoEstado;

    private readonly List<DatosPersonaje> equipoP1 = new List<DatosPersonaje>();
    private readonly List<DatosPersonaje> equipoP2 = new List<DatosPersonaje>();
    private int jugadorActual = 1;

    void Start()
    {
        ActualizarTextoEstado();
    }

    public void SeleccionarPersonaje(DatosPersonaje datosPersonaje)
    {
        if (datosPersonaje == null)
        {
            return;
        }

        // Primero llena el equipo del jugador 1. Cuando llega a 3, pasa al jugador 2.
        if (jugadorActual == 1)
        {
            AgregarAlEquipo(equipoP1, datosPersonaje);

            if (equipoP1.Count >= personajesPorEquipo)
            {
                jugadorActual = 2;
            }
        }
        else
        {
            AgregarAlEquipo(equipoP2, datosPersonaje);
        }

        ActualizarTextoEstado();

        // Cuando los dos equipos estan completos, guardamos la seleccion y vamos al combate.
        if (equipoP1.Count >= personajesPorEquipo && equipoP2.Count >= personajesPorEquipo)
        {
            DatosSeleccionCombate.GuardarEquipos(equipoP1.ToArray(), equipoP2.ToArray());
            SceneManager.LoadScene(escenaCombate);
        }
    }

    public void ReiniciarSeleccion()
    {
        equipoP1.Clear();
        equipoP2.Clear();
        jugadorActual = 1;
        DatosSeleccionCombate.Limpiar();
        ActualizarTextoEstado();
    }

    private void AgregarAlEquipo(List<DatosPersonaje> equipo, DatosPersonaje datosPersonaje)
    {
        // Evita meter mas personajes de los permitidos.
        if (equipo.Count < personajesPorEquipo)
        {
            equipo.Add(datosPersonaje);
        }
    }

    private void ActualizarTextoEstado()
    {
        if (textoEstado == null)
        {
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
}
