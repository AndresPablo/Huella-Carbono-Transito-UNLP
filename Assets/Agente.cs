using System.Collections;
using UnityEngine;

public class Agente : MonoBehaviour
{
    public TipoVehiculo tipoVehiculo;
    public Vector2 speed;
    public Transform target;
    public SpriteRenderer grafico;

    [SerializeField] int pasajeros_min = 1;
    [SerializeField] int pasajeros_max = 4;

    public int pasajeros;

    GameManager gm;

    void Start()
    {
        gm = GameManager.singleton;  
        pasajeros = Random.Range(pasajeros_min, pasajeros_max);  
    }

    void Update()
    {
        Vector2 newpos = (Vector2)transform.position + (speed) * Time.deltaTime;
        transform.position = newpos;
    }


}