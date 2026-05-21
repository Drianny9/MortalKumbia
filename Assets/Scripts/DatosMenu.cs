using UnityEngine;
using System.Collections.Generic; // Para que reconozca las listas

public class DatosMenu : MonoBehaviour
{
    public static DatosMenu Instancia { get; private set; }

    // Cambiar las variables viejas por estas listas:
    public List<PersonajeData> equipoElegidoP1;
    public List<PersonajeData> equipoElegidoP2;

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}