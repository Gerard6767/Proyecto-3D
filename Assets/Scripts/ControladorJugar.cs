
using UnityEngine;

public class ControladorJugar : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject menuPrincipal;
    public GameObject panelModos;

    public void AbrirModos()
    {
        menuPrincipal.SetActive(false);
        panelModos.SetActive(true);
    }

    public void VolverAlMenu()
    {
        panelModos.SetActive(false);
        menuPrincipal.SetActive(true);
    }
}
