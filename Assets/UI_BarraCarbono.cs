using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UI_BarraCarbono : MonoBehaviour
{
    [SerializeField] Image barra_color;

    public float wobbleAmount = 0.02f; // Rango de movimiento
    public float wobbleSpeed = 5f; // Velocidad de la oscilación

    [SerializeField]float target_value = .5f;
    float smooth_value;


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
    }

    /*
    void RandomizarValorMovimiento()
    {
        target_value += target_value + RandomRange(-random_factor, random_factor);
    }
    */
}
