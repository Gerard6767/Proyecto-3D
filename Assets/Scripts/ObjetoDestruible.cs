using UnityEngine;

public class ObjetoDestruible : MonoBehaviour
{
    public void Romper()
    {
        Destroy(transform.root.gameObject);
    }
}