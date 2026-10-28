using UnityEngine;
using UnityEngine.UI;

public class MoverCamara : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float velocidad = 3.5f;
    public float sensibilidadMouse = 2.0f;

    [Header("Configuración de Colección y Progreso")]
    public int totalObjetos = 4; // Asegúrate de que coincida con tus llaves
    public Text textoProgreso;   // Tu objeto TextoFinal arrastrado desde la Hierarchy

    private int objetosRecolectados = 0;
    private float rotacionX = 0.0f;
    private float rotacionY = 0.0f;

    void Start()
    {
        // Ocultar el texto al iniciar el juego
        if (textoProgreso != null)
        {
            textoProgreso.gameObject.SetActive(false);
        }

        Vector3 rotacionInicial = transform.localRotation.eulerAngles;
        rotacionX = rotacionInicial.y;
        rotacionY = rotacionInicial.x;
    }

    void Update()
    {
        // Movimiento WASD
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");

        if (inputH != 0 || inputV != 0)
        {
            Vector3 frente = transform.forward;
            Vector3 derecha = transform.right;

            frente.y = 0;
            derecha.y = 0;

            frente.Normalize();
            derecha.Normalize();

            Vector3 direccion = (frente * inputV + derecha * inputH).normalized;
            transform.position += direccion * velocidad * Time.deltaTime;
        }

        // Rotación con Clic Derecho
        if (Input.GetMouseButton(1))
        {
            rotacionX += Input.GetAxis("Mouse X") * sensibilidadMouse;
            rotacionY -= Input.GetAxis("Mouse Y") * sensibilidadMouse;
            rotacionY = Mathf.Clamp(rotacionY, -80f, 80f);

            transform.localRotation = Quaternion.Euler(rotacionY, rotacionX, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Choqué con: " + other.gameObject.name + " | Tag: " + other.tag);

        if (other.gameObject.name.Contains("Llave") || other.CompareTag("Coleccionable"))
        {
            objetosRecolectados++;
            other.gameObject.SetActive(false);

            Debug.Log("Llaves recolectadas: " + objetosRecolectados + " de " + totalObjetos);

            if (objetosRecolectados >= totalObjetos)
            {
                MostrarResultadoFinal();
            }
        }
    }

    void MostrarResultadoFinal()
    {
        Debug.Log("¡Se recolectaron todas las llaves! Mostrando interfaz final...");

        if (textoProgreso != null)
        {
            // Activa el objeto y su canvas
            textoProgreso.gameObject.SetActive(true);
            if (textoProgreso.canvas != null)
            {
                textoProgreso.canvas.gameObject.SetActive(true);
            }

            // Forzar formateo y visibilidad del texto
            textoProgreso.color = Color.white; // Cambia el color a blanco brillante
            textoProgreso.alignment = TextAnchor.MiddleCenter; // Centra el texto
            textoProgreso.horizontalOverflow = HorizontalWrapMode.Overflow;
            textoProgreso.verticalOverflow = VerticalWrapMode.Overflow;

            textoProgreso.text = "¡EXPOSICIÓN COMPLETADA!\n\n" +
                                 "Objetos Recolectados: " + objetosRecolectados + " / " + totalObjetos + "\n" +
                                 "Nivel de Desensibilización: 100%\n" +
                                 "Estado de Ansiedad: Bajo Control";
        }
    }
}