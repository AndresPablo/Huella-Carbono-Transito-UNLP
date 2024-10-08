using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetSceneOnTap : MonoBehaviour
{
    [SerializeField] int requiredTaps = 5;  // Número de toques/clics necesarios
    [SerializeField] float tapTimeLimit = 2f;  // Tiempo límite para completar los toques/clics (en segundos)
    private int tapCount = 0;
    private float lastTapTime = 0f;

    void Update()
    {
        // Detectar si hubo un toque en pantalla o un clic
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                Vector2 touchPosition = touch.position;

                if (IsTapInCenter(touchPosition))
                {
                    RegisterTap();
                }
            }
        }
        else if (Input.GetMouseButtonDown(0)) // Detectar clic izquierdo del mouse
        {
            Vector2 mousePosition = Input.mousePosition;

            if (IsTapInCenter(mousePosition))
            {
                RegisterTap();
            }
        }
    }

    // Método para verificar si el toque/clic fue en el centro de la pantalla
    bool IsTapInCenter(Vector2 position)
    {
        // Definir el área central de la pantalla
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Definir un área central (ajusta estos valores si es necesario)
        float centralAreaWidth = screenWidth * 0.2f;  // Un 20% del ancho total en el centro
        float centralAreaHeight = screenHeight * 0.2f; // Un 20% del alto total en el centro

        // Limites del área central
        float minX = (screenWidth - centralAreaWidth) / 2;
        float maxX = minX + centralAreaWidth;
        float minY = (screenHeight - centralAreaHeight) / 2;
        float maxY = minY + centralAreaHeight;

        // Verificar si el toque/clic está dentro del área central
        return (position.x > minX && position.x < maxX &&
                position.y > minY && position.y < maxY);
    }

    // Método para registrar un toque/clic
    void RegisterTap()
    {
        float currentTime = Time.time;

        // Si es el primer toque/clic o si los toques/clics están dentro del límite de tiempo
        if (tapCount == 0 || currentTime - lastTapTime <= tapTimeLimit)
        {
            tapCount++;
            lastTapTime = currentTime;

            // Si se alcanzó el número de toques/clics necesarios, reiniciar la escena
            if (tapCount >= requiredTaps)
            {
                ResetScene();
            }
        }
        else
        {
            // Si el tiempo entre toques/clics es mayor al límite, reiniciar el contador
            tapCount = 1;
            lastTapTime = currentTime;
        }
    }

    // Método para reiniciar la escena actual
    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
