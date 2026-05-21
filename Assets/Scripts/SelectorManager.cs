using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic; // Obligatorio para poder usar Listas

public class SelectorManager : MonoBehaviour
{
    [Header("Referencias de la UI")]
    public Image vistaPreviaP1; // Se actualizará con el último elegido del P1
    public Image vistaPreviaP2; // Se actualizará con el último elegido del P2

    [Header("Nombre de la escena de combate")]
    public string nombreEscenaPelea = "EscenaPelea"; 

    // Cambiamos un solo personaje por listas para guardar 3 de cada uno
    [Header("Equipos Seleccionados")]
    public List<PersonajeData> equipoP1 = new List<PersonajeData>();
    public List<PersonajeData> equipoP2 = new List<PersonajeData>();

    // Esta función la llamarán los botones de la cuadrícula (Jotaro, Vegeta, etc.)
    public void SeleccionarPersonaje(PersonajeData personaje)
    {
        // El Jugador 1 elige primero hasta tener 3
        if (equipoP1.Count < 3)
        {
            equipoP1.Add(personaje);
            if (vistaPreviaP1 != null) vistaPreviaP1.sprite = personaje.fotoPerfil;
            Debug.Log($"P1 eligió a: {personaje.nombrePersonaje} ({equipoP1.Count}/3)");
        }
        // Cuando el P1 termina, los siguientes 3 clics son para el Jugador 2
        else if (equipoP2.Count < 3)
        {
            equipoP2.Add(personaje);
            if (vistaPreviaP2 != null) vistaPreviaP2.sprite = personaje.fotoPerfil;
            Debug.Log($"P2 eligió a: {personaje.nombrePersonaje} ({equipoP2.Count}/3)");
        }
        else
        {
            Debug.Log("¡Ambos equipos ya tienen 3 personajes seleccionados!");
        }
    }

    // Esta función la llamará el botón "¡A LUCHAR!"
    public void IniciarCombate()
    {
        // Solo deja pasar a la pelea si ambos equipos están completos con 3 personajes
        if (equipoP1.Count == 3 && equipoP2.Count == 3)
        {
            // Pasamos las listas completas de 3 al script que no se destruye
            DatosMenu.Instancia.equipoElegidoP1 = equipoP1;
            DatosMenu.Instancia.equipoElegidoP2 = equipoP2;

            // Cargamos la escena de la pelea
            SceneManager.LoadScene(nombreEscenaPelea);
        }
        else
        {
            Debug.LogWarning("¡Ambos jugadores deben tener 3 personajes antes de pelear!");
        }
    }

    // Por si quieren resetear la selección y elegir de nuevo
    public void LimpiarSeleccion()
    {
        equipoP1.Clear();
        equipoP2.Clear();
        if (vistaPreviaP1 != null) vistaPreviaP1.sprite = null;
        if (vistaPreviaP2 != null) vistaPreviaP2.sprite = null;
        Debug.Log("Selección reiniciada. Equipos vacíos.");
    }
}