using UnityEngine;

[CreateAssetMenu(fileName = "DatosPersonaje", menuName = "MortalKumbia/Datos Personaje")]
public class DatosPersonaje : ScriptableObject
{
    // Este asset es la ficha del personaje.
    // Asi no hace falta crear un script distinto para cada luchador.

    [Header("Identidad")]
    public string nombrePersonaje = "Personaje";
    public GameObject prefabPersonaje;
    public Sprite iconoSelector;

    [Header("Audio")]
    public AudioClip audioSeleccion;

    [Header("Estadisticas")]
    public float vidaMaxima = 100f;
    public float danoBasico = 10f;
    public float danoEspecial = 20f;
    public float danoUlti = 50f;

    [Header("Nombres de habilidades")]
    public string nombreBasico = "Paso basico";
    public string nombreEspecial = "Paso fuerte";
    public string nombreDefensa = "Defensa";
    public string nombreUlti = "Ulti";

    [Header("Estados del Animator")]
    public string estadoIdle = "idle";
    public string estadoBasico = "basico";
    public string estadoEspecial = "especial";
    public string estadoDefensa = "bloqueo";
    public string estadoUlti = "ulti";

    public string ObtenerTextoHabilidades()
    {
        return "1 - " + nombreBasico + "\n" +
               "2 - " + nombreEspecial + "\n" +
               "3 - " + nombreDefensa + "\n" +
               "4 - " + nombreUlti;
    }
}
