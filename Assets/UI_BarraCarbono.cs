using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_BarraCarbono : MonoBehaviour
{
    [SerializeField] Image barra_color;
    [SerializeField] bool colorear;
    public float wobbleAmount = 0.02f; // Rango de movimiento
    public float wobbleSpeed = 5f; // Velocidad de la oscilaci�n

    [SerializeField]float target_value = .5f;
    float smooth_value;

        // Define los colores
        [Header("Colores")]
    [SerializeField]Color colorMinimo = Color.green; // Verde para mínimo
    [SerializeField]Color colorMedio = Color.yellow; // Amarillo para medio
    [SerializeField]Color colorMaximo = Color.red; // Rojo para máximo


    public void SetValor(float nuevo_valor)
    {
        target_value = nuevo_valor;
    }

    void Update()
    {

        // Calcular un valor oscilante basado en el tiempo
        float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;

        // Establecer el fillAmount
        barra_color.fillAmount = Mathf.Clamp01(target_value + wobble);

        if(colorear)
            CambiarColorBarra(target_value);
    }

    void CambiarColorBarra(float valor)
    {
        // Interpolación de color
        Color colorActual = Color.Lerp(colorMinimo,colorMaximo , valor);
        barra_color.color = colorActual; // Cambia el color de la barra
    }
}
