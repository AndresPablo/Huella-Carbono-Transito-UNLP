using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VehiclePoolingManager : MonoBehaviour
{
    public GameObject[] vehiclePrefabs;  // Prefabs de autos, buses, bicicletas
    public int initialPoolSize = 10;  // Tamaño inicial del pool
    public Transform[] spawnPoints;  // Puntos donde los vehículos se generan
    public Slider vehicleCountSlider;  // Slider para ajustar la cantidad de vehículos
    public float minDistanceBetweenVehicles = 2f;  // Distancia mínima entre vehículos
    public float activationDelay = 0.1f;  // Retraso entre la activación de vehículos (en segundos)
    public int vehiclesPerFrame = 3;  // Máximo de vehículos que se activan/desactivan por frame

    private List<GameObject> vehiclePool;  // Pool de vehículos
    private int activeVehicles = 0;  // Número actual de vehículos activos
    private List<Vector3> activeVehiclePositions;  // Posiciones actuales de los vehículos activos
    private Coroutine activationCoroutine;  // Corrutina para la activación gradual

    void Start()
    {
        // Inicializar el pool de vehículos
        vehiclePool = new List<GameObject>();
        activeVehiclePositions = new List<Vector3>();

        // Crear el pool con vehículos variados
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject vehicle = Instantiate(vehiclePrefabs[Random.Range(0, vehiclePrefabs.Length)]);
            vehicle.SetActive(false);  // Desactivar vehículos al inicio
            vehiclePool.Add(vehicle);
        }

        // Configurar el slider
        vehicleCountSlider.minValue = 0;
        vehicleCountSlider.maxValue = vehiclePool.Count;
        vehicleCountSlider.onValueChanged.AddListener(UpdateVehicleCount);
    }

    // Actualizar la cantidad de vehículos activos basado en el slider
    public void UpdateVehicleCount(float count)
    {
        int newVehicleCount = Mathf.FloorToInt(count);

        // Detener cualquier corrutina anterior para evitar conflictos
        if (activationCoroutine != null)
        {
            StopCoroutine(activationCoroutine);
        }

        // Iniciar una nueva corrutina para activar o desactivar los vehículos gradualmente
        activationCoroutine = StartCoroutine(GradualVehicleActivation(newVehicleCount));
    }

    // Corrutina para activar/desactivar vehículos gradualmente
    private IEnumerator GradualVehicleActivation(int targetVehicleCount)
    {
        while (activeVehicles != targetVehicleCount)
        {
            if (targetVehicleCount > activeVehicles)
            {
                // Activar hasta "vehiclesPerFrame" vehículos en cada ciclo
                for (int i = 0; i < vehiclesPerFrame && activeVehicles < targetVehicleCount; i++)
                {
                    ActivateVehicle();
                    activeVehicles++;
                }
            }
            else if (targetVehicleCount < activeVehicles)
            {
                // Desactivar hasta "vehiclesPerFrame" vehículos en cada ciclo
                for (int i = 0; i < vehiclesPerFrame && activeVehicles > targetVehicleCount; i++)
                {
                    DeactivateVehicle(activeVehicles - 1);
                    activeVehicles--;
                }
            }

            yield return null;  // Esperar un frame antes de continuar
        }
    }

    // Activar un vehículo desde el pool
    private void ActivateVehicle()
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

    // Obtener una posición de spawn que no esté sobre otros vehículos
    private Vector3 GetNonOverlappingPosition()
    {
        Vector3 randomPosition;
        bool validPosition = false;

        // Buscar una posición que no esté cerca de otros vehículos
        do
        {
            int randomPoint = Random.Range(0, spawnPoints.Length);
            randomPosition = spawnPoints[randomPoint].position;

            validPosition = true;
            foreach (Vector3 activePosition in activeVehiclePositions)
            {
                if (Vector3.Distance(randomPosition, activePosition) < minDistanceBetweenVehicles)
                {
                    validPosition = false;
                    break;
                }
            }
        } while (!validPosition);

        return randomPosition;
    }

    // Desactivar un vehículo
    private void DeactivateVehicle(int index)
    {
        if (index < 0 || index >= vehiclePool.Count)
        {
            Debug.LogWarning("Índice fuera de rango al desactivar vehículo");
            return;
        }

        GameObject vehicle = vehiclePool[index];

        if (vehicle.activeInHierarchy)
        {
            vehicle.SetActive(false);
            activeVehiclePositions.Remove(vehicle.transform.position);
        }
    }

    // Método opcional para resetear todos los vehículos
    public void ResetVehicles()
    {
        foreach (GameObject vehicle in vehiclePool)
        {
            vehicle.SetActive(false);
        }
        activeVehicles = 0;
        activeVehiclePositions.Clear();
        vehicleCountSlider.value = 0;
    }
}
