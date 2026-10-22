
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject panelInstrucciones;

    public void Jugar()
    {
        
    }

    public void MostrarInstrucciones()
    {
        panelInstrucciones.SetActive(true);
    }

    public void OcultarInstrucciones()
    {
        panelInstrucciones.SetActive(false);
    }

    public void Salir()
    {
        Application.Quit();
    }
}
