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
    public Sprite imagenPantallaCarga;
<<<<<<< HEAD
    public Sprite spriteMuerto;
    public Vector3 offsetSpriteMuerto;
    public Sprite spriteDano;
=======
>>>>>>> 033c59f40d396e0d669d3ab20d33b7081d5436fd

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
    public string estadoWin = "win";

    public string ObtenerTextoHabilidades()
    {
        return "- " + nombreBasico + "\n\n" +
               "- " + nombreEspecial + "\n\n" +
               "- " + nombreDefensa + "\n\n" +
               "- " + nombreUlti;
    }
}
