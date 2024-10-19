using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MantenerPantallaEncendida : MonoBehaviour
{
    void Start()
    {
        // Desactiva el apagado de la pantalla
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void OnApplicationQuit()
    {
        // Restablece el tiempo de suspensión a su valor predeterminado al salir del juego
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }
}