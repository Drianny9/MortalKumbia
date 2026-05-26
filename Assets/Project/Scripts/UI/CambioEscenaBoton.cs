using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscenaBoton : MonoBehaviour
{
    public CanvasGroup fadeNegro;
    public float tiempoEspera = 0.7f;
    public float tiempoFade = 0.5f;

    public void IrAEscena(string nombreEscena)
    {
        StartCoroutine(CambiarConDelay(nombreEscena));
    }

    private IEnumerator CambiarConDelay(string nombreEscena)
    {
        yield return new WaitForSeconds(tiempoEspera);

        float tiempo = 0f;

        while (tiempo < tiempoFade)
        {
            tiempo += Time.deltaTime;
            fadeNegro.alpha = tiempo / tiempoFade;
            yield return null;
        }

        fadeNegro.alpha = 1f;

        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene(nombreEscena);
    }
}