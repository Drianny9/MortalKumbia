using UnityEngine;

[CreateAssetMenu(fileName = "NuevoPersonaje", menuName = "KumbiaFighter/Personaje")]
public class PersonajeData : ScriptableObject
{
    public string nombre;
    public GameObject prefabLuchador;
}