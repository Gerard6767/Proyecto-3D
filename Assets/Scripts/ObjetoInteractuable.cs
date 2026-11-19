using UnityEngine;

public class ObjetoInteractuable : MonoBehaviour
{
    public string nombreObjeto = "Objeto";

    public void Interactuar()
    {
        Debug.Log("Has interactuado con: " + nombreObjeto);
    }
}