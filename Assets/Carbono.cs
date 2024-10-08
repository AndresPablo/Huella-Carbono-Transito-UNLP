using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carbono : MonoBehaviour
{
    [SerializeField] UI_BarraCarbono barra_carbono;

    float huella_maxima = 100f; // Huella máxima estimada
    float huella_minima = 0f; // Huella mínima estimada

    void Start()
    {
        // Calcula huella máxima y mínima basada en posibles combinaciones
        huella_maxima = CalcularHuellaMaxima();
        huella_minima = CalcularHuellaMinima();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Calcular_Huella_Carbono()
    {
        // Factores de impacto ambiental
        float factor_auto = 1f;
        float factor_bus = 0.85f;
        float factor_bici = 0.3f;
        float factor_tren = 0.3f;
        float factor_caminar = 0.85f; // reducción del 15% si se fomenta la caminata

        // Calculo de la huella de carbono
        float huella_autos = GameManager.singleton.autos_actuales * factor_auto;
        float huella_buses = GameManager.singleton.colectivos_actuales * factor_bus;
        float huella_bicis = GameManager.singleton.bicis_actuales * factor_bici;
        float huella_trenes = GameManager.singleton.trenes_actuales * factor_tren;

        // Huella total
        float huella_total = huella_autos + huella_buses + huella_bicis + huella_trenes;

        // Aplicar reducción si se fomenta la caminata
        if (GameManager.singleton.nucleos_de_caminata) {
            huella_total *= factor_caminar;
        }

        // Normalizar la huella de carbono entre huella mínima y máxima
        float huella_normalizada = (huella_total - huella_minima) / (huella_maxima - huella_minima);
        huella_normalizada = Mathf.Clamp(huella_normalizada, 0f, 1f); // Asegurarse de que esté entre 0 y 1

        // Actualizar la barra de carbono
        barra_carbono.SetValor(huella_normalizada);
    }

    float CalcularHuellaMaxima()
    {
        // Suposición: máxima huella es cuando todos los vehículos son autos
        return GameManager.singleton.autos_max * 1f; // Autos con el factor más alto
    }

    float CalcularHuellaMinima()
    {
        // Suposición: mínima huella es cuando solo hay bicicletas o caminata
        return 0f; // Bicis y caminata no generan huella
    }

}
