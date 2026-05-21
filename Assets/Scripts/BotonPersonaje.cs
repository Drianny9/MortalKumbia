using UnityEngine;
using UnityEngine.UI;

public class BotonPersonaje : MonoBehaviour
{
    public DatosPersonaje datosPersonaje;
    public SeleccionPersonajes seleccionPersonajes;
    public Image imagenIcono;
    public Text textoNombre;

    private Button boton;

    void Awake()
    {
        boton = GetComponent<Button>();

        if (seleccionPersonajes == null)
        {
            seleccionPersonajes = FindObjectOfType<SeleccionPersonajes>();
        }
    }

    void Start()
    {
        ActualizarVista();

        if (boton != null)
        {
            boton.onClick.AddListener(Seleccionar);
        }
    }

    public void Seleccionar()
    {
        if (seleccionPersonajes != null && datosPersonaje != null)
        {
            seleccionPersonajes.SeleccionarPersonaje(datosPersonaje);
        }
    }

    private void ActualizarVista()
    {
        if (datosPersonaje == null)
        {
            return;
        }

        if (imagenIcono != null)
        {
            imagenIcono.sprite = datosPersonaje.iconoSelector;
        }

        if (textoNombre != null)
        {
            textoNombre.text = datosPersonaje.nombrePersonaje;
        }
    }
}
