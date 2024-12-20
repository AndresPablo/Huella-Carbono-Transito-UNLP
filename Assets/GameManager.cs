using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public enum TipoVehiculo {AUTO, BUS, BICICLETA, TREN, PEATON}


public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public void Awake()
    {
        singleton = this;
    }

    [SerializeField] float frecuencia_generacion_min = 0.1f;
    [SerializeField] float frecuencia_generacion_max = 0.2f;


    public float velocidad_transito = 2f;
    [SerializeField] Autovia calle_izq_1;
    [SerializeField] Autovia calle_der_1;
    [SerializeField] GameObject via_tren;
    [SerializeField] GameObject ciclovia;
    [SerializeField] GameObject solo_bus;
    [SerializeField] GameObject go_mejora_camina;
    [SerializeField] TrafficSpawner spawner;
    [Header("Sliders")]
    [SerializeField] Image autos_barra;
    [SerializeField] Slider bus_slider;
    [SerializeField] Slider bici_slider;
    [SerializeField] Slider tren_slider;
    [SerializeField] Carbono carbono_handler;

    [Header("Valores Min-Max")]
    public int autos_min = 10;
    public int autos_max = 50;
    public int buses_max = 0;
    public int bicis_max = 0;
    public int trenes_max = 0;
    public int peatones_max = 0;

    [Header("Cantidades objetivo")]
    public int autosTargetCount;
    public int busesTargetCount;
    public int bicisTargetCount;
    public int trenesTargetCount;
    public int peatonesTargetCount;


    [Header("Mejoras")]
    public bool mejora_ciclovia;
    public bool mejora_carril_bus;
    public bool nucleos_de_caminata;
    [SerializeField] UI_PanelesInfo UI_PanelesInfo;

    [Header("Vehiculos actuales")]
    public int autos_actuales;
    public int colectivos_actuales;
    public int bicis_actuales;
    public int trenes_actuales;
    public int peatones_actuales;

    public List<Agente> autosLista;
    public List<Agente> busesLista;
    public List<Agente> bicisLista;
    public List<Agente> trenesLista;
    public List<Agente> peatonesLista;


    void Start()
    {
        Configurar_Sliders_OnStart();
        StartCoroutine(RutinaSpawnearVehiculos());
        solo_bus.SetActive(false);
        via_tren.SetActive(false);
        ciclovia.SetActive(false);
        go_mejora_camina.SetActive(false);
    }


    public void Mejorar_Ciclovias(){
        mejora_ciclovia = true;
        bici_slider.maxValue = bicis_max + 6;
        ciclovia.SetActive(true);
        UI_PanelesInfo.Mostrar_MejoraCiclovia();
    }

    public void Mejorar_SoloBus(){
        mejora_carril_bus = true;
        bus_slider.maxValue = buses_max +2;
        solo_bus.SetActive(true);
        UI_PanelesInfo.Mostrar_MejoraSolobus();
    }


    public void Mejorar_Caminata(){
        nucleos_de_caminata = true;
        go_mejora_camina.SetActive(true);
        UI_PanelesInfo.Mostrar_MejoraCaminata();
        Update_PeatonesTargetCount(peatones_max);
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
        if(trenesTargetCount != 0 && cantidad_nueva == 0){
            EliminarTodosTrenes();
        }
        trenesTargetCount = ((int)cantidad_nueva);
        Update_VehicleTargetAmounts();
    }

    void Update_PeatonesTargetCount(float cantidad_nueva)
    {
        peatonesTargetCount = ((int)cantidad_nueva);
        Update_VehicleTargetAmounts();
    }

    void Update_VehicleTargetAmounts(){
        autosTargetCount = autos_max - (busesTargetCount * 3) - bicisTargetCount - (trenesTargetCount * 6) - (peatones_actuales/2);
        if(autosTargetCount < autos_min) autosTargetCount = autos_min;
    }

    public void GenerarTransportes()
    {
        float r = Random.Range(0,1f);
        if(r>.85f && trenes_actuales < trenesTargetCount-1)
        {
            Agente nuevo_tren = spawner.GenerarTren();
            
            if(nuevo_tren != null){
                trenesLista.Add(nuevo_tren);
            }
        }

        if(colectivos_actuales < busesTargetCount)
        {
            Agente nuevo_bus = spawner.GenerarBus(mejora_carril_bus);
            if(nuevo_bus != null)
            {
                busesLista.Add(nuevo_bus);
                return;
            }
        }

        if(bicis_actuales < bicisTargetCount)
        {
            Agente nueva_bicicleta = spawner.GenerarBicicleta(mejora_ciclovia);
            if(nueva_bicicleta != null){
                bicisLista.Add(nueva_bicicleta);
            }
        }

        if(autos_actuales < autosTargetCount)
        {
            Agente nuevo_auto = spawner.GenerarAuto();
            if(nuevo_auto != null){
                autosLista.Add(nuevo_auto);
            }
        }

        if(r>.75f && peatones_actuales < peatonesTargetCount){
            Agente nuevo_peaton = spawner.GenerarPeaton();
            if(nuevo_peaton != null){
                peatonesLista.Add(nuevo_peaton);
            }
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
            case TipoVehiculo.TREN:
                if(trenesLista.Count > 0)
                {
                    trenesLista.Remove(a);
                    Destroy(a.gameObject);
                }
                break;
            case TipoVehiculo.PEATON:
                if(peatonesLista.Count > 0)
                {
                    peatonesLista.Remove(a);
                    Destroy(a.gameObject);
                }
                break;
            default:
                break;
        }

        ContarVehiculosActuales();
    }

    public void EliminarTodosTrenes(){
        foreach(Agente tren in trenesLista){
            Destroy(tren.gameObject);
        }
        trenesLista.Clear();
        via_tren.SetActive(false);
        trenes_actuales = trenesLista.Count;
    }


    void ContarVehiculosActuales() {
        autos_actuales = autosLista.Count;
        autos_barra.fillAmount = autos_actuales / autos_max;
        colectivos_actuales = busesLista.Count;
        bicis_actuales = bicisLista.Count;
        trenes_actuales = trenesLista.Count;
        peatones_actuales = peatonesLista.Count;
        if(trenesTargetCount > 0){ 
            via_tren.SetActive(true);
        }
        else
        {
            if(trenes_actuales == 0) via_tren.SetActive(false);
        }


        autos_barra.fillAmount = (float)(autos_actuales - autos_min) / (float)(autos_max - autos_min);

        Calcular_Huella_Carbono();
    }




    public void Calcular_Huella_Carbono()
    {
        carbono_handler.Calcular_Huella_Carbono();
    }

    void Configurar_Sliders_OnStart()
    {
        bus_slider.maxValue = buses_max;
        bici_slider.maxValue = bicis_max;
        tren_slider.maxValue = trenes_max;

        autosTargetCount = autos_max;
        autos_barra.fillAmount = 1f;
        bus_slider.SetValueWithoutNotify(busesTargetCount);
        bici_slider.SetValueWithoutNotify(bicisTargetCount);
        tren_slider.SetValueWithoutNotify(trenesTargetCount);

        bus_slider.onValueChanged.AddListener(Update_BusesTargetCount);
        bici_slider.onValueChanged.AddListener(Update_BicicletasTargetCount);
        tren_slider.onValueChanged.AddListener(Update_TrenesTargetCount);
    }

    // Método para reiniciar la escena actual
    public void Resetear_Escena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}