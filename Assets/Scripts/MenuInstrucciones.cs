
using UnityEngine;

public class MenuInstrucciones : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject menuPrincipal;
    public GameObject panelInstrucciones;

    public void MostrarInstrucciones()
    {
        menuPrincipal.SetActive(false);
        panelInstrucciones.SetActive(true);
    }

    public void VolverAlMenu()
    {
        panelInstrucciones.SetActive(false);
        menuPrincipal.SetActive(true);
    }
}

