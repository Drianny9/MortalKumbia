using UnityEngine;

public class GestorCombate : MonoBehaviour
{
    public enum EstadoJuego { TURNO_P1, TURNO_P2}
    public EstadoJuego estadoActual;

    public Luchador saludP1;
    public Luchador saludP2;
    void Start()
    {
        estadoActual = EstadoJuego.TURNO_P1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            EjecutarTurno();
        }
    }

    void EjecutarTurno(){
        if(estadoActual == EstadoJuego.TURNO_P1){
            saludP2.RecibirDaño(2f);
            estadoActual = EstadoJuego.TURNO_P2;
        }else{
            saludP1.RecibirDaño(2f);
            estadoActual = EstadoJuego.TURNO_P1;
        }
    }
}
