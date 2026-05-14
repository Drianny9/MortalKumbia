using UnityEngine;
using TMPro;

public class Luchador : MonoBehaviour
{
    public float vidaMaxima = 100f;
    public float vidaActual;
    public string nombrePersonaje;
    public TextMeshProUGUI textoDeVida;
    void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarTexto();
    }
    public void RecibirDaño(float cantidad){
        vidaActual -=cantidad;
        Debug.Log(gameObject.name+" tiene "+vidaActual+" vidaActual "+" de vida.");
        ActualizarTexto();
        if(vidaActual <= 0){
            Derrota();
        }
    }

    void ActualizarTexto()
    {
        if (textoDeVida != null)
        {
            textoDeVida.text = vidaActual.ToString();
        }
    }

    void Derrota(){
        Debug.Log(gameObject.name+" ha perdido.");
    }
}
