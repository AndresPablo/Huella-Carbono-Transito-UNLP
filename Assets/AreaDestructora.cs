
using UnityEngine;

public class AreaDestructora : MonoBehaviour
{
    GameManager gm;

    void Start()
    {
        gm = GameManager.singleton;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        gm.EliminarAgente( other.gameObject.GetComponent<Agente>() );
    }

}
