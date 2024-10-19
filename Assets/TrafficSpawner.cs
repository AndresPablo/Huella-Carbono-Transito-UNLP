using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    // Intentos máximos para asignar una calle
    [SerializeField] int intentosMaximos = 5;

    [Header("Prefabs")]
    [SerializeField] GameObject[] autosPrefabs;
    [SerializeField] GameObject[] colectivosPrefabs;
    [SerializeField] GameObject[] bicicletasPrefabs;
    [SerializeField] GameObject[] peatonesPrefabs;
    [SerializeField] GameObject tren_prefab;

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
    [SerializeField] Autovia peatonal_izq_1;
    [SerializeField] Autovia peatonal_izq_2;
    [SerializeField] Autovia peatonal_der_1;
    [SerializeField] Autovia peatonal_der_2;

    [Header("Contenedores / Poolers")]
    [SerializeField] Transform carPooler;
    [SerializeField] Transform busPooler;
    [SerializeField] Transform biciPooler;
    [SerializeField] Transform trenPooler;
    [SerializeField] Transform peatonPooler;



    void Start()
    {
        
    }

    public Agente GenerarAuto()
    {
        // Hay una calle libre?
        Autovia carril_libre;

        if(GameManager.singleton.mejora_carril_bus){
            // usar carriles sin bus
            carril_libre = GetRandomCalleSinBus();
        }else{
            // usar todos los carriles
            carril_libre = GetRandomCalle();
        }


        int randomIndex = 0;
        if(carril_libre)
        {
            if (autosPrefabs.Length > 0)
            {
                // Selecciona un prefab aleatorio del arreglo
                randomIndex = Random.Range(0, autosPrefabs.Length);
            
                GameObject go = Instantiate(autosPrefabs[randomIndex]);
                go.transform.SetParent(carPooler);
            
                Agente agente = go.GetComponent<Agente>();
                carril_libre.ColocarVehiculo(agente);
                return agente;
            }
        }
        return null;
    }

    public Agente GenerarBus(bool _mejora_carril_buses)
    {
        Autovia carril_libre;
        if(_mejora_carril_buses)
        {
            carril_libre = GetSoloBusLibre();
            if(carril_libre)
            {
                if(carril_libre)
                {
                    int randomIndex = 0;
                    if (colectivosPrefabs.Length > 0)
                    {
                        // Selecciona un prefab aleatorio del arreglo
                        randomIndex = Random.Range(0, colectivosPrefabs.Length);
                    
                        GameObject go = Instantiate(colectivosPrefabs[randomIndex]);
                        go.transform.SetParent(busPooler);

                        Agente agente = go.GetComponent<Agente>();
                        agente.via_exclusiva = true;
                        agente.speed.x += 1f; 
                        carril_libre.ColocarVehiculo(agente);
                        return agente;
                    }
                }
            }
            return null;
        }else{
            carril_libre = GetRandomCalle();
            if(carril_libre)
                {
                    int randomIndex = 0;
                    if (colectivosPrefabs.Length > 0)
                    {
                        // Selecciona un prefab aleatorio del arreglo
                        randomIndex = Random.Range(0, colectivosPrefabs.Length);
                    
                        GameObject go = Instantiate(colectivosPrefabs[randomIndex]);
                        go.transform.SetParent(busPooler);
                    
                        Agente agente = go.GetComponent<Agente>();
                        carril_libre.ColocarVehiculo(agente);
                        return agente;
                    }
                }
            return null;
        }
    }

    public Agente GenerarBicicleta(bool _ciclovias)
    {
        int randomIndex;
        randomIndex = Random.Range(0, bicicletasPrefabs.Length);

        if(_ciclovias)
        {
            Autovia carril_libre = GetCicloviaLibre();
            if(carril_libre)
            {
                GameObject go = Instantiate(bicicletasPrefabs[randomIndex]);
                go.transform.SetParent(biciPooler);
                Agente agente = go.GetComponent<Agente>();
                agente.via_exclusiva = true;
                agente.speed.x += .5f; 
                carril_libre.ColocarVehiculo(agente);
                return agente;
            }
            return null;
        }else{
            Autovia carril_libre = GetRandomCalle();
            if(carril_libre)
            {
                GameObject go = Instantiate(bicicletasPrefabs[randomIndex]);
                go.transform.SetParent(biciPooler);
                Agente agente = go.GetComponent<Agente>();
                carril_libre.ColocarVehiculo(agente);
                return agente;
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
            go.transform.SetParent(trenPooler);
            Agente agente = go.GetComponent<Agente>();
            agente.via_exclusiva = true;
            carril_libre.ColocarVehiculo(agente);
            return agente;
        }
        return null;
    }

    public Agente GenerarPeaton()
    {
        // Hay una calle libre?
        Autovia carril_libre = GetVeredaLibre();
        if(carril_libre)
        {
            int randomIndex;
            randomIndex = Random.Range(0, peatonesPrefabs.Length);
            GameObject go = Instantiate(peatonesPrefabs[randomIndex]);
            go.transform.SetParent(peatonPooler);
            Agente agente =go.GetComponent<Agente>();
            carril_libre.ColocarVehiculo(agente);
            return agente;
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

    public Autovia GetRandomCalleSinBus()
    {
        // Generar una lista de objetos
        List<Autovia> calles = new List<Autovia>();

        calles.Add(calle_izq_2);
        calles.Add(calle_der_1);

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

    public Autovia GetVeredaLibre()
    {
        List<Autovia> veredas = new List<Autovia>();
        veredas.Add(peatonal_izq_1);
        veredas.Add(peatonal_izq_2);
        veredas.Add(peatonal_der_1);
        veredas.Add(peatonal_der_2);

        int intentosRestantes = intentosMaximos;
        while (intentosRestantes > 0)
        {
            int randomIndex = Random.Range(0, veredas.Count);
            Autovia vereda_seleccionada = veredas[randomIndex];

            // Verificamos si el spawn no está ocupado
            if (!vereda_seleccionada.spawn_ocupado)
            {
                return vereda_seleccionada;
            }

            // Restamos un intento
            intentosRestantes--;
        }

        // Si no se encontró ninguna calle disponible, retornamos null
        return null;
    }

}