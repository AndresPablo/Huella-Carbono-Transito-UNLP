using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PanelesInfo : MonoBehaviour
{
    public GameObject background;

    public GameObject autos_panel;
    public GameObject bicis_panel;
    public GameObject buses_panel;
    public GameObject trenes_panel;
    public GameObject ciclovia_panel;
    public GameObject solobus_panel;
    public GameObject caminata_panel;

    [Space]

    public GameObject boton_mejorar_ciclovias;
    public GameObject boton_mejorar_solobus;
    public GameObject boton_mejorar_caminata;
    [Space]
    public GameObject check_ciclovias_listo;
    public GameObject check_solobus_listo;
    public GameObject check_caminata_listo;


    void Start()
    {
        ApagarTodosLosHijos();
        check_solobus_listo.SetActive(false);
        check_ciclovias_listo.SetActive(false);
        check_caminata_listo.SetActive(false);
    }

    // Método para apagar todos los hijos
    public void ApagarTodosLosHijos()
    {
        // Iterar sobre todos los hijos
        foreach (Transform hijo in transform)
        {
            hijo.gameObject.SetActive(false);
        }
    }

    public void Mostrar_MejoraCiclovia()
    {
        ApagarTodosLosHijos();
        boton_mejorar_ciclovias.GetComponent<Button>().interactable = false;
        boton_mejorar_ciclovias.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0, 0, 0, 0.73f);
        check_ciclovias_listo.SetActive(true);
    }

    public void Mostrar_MejoraSolobus()
    {
        ApagarTodosLosHijos();
        boton_mejorar_solobus.GetComponent<Button>().interactable = false;
        boton_mejorar_solobus.GetComponentInChildren<TextMeshProUGUI>().color = new Color(0, 0, 0, 0.73f);
        check_solobus_listo.SetActive(true);
    }

    public void Mostrar_MejoraCaminata()
    {
        ApagarTodosLosHijos();
        boton_mejorar_caminata.GetComponent<Button>().interactable = false; 
        boton_mejorar_caminata.GetComponentInChildren<TextMeshProUGUI>().color =  new Color(0, 0, 0, 0.73f);
        check_caminata_listo.SetActive(true);
    }



    // Abrir


    public void AbrirPanel_Autos(){
        background.SetActive(true);
        autos_panel.SetActive(true);
    }

    public void AbrirPanel_Buses(){
        background.SetActive(true);
        buses_panel.SetActive(true);
    }

    public void AbrirPanel_Bicicletas(){
        background.SetActive(true);
        bicis_panel.SetActive(true);
    }

    public void AbrirPanel_Tren(){
        background.SetActive(true);
        trenes_panel.SetActive(true);
    }


    public void AbrirPanelCiclovia(){
        background.SetActive(true);
        ciclovia_panel.SetActive(true);
    }

    public void AbrirPanelSoloBus(){
        background.SetActive(true);
        solobus_panel.SetActive(true);
    }

    public void AbrirPanelCaminata(){
        background.SetActive(true);
        caminata_panel.SetActive(true);
    }
}
