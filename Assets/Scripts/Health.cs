using UnityEngine;

public class Health : MonoBehaviour
{
    public float vidaMaxima = 100f;
    public float vidaActual;
    void Start()
    {
        vidaActual = vidaMaxima;
    }

    public void RecibirDaño(float cantidad){
        vidaActual -=cantidad;
        Debug.Log(gameObject.name+" tiene "+vidaActual+" vidaActual "+" de vida.");
        if(vidaActual <= 0){
            Morir();
        }
    }

    void Morir(){
        Debug.Log(gameObject.name+" ha perdido.");
    }
}
