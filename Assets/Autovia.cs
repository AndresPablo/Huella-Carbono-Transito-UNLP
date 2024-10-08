using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autovia : MonoBehaviour
{
    [SerializeField] int direccion = 1;
    [SerializeField] int orden_capa = 10;
    [SerializeField] Transform start;
    [SerializeField] Transform end;

    public bool spawn_ocupado;


    void Start()
    {
        
    }

    public bool ColocarVehiculo(GameObject t)
    {
        t.transform.position = start.position;
        Agente agente = t.GetComponent<Agente>();
        if(agente){
                agente.grafico.sortingOrder = orden_capa;
            if (direccion == -1){
                agente.speed = -agente.speed;
                Vector3 nuevaEscala = agente.transform.localScale;
                nuevaEscala.x = -agente.transform.localScale.x;
                agente.transform.localScale = nuevaEscala;
            }
        }

        return true;
    }

    void AsignarOrderInLayer(int order)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = order;
            Debug.Log("orderInLayer asignado: " + order);
        }
        else
        {
            Debug.LogWarning("No se encontr� un SpriteRenderer en este GameObject.");
        }
    }
}
