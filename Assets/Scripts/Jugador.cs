
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Jugador : MonoBehaviour
{
    [Header("Camara")]
    public Transform camara;
    public float sensibilidadRaton = 2f;

    [Header("Movimiento")]
    public float velocidad = 5f;
    public float velocidadCorrer = 8f;
    public float fuerzaSalto = 7f;
    public float gravedad = -20f;

    [Header("Agacharse")]
    public Transform cuerpoVisual;
    public float alturaNormal = 2f;
    public float alturaAgachado = 1.2f;
    public float alturaCamaraNormal = 1.6f;
    public float alturaCamaraAgachado = 1f;

    [Header("Pausa")]
    public GameObject menuPausa;

    private CharacterController controlador;
    private float velocidadVertical;
    private float rotacionVertical;
    private bool agachado;
    private bool pausado;

    private Vector3 escalaVisualInicial;
    private Vector3 posicionVisualInicial;

    void Start()
    {
       // Vector3 initCamPos = camara.localPosition;
        //float initCamPosY = initCamPos.Y;
        controlador = GetComponent<CharacterController>();

        controlador.height = alturaNormal;
        controlador.center = new Vector3(0, alturaNormal / 2f, 0);

        if (camara == null && Camera.main != null)
            camara = Camera.main.transform;

        if (camara != null)
        {
            Vector3 p = camara.localPosition;
            p.y = alturaCamaraNormal;
            camara.localPosition = p;
        }

        if (cuerpoVisual != null)
        {
            escalaVisualInicial = cuerpoVisual.localScale;
            posicionVisualInicial = cuerpoVisual.localPosition;
        }

        if (menuPausa != null)
            menuPausa.SetActive(false);

        Time.timeScale = 1f;
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
    Agacharse();
    Mover();
}

    void Mirar()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        float ratonX = Input.GetAxis("Mouse X") * sensibilidadRaton;
        float ratonY = Input.GetAxis("Mouse Y") * sensibilidadRaton;

        // El cuerpo gira a izquierda y derecha.
        transform.Rotate(0f, ratonX, 0f);

        // La camara mira arriba y abajo.
        rotacionVertical -= ratonY;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -80f, 80f);

        if (camara != null)
            camara.localRotation = Quaternion.Euler(rotacionVertical, 0f, 0f);
    }

    
void Mover()
{
    bool enSuelo = controlador.isGrounded;

    // Si toca el suelo, detener la caida.
    if (enSuelo && velocidadVertical < 0f)
    {
        velocidadVertical = -2f;
    }

    // Saltar una sola vez al pulsar Espacio.
    if (enSuelo && Input.GetKeyDown(KeyCode.Space) && !agachado)
    {
        velocidadVertical = 6f;
    }

    // Aplicar gravedad continuamente.
    velocidadVertical += gravedad * Time.deltaTime;

    // Limitar la velocidad de caida.
   velocidadVertical = Mathf.Max(velocidadVertical, -20f);

    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");

    Vector3 movimiento =
        transform.right * x +
        transform.forward * z;

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

    Vector3 movimientoFinal =
        movimiento * velocidadActual +
        Vector3.up * velocidadVertical;

    controlador.Move(movimientoFinal * Time.deltaTime);
}


   void Agacharse()
{
    
    if (Input.GetKeyDown(KeyCode.LeftControl))
    {
        agachado = true;

        controlador.height = alturaAgachado;
        //controlador.center = new Vector3(0, alturaAgachado / 2f, 0);

        Vector3 posicion = camara.localPosition;
        posicion.y = controlador.height;
        //camara.localPosition = posicion;
        //transform.position = posicion;
    }
    else if (Input.GetKeyUp(KeyCode.LeftControl))
    {
        agachado = false;

        controlador.height = alturaNormal;
        //controlador.center = new Vector3(0, alturaNormal / 2f, 0);

        Vector3 posicion = camara.localPosition;
        posicion.y = controlador.height/1.25f;
        camara.localPosition = posicion;
        //transform.position = posicion;
    }
}

    public void CambiarPausa()
    {
        if (menuPausa == null)
        {
            Debug.LogError("Falta asignar el menu de pausa en Jugador.");
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
        if (pausado)
            CambiarPausa();
    }
}
