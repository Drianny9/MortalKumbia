using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PantallaCarga : MonoBehaviour
{
    public Image[] imagenesP1;
    public Image[] imagenesP2;

    public string escenaCombate = "EscenaCombate";
    public float tiempoCarga = 15f;

    void Start()
    {
        MostrarPersonajes();
        StartCoroutine(CargarCombate());
    }

    void MostrarPersonajes()
    {
        DatosPersonaje[] equipoP1 = DatosSeleccionCombate.equipoP1;
        DatosPersonaje[] equipoP2 = DatosSeleccionCombate.equipoP2;

        for (int i = 0; i < imagenesP1.Length; i++)
        {
            if (equipoP1 != null && i < equipoP1.Length && equipoP1[i] != null)
            {
                imagenesP1[i].sprite = equipoP1[i].imagenPantallaCarga;
                imagenesP1[i].color = Color.white;
            }
        }

        for (int i = 0; i < imagenesP2.Length; i++)
        {
            if (equipoP2 != null && i < equipoP2.Length && equipoP2[i] != null)
            {
                imagenesP2[i].sprite = equipoP2[i].imagenPantallaCarga;
                imagenesP2[i].color = Color.white;
            }
        }
    }

    IEnumerator CargarCombate()
    {
        yield return new WaitForSeconds(tiempoCarga);
        SceneManager.LoadScene(escenaCombate);
    }
}