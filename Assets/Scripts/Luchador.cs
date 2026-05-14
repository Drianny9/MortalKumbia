using UnityEngine;

public class Luchador : MonoBehaviour
{
    public float vidaMaxima = 100f;
    public float vidaActual;
    public string nombrePersonaje;
    void Start()
    {
        vidaActual = vidaMaxima;
    }
    public void RecibirDaño(float cantidad){
        vidaActual -=cantidad;
        Debug.Log(gameObject.name+" tiene "+vidaActual+" vidaActual "+" de vida.");
        if(vidaActual <= 0){
            Derrota();
        }
    }
    void Derrota(){
        Debug.Log(gameObject.name+" ha perdido.");
    }
}
