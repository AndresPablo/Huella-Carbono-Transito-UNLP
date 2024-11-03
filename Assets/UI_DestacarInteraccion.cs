using System.Collections;
using UnityEngine;


public class UI_DestacarInteraccion : MonoBehaviour
{
    // Array de animadores de botones asignados en el inspector.
    public Animator[] botonesAnimators;

    // Tiempo mínimo y máximo entre activaciones aleatorias.
    public float tiempoMinimo = 10f;
    public float tiempoMaximo = 120f;

    // Nombre de la animación a activar
    private string nombreAnimacion = "SacudirArribaAbajo";

    void Start()
    {
        // Inicia la corrutina que manejará las activaciones aleatorias.
        StartCoroutine(ActivarAnimacionAleatoria());
    }

    IEnumerator ActivarAnimacionAleatoria()
    {
        while (true)
        {
            // Espera un tiempo aleatorio entre el mínimo y el máximo antes de activar el siguiente botón.
            float tiempoEspera = Random.Range(tiempoMinimo, tiempoMaximo);
            yield return new WaitForSeconds(tiempoEspera);

            // Selecciona un botón al azar.
            int indiceAleatorio = Random.Range(0, botonesAnimators.Length);
            Animator animatorSeleccionado = botonesAnimators[indiceAleatorio];

            // Activa la animación del botón seleccionado.
            animatorSeleccionado.SetTrigger(nombreAnimacion);
        }
    }
}
