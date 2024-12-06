using UnityEngine;
using UnityEngine.UI;

public class ReglasInstrucciones : MonoBehaviour
{
    public Canvas CanvasReglasInstrucciones; // Asigna el canvas desde el inspector
    public Button ButtonCerrarReglas;       // Asigna el botón desde el inspector
    public GameObject CanvasScore; // Referencia al Canvas de reglas e instrucciones
    public GameObject CanvasVida; // Referencia al Canvas de reglas e instrucciones

    void Start()
    {
        // Asegúrate de que el botón tenga asignado este método
        if (ButtonCerrarReglas != null)
        {
            ButtonCerrarReglas.onClick.AddListener(CerrarCanvas);
        }
    }

    void Update()
    {
        // Detectar la tecla 'E'
        if (Input.GetKeyDown(KeyCode.E))
        {
            CerrarCanvas();
        }
    }

    void CerrarCanvas()
    {
        if (CanvasReglasInstrucciones != null)
        {
            CanvasReglasInstrucciones.gameObject.SetActive(false);
        }
         if (CanvasScore != null)
        {
            CanvasScore.SetActive(true);
        }

        if (CanvasVida != null)
        {
            CanvasVida.SetActive(true);
        }
    }
}
