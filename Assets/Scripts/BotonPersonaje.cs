using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BotonPersonaje : MonoBehaviour, ISelectHandler, IPointerEnterHandler, ISubmitHandler
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

        if (boton != null)
        {
            boton.onClick.AddListener(Seleccionar);
            boton.transition = Selectable.Transition.None;
        }
    }

    void Start()
    {
        ActualizarVista();
    }

    public void Seleccionar()
    {
        if (seleccionPersonajes != null && seleccionPersonajes.PuedeSeleccionar(datosPersonaje))
        {
            seleccionPersonajes.SeleccionarPersonaje(datosPersonaje);
        }
    }

    public void ActualizarInteractividad(bool puedeSeleccionar)
    {
        if (boton == null)
        {
            boton = GetComponent<Button>();
        }

        if (boton != null)
        {
            boton.interactable = puedeSeleccionar;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        AvisarFoco();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        AvisarFoco();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Seleccionar();
    }

    private void AvisarFoco()
    {
        if (seleccionPersonajes != null)
        {
            seleccionPersonajes.NotificarFoco(this);
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
            imagenIcono.color = Color.white;
            imagenIcono.preserveAspect = true;
        }

        if (textoNombre != null)
        {
            textoNombre.text = datosPersonaje.nombrePersonaje;
        }
    }
}
