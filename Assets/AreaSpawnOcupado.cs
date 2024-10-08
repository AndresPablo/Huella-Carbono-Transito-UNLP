using UnityEngine;

public class AreaSpawnOcupado : MonoBehaviour
{
    [SerializeField] Autovia mi_autovia;


    void Start()
    {
        mi_autovia = GetComponentInParent<Autovia>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        mi_autovia.spawn_ocupado = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        mi_autovia.spawn_ocupado = false; 
    }
}
