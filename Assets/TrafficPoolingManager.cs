using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TrafficPoolingManager : MonoBehaviour
{
    public GameObject[] vehiclePrefabs;  // Autos, buses, camiones, bicicletas
    public int initialPoolSize = 20;
    public Transform[] lanes;  // Carriles disponibles
    public Slider vehicleCountSlider;
    public float minDistanceBetweenVehicles = 3f;

    private List<GameObject> vehiclePool;
    private List<Vector3> activeVehiclePositions;
    private int activeVehicles = 0;

    void Start()
    {
        // Inicializamos el pool de vehículos
        vehiclePool = new List<GameObject>();
        activeVehiclePositions = new List<Vector3>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject vehicle = Instantiate(vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)]);
            vehicle.SetActive(false);
            vehiclePool.Add(vehicle);
        }

        vehicleCountSlider.onValueChanged.AddListener(UpdateVehicleCount);
    }

    // Actualiza la cantidad de vehículos en pantalla según el slider
    void UpdateVehicleCount(float value)
    {
        int targetCount = Mathf.FloorToInt(value);

        while (activeVehicles < targetCount)
        {
            ActivateVehicle();
            activeVehicles++;
        }

        while (activeVehicles > targetCount)
        {
            DeactivateVehicle();
            activeVehicles--;
        }
    }

    // Activa un vehículo en un carril libre
    void ActivateVehicle()
    {
        foreach (GameObject vehicle in vehiclePool)
        {
            if (!vehicle.activeInHierarchy)
            {
                Vector3 spawnPosition = GetNonOverlappingPosition();
                vehicle.transform.position = spawnPosition;
                vehicle.SetActive(true);
                activeVehiclePositions.Add(spawnPosition);
                break;
            }
        }
    }

    // Obtiene una posición sin superponer otros vehículos
    Vector3 GetNonOverlappingPosition()
    {
        Vector3 position;
        bool validPosition;

        do
        {
            int randomLane = Random.Range(0, lanes.Length);
            position = lanes[randomLane].position;

            validPosition = true;
            foreach (Vector3 activePos in activeVehiclePositions)
            {
                if (Vector3.Distance(position, activePos) < minDistanceBetweenVehicles)
                {
                    validPosition = false;
                    break;
                }
            }
        } while (!validPosition);

        return position;
    }

    // Desactiva el último vehículo activado
    void DeactivateVehicle()
    {
        if (activeVehiclePositions.Count > 0)
        {
            Vector3 lastPosition = activeVehiclePositions[activeVehiclePositions.Count - 1];
            foreach (GameObject vehicle in vehiclePool)
            {
                if (vehicle.activeInHierarchy && vehicle.transform.position == lastPosition)
                {
                    vehicle.SetActive(false);
                    activeVehiclePositions.Remove(lastPosition);
                    break;
                }
            }
        }
    }
}
