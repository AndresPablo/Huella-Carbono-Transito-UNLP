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

    public bool ColocarVehiculo(Agente agente)
    {
        agente.transform.position = start.position;
        if(agente){
            agente.AsignarOrderInLayer(orden_capa);
            if (direccion == -1){
                agente.speed = -agente.speed;
                Vector3 nuevaEscala = agente.transform.localScale;
                nuevaEscala.x = -agente.transform.localScale.x;
                agente.transform.localScale = nuevaEscala;
            }
        }

        return true;
    }


}
