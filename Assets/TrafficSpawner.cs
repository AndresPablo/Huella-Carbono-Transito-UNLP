using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    // Intentos máximos para asignar una calle
    [SerializeField] int intentosMaximos = 5;

    [Header("Prefabs")]
    [SerializeField] GameObject carPrefab;
    [SerializeField] GameObject bikePrefab;
    [SerializeField] GameObject tren_prefab;
    [SerializeField] GameObject busPrefab;

    [Header("Vias")]
    [SerializeField] Autovia calle_izq_1;
    [SerializeField] Autovia calle_izq_2;
    [SerializeField] Autovia calle_der_1;
    [SerializeField] Autovia calle_der_2;
    [SerializeField] Autovia calle_ciclovia_1;
    [SerializeField] Autovia calle_ciclovia_2;
    [SerializeField] Autovia solo_bus_izq;
    [SerializeField] Autovia solo_bus_der;
    [SerializeField] Autovia via_tren_1;
    [SerializeField] Autovia via_tren_2;

    [Header("Contenedores / Poolers")]
    [SerializeField] Transform carPooler;
    [SerializeField] Transform busPooler;
    [SerializeField] Transform biciPooler;
    [SerializeField] Transform trenPooler;




    void Start()
    {
        
    }

    public Agente GenerarAuto()
    {
        // Hay una calle libre?
        Autovia carril_libre = GetRandomCalle();
        if(carril_libre)
        {
            GameObject go = Instantiate(carPrefab);
            carril_libre.ColocarVehiculo(go);
            go.transform.SetParent(carPooler);
            return go.GetComponent<Agente>();
        }
        return null;
    }

    public Agente GenerarBus(bool _carril_buses)
    {
        Autovia carril_libre;
        GameObject go;
        if(_carril_buses)
        {
            carril_libre = GetSoloBusLibre();
            if(carril_libre)
            {
                go = Instantiate(busPrefab);
                carril_libre.ColocarVehiculo(go);
                go.transform.SetParent(busPooler);
                return go.GetComponent<Agente>();
            }
            return null;
        }else{
            carril_libre = GetRandomCalle();
            if(carril_libre)
            {
                go = Instantiate(busPrefab);
                carril_libre.ColocarVehiculo(go);
                go.transform.SetParent(busPooler);
                return go.GetComponent<Agente>();
            }
            return null;
        }
    }

    public Agente GenerarBicicleta(bool _ciclovias)
    {
        if(_ciclovias)
        {
            Autovia carril_libre = GetCicloviaLibre();
            if(carril_libre)
            {
                GameObject go = Instantiate(bikePrefab);
                carril_libre.ColocarVehiculo(go);
                go.transform.SetParent(biciPooler);
                return go.GetComponent<Agente>();
            }
            return null;
        }else{
            Autovia carril_libre = GetRandomCalle();
            if(carril_libre)
            {
                GameObject go = Instantiate(bikePrefab);
                carril_libre.ColocarVehiculo(go);
                go.transform.SetParent(biciPooler);
                return go.GetComponent<Agente>();
            }
            return null;
        }
    }

    public Agente GenerarTren()
    {
        // Hay una calle libre?
        Autovia carril_libre = GetViasLibres();
        if(carril_libre)
        {
            GameObject go = Instantiate(tren_prefab);
            carril_libre.ColocarVehiculo(go);
            go.transform.SetParent(trenPooler);
            return go.GetComponent<Agente>();
        }
        return null;
    }

    public Autovia GetCicloviaLibre()
    {
        List<Autovia> ciclovias = new List<Autovia>();
        ciclovias.Add(calle_ciclovia_1);
        ciclovias.Add(calle_ciclovia_2);

        int intentosRestantes = intentosMaximos;
        while (intentosRestantes > 0)
        {
            int randomIndex = Random.Range(0, ciclovias.Count);
            Autovia ciclovia_seleccionada = ciclovias[randomIndex];

            // Verificamos si el spawn no está ocupado
            if (!ciclovia_seleccionada.spawn_ocupado)
            {
                return ciclovia_seleccionada;
            }

            // Restamos un intento
            intentosRestantes--;
        }

        // Si no se encontró ninguna calle disponible, retornamos null
        return null;
    }

    public Autovia GetSoloBusLibre()
    {
        List<Autovia> carriles_bus = new List<Autovia>();
        carriles_bus.Add(solo_bus_izq);
        carriles_bus.Add(solo_bus_der);

        int intentosRestantes = intentosMaximos;
        while (intentosRestantes > 0)
        {
            int randomIndex = Random.Range(0, carriles_bus.Count);
            Autovia carril_seleccionado = carriles_bus[randomIndex];

            // Verificamos si el spawn no está ocupado
            if (!carril_seleccionado.spawn_ocupado)
            {
                return carril_seleccionado;
            }

            // Restamos un intento
            intentosRestantes--;
        }

        // Si no se encontró ninguna calle disponible, retornamos null
        return null;
    }

    public Autovia GetViasLibres()
    {
        List<Autovia> vias_ferrocarril = new List<Autovia>();
        vias_ferrocarril.Add(via_tren_1);
        vias_ferrocarril.Add(via_tren_2);

        int intentosRestantes = intentosMaximos;
        while (intentosRestantes > 0)
        {
            int randomIndex = Random.Range(0, vias_ferrocarril.Count);
            Autovia carril_seleccionado = vias_ferrocarril[randomIndex];

            // Verificamos si el spawn no está ocupado
            if (!carril_seleccionado.spawn_ocupado)
            {
                return carril_seleccionado;
            }

            // Restamos un intento
            intentosRestantes--;
        }

        // Si no se encontró ninguna calle disponible, retornamos null
        return null;
    }

    public Autovia GetRandomCalle()
    {
        // Generar una lista de objetos
        List<Autovia> calles = new List<Autovia>();

        calles.Add(calle_izq_1);
        calles.Add(calle_izq_2);
        calles.Add(calle_der_1);
        calles.Add(calle_der_2);

        int intentosRestantes = intentosMaximos;
        while (intentosRestantes > 0)
        {
            int randomIndex = Random.Range(0, calles.Count);
            Autovia calleSeleccionada = calles[randomIndex];

            // Verificamos si el spawn no está ocupado
            if (!calleSeleccionada.spawn_ocupado)
            {
                return calleSeleccionada;
            }

            // Restamos un intento
            intentosRestantes--;
        }

        // Si no se encontró ninguna calle disponible, retornamos null
        return null;
    }
}