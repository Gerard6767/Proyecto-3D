
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Jugador : MonoBehaviour
{
    [Header("Movimiento")]
    public Transform camara;
    public float velocidad = 5f;
    public float velocidadCorrer = 8f;
    public float saltar = 2f;
    public float sensibilidadRaton = 2f;
    public float gravedad = -20f;
    public float alturaNormal = 2f;
    public float alturaAgachado = 1.2f;

    [Header("Menu de pausa")]
    public GameObject menuPausa;

    private CharacterController controlador;
    private float rotacionVertical;
    private float velocidadVertical;
    private bool agachado;
    private bool pausado;

    void Start()
    {
        controlador = GetComponent<CharacterController>();

        controlador.height = alturaNormal;
        controlador.center = new Vector3(0, alturaNormal / 2f, 0);

        if (camara == null && Camera.main != null)
        {
            camara = Camera.main.transform;
        }

        if (menuPausa != null)
        {
            menuPausa.SetActive(false);
        }

        Time.timeScale = 1f;
        pausado = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CambiarPausa();
        }

        if (pausado)
        {
            return;
        }

        Mirar();
        Mover();
        Saltar();
        Agacharse();
        ControlarRaton();
    }

    void Mirar()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        float ratonX = Input.GetAxis("Mouse X") * sensibilidadRaton;
        float ratonY = Input.GetAxis("Mouse Y") * sensibilidadRaton;

        transform.Rotate(0, ratonX, 0);

        rotacionVertical -= ratonY;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -80f, 80f);

        if (camara != null)
        {
            camara.localRotation =
                Quaternion.Euler(rotacionVertical, 0, 0);
        }
    }

    void Mover()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movimiento =
            transform.right * horizontal +
            transform.forward * vertical;

        movimiento = Vector3.ClampMagnitude(movimiento, 1f);

        float velocidadActual = velocidad;

        if (Input.GetKey(KeyCode.LeftShift) && !agachado)
        {
            velocidadActual = velocidadCorrer;
        }

        if (agachado)
        {
            velocidadActual = 2.5f;
        }

        controlador.Move(
            movimiento * velocidadActual * Time.deltaTime
        );
    }

    void Saltar()
    {
        if (controlador.isGrounded && velocidadVertical < 0)
        {
            velocidadVertical = -2f;
        }

        if (controlador.isGrounded &&
            Input.GetKeyDown(KeyCode.Space) &&
            !agachado)
        {
            velocidadVertical =
                Mathf.Sqrt(saltar * -2f * gravedad);
        }

        velocidadVertical += gravedad * Time.deltaTime;

        controlador.Move(
            Vector3.up * velocidadVertical * Time.deltaTime
        );
    }

    void Agacharse()
    {
        agachado = Input.GetKey(KeyCode.LeftControl);

        float alturaObjetivo =
            agachado ? alturaAgachado : alturaNormal;

        controlador.height = Mathf.Lerp(
            controlador.height,
            alturaObjetivo,
            10f * Time.deltaTime
        );

        controlador.center = new Vector3(
            0,
            controlador.height / 2f,
            0
        );

        if (camara != null)
        {
            Vector3 posicion = camara.localPosition;
            float alturaCamara = agachado ? 1f : 1.6f;

            posicion.y = Mathf.Lerp(
                posicion.y,
                alturaCamara,
                10f * Time.deltaTime
            );

            camara.localPosition = posicion;
        }
    }

    void ControlarRaton()
    {
        if (Input.GetMouseButtonDown(0) &&
            Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void CambiarPausa()
    {
        if (menuPausa == null)
        {
            Debug.LogError("No has asignado el MenuPausa en el Inspector.");
            return;
        }

        pausado = !pausado;
        menuPausa.SetActive(pausado);

        Time.timeScale = pausado ? 0f : 1f;

        Cursor.lockState = pausado
            ? CursorLockMode.None
            : CursorLockMode.Locked;

        Cursor.visible = pausado;
    }

    public void Reanudar()
    {
        if (!pausado)
            return;

        CambiarPausa();
    }
}

