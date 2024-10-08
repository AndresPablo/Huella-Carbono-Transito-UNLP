using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum TipoVehiculo {AUTO, BUS, BICICLETA, TREN}


public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public void Awake()
    {
        singleton = this;
    }

    [SerializeField] float frecuencia_generacion_min = 0.10f;
    [SerializeField] float frecuencia_generacion_max = 0.2f;
    [Range(0, 1f)] public float congestion = .85f;


    public float velocidad_transito = 2f;
    [SerializeField] Autovia calle_izq_1;
    [SerializeField] Autovia calle_der_1;
    [SerializeField] Autovia via_tren;
    [SerializeField] TrafficSpawner spawner;
    [Header("Sliders")]
    [SerializeField] Slider auto_slider;
    [SerializeField] Slider bus_slider;
    [SerializeField] Slider bici_slider;
    [SerializeField] Slider tren_slider;
    [SerializeField] UI_BarraCarbono barra_carbono;

    [Header("Valores Min-Max")]
    public int autos_min = 10;
    public int autos_max = 50;
    public int buses_max = 0;
    public int bicis_max = 0;
    public int trenes_max = 0;

    [Header("Cantidades objetivo")]
    public int autosTargetCount;
    public int busesTargetCount;
    public int bicisTargetCount;
    public int trenesTargetCount;


    [Header("Mejoras")]
    public bool vias_bici;
    public bool vias_bus;

    [Header("Vehiculos actuales")]
    public int autos_actuales;
    public int colectivos_actuales;
    public int bicis_actuales;
    public int trenes_actuales;

    public List<Agente> autosLista;
    public List<Agente> busesLista;
    public List<Agente> bicisLista;
    public List<Agente> trenesLista;


    void Start()
    {
        Configurar_Sliders_OnStart();
        StartCoroutine(RutinaSpawnearVehiculos());
    }

    void Update_AutosTargetCount(float cantidad_nueva)
    {
        autosTargetCount = (int)cantidad_nueva;
        Update_VehicleTargetAmounts();
    }

    void Update_BusesTargetCount(float cantidad_nueva)
    {
        busesTargetCount = ((int)cantidad_nueva);
        Update_VehicleTargetAmounts();
    }

    void Update_BicicletasTargetCount(float cantidad_nueva)
    {
        bicisTargetCount = ((int)cantidad_nueva);
        Update_VehicleTargetAmounts();
        
    }

    void Update_TrenesTargetCount(float cantidad_nueva)
    {
        trenesTargetCount = ((int)cantidad_nueva);
        Update_VehicleTargetAmounts();
    }

    void Update_VehicleTargetAmounts(){
        autosTargetCount -= (busesTargetCount * 5);
        if(autosTargetCount < autos_min) autosTargetCount = autos_min;
    }


    public void GenerarTransportes()
    {
        float r = Random.Range(0,1f);



        if(colectivos_actuales < busesTargetCount)
        {
            busesLista.Add(spawner.GenerarBus());
        }

        if(bicis_actuales < bicisTargetCount)
        {
            bicisLista.Add(spawner.GenerarBicicleta());
        }

        if(autos_actuales < autosTargetCount)
        {
            autosLista.Add(spawner.GenerarAuto());
        }

        ContarVehiculosActuales();
    }

    IEnumerator RutinaSpawnearVehiculos(){
        GenerarTransportes();
        ContarVehiculosActuales();
        float delay = Random.Range(frecuencia_generacion_min, frecuencia_generacion_max);
        yield return new WaitForSeconds(delay);
        StartCoroutine(RutinaSpawnearVehiculos());
    }

    public void ActualizarValores(float _congestion){
        congestion = _congestion;
    }

    void ReciclarVehiculo()
    {

    }

    public void EliminarAgente(Agente a)
    {
        switch (a.tipoVehiculo)
        {
            case TipoVehiculo.AUTO:
                if(autosLista.Count > 0)
                {
                    autosLista.Remove(a);
                    Destroy(a.gameObject);
                }
                break;
            case TipoVehiculo.BUS:
                if (busesLista.Count > 0)
                {
                    busesLista.Remove(a);
                    Destroy(a.gameObject);
                }
                break;
            case TipoVehiculo.BICICLETA:
                if(bicisLista.Count > 0)
                {
                    bicisLista.Remove(a);
                    Destroy(a.gameObject);
                }
                break;
            default:
                break;
        }

        ContarVehiculosActuales();
    }

    void ContarVehiculosActuales() {
        autos_actuales = autosLista.Count;
        auto_slider.SetValueWithoutNotify(autos_actuales);
        colectivos_actuales = busesLista.Count;
        bicis_actuales = bicisLista.Count;

        auto_slider.SetValueWithoutNotify(autos_actuales);
    }

    public void Calcular_Huella_Carbono()
    {
        // TODO formula para calcular la huella de carbono
    }

    void Configurar_Sliders_OnStart()
    {
        auto_slider.maxValue = autos_max;
        auto_slider.minValue = autos_min;
        bus_slider.maxValue = buses_max;
        bici_slider.maxValue = bicis_max;
        tren_slider.maxValue = trenes_max;

        autosTargetCount = autos_max;
        auto_slider.SetValueWithoutNotify(autos_max);
        bus_slider.SetValueWithoutNotify(busesTargetCount);
        bici_slider.SetValueWithoutNotify(bicisTargetCount);
        tren_slider.SetValueWithoutNotify(trenesTargetCount);


        auto_slider.onValueChanged.AddListener(Update_AutosTargetCount);
        bus_slider.onValueChanged.AddListener(Update_BusesTargetCount);
        bici_slider.onValueChanged.AddListener(Update_BicicletasTargetCount);
        tren_slider.onValueChanged.AddListener(Update_TrenesTargetCount);
    }
}