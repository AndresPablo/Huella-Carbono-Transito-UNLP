
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{

    public float time = 5f;

    void Start()
    {
        Invoke("Kill", time);
    }

    void Kill()
    {
        Destroy(gameObject);
        GameManager.singleton.EliminarAgente(this.GetComponent<Agente>());
    }
}
