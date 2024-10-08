using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    [SerializeField] GameObject carPrefab;
    [SerializeField] GameObject bikePrefab;
    [SerializeField] GameObject trainPrefab;
    [SerializeField] GameObject busPrefab;

    [SerializeField] Autovia calle_izq_1;
    [SerializeField] Autovia calle_izq_2;
    [SerializeField] Autovia calle_der_1;
    [SerializeField] Autovia calle_der_2;
    [SerializeField] Autovia via_tren;

    [SerializeField] Transform carPooler;
    [SerializeField] Transform busPooler;
    [SerializeField] Transform biciPooler;
    [SerializeField] Transform trenPooler;


    void Start()
    {
        
    }

    public Agente GenerarAuto()
    {
        GameObject go = Instantiate(carPrefab);
        AsignarCalle(go);
        return go.GetComponent<Agente>();
    }

    public Agente GenerarBus()
    {
        GameObject go = Instantiate(busPrefab);
        AsignarCalle(go);
        return go.GetComponent<Agente>();
    }

        public Agente GenerarBicicleta()
    {
        GameObject go = Instantiate(bikePrefab);
        AsignarCalle(go);
        return go.GetComponent<Agente>();
    }



    void AsignarCalle(GameObject vehiculo)
    {
        GetRandomCalle().ColocarVehiculo(vehiculo);
    }

    
    public Autovia GetRandomCalle()
    {
        // Generar una lista de objetos
        List<Autovia> calles = new List<Autovia>();

        // Aquí puedes agregar tus objetos, por ejemplo:
        calles.Add(calle_izq_1);
        calles.Add(calle_izq_2);
        calles.Add(calle_der_1);
        calles.Add(calle_der_2);

        // Devolver un objeto aleatorio de la lista
        if (calles.Count > 0)
        {
            int randomIndex = Random.Range(0, calles.Count);
            return calles[randomIndex];
        }

        return null; // Retorna null si la lista está vacía
    }
}