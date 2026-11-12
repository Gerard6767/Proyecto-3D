using UnityEngine;

public class Bala : MonoBehaviour
{
    public float velocidad = 100f;
    public float tiempoVida = 3f;
    public float radio = 0.05f;

    private Rigidbody rb;
    private Transform jugador;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = true;

        Destroy(gameObject, tiempoVida);
    }

    public void Configurar(Transform propietario)
    {
        jugador = propietario;
    }

    void FixedUpdate()
    {
        float distanciaMovimiento = velocidad * Time.fixedDeltaTime;
        Vector3 direccion = transform.forward;

        RaycastHit impacto;

        if (Physics.SphereCast(
            transform.position,
            radio,
            direccion,
            out impacto,
            distanciaMovimiento
        ))
        {
            if (jugador != null && impacto.transform.IsChildOf(jugador))
            {
                transform.position += direccion * distanciaMovimiento;
                return;
            }

            ObjetoDestruible objeto = impacto.collider.GetComponentInParent<ObjetoDestruible>();

            if (objeto != null)
            {
                objeto.Romper();
            }

            Destroy(gameObject);
            return;
        }

        transform.position += direccion * distanciaMovimiento;
    }
}