using UnityEngine;

public class Disparar : MonoBehaviour
{
    public Transform camara;
    public Transform puntoDisparo;
    public GameObject prefabBala;
    public float distancia = 100f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DispararBala();
        }
    }

    void DispararBala()
    {
        // La cámara apunta exactamente al centro de la mira
        Ray rayo = new Ray(camara.position, camara.forward);

        Vector3 objetivo = camara.position + camara.forward * distancia;

        RaycastHit impacto;

        if (Physics.Raycast(rayo, out impacto, distancia))
        {
            objetivo = impacto.point;
        }

        // La bala sale del arma y se dirige al punto de la mira
        Vector3 direccion = (objetivo - puntoDisparo.position).normalized;

        GameObject nuevaBala = Instantiate(
            prefabBala,
            puntoDisparo.position,
            Quaternion.LookRotation(direccion)
        );

        Bala scriptBala = nuevaBala.GetComponent<Bala>();

        if (scriptBala != null)
        {
            scriptBala.Configurar(transform.root);
        }
    }
}