using UnityEngine;
using UnityEngine.SceneManagement;

public class MonitorInactividad : MonoBehaviour
{
    // Tiempo máximo de inactividad en segundos antes de reiniciar la escena
    public float minutosInactivo = 20f;  // 5 minutos (puedes ajustar este valor)

    private float idleTimer = 0f;

    void Update()
    {
        // Aumenta el temporizador de inactividad con el tiempo transcurrido desde el último frame
        idleTimer += Time.deltaTime;

        // Verifica si hay interacción del usuario (toques en pantalla o movimiento del mouse)
        if (Input.anyKeyDown || Input.touchCount > 0)
        {
            // Restablece el temporizador de inactividad cuando haya interacción
            idleTimer = 0f;
        }

        // Si se excede el tiempo máximo de inactividad, reinicia la escena
        if (idleTimer >= (minutosInactivo * 60))
        {
            RestartScene();
        }
    }

    // Reinicia la escena actual
    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
