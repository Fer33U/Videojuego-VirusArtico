using System.Collections;
using UnityEngine;
using TMPro;

public class EscenaDialogo : MonoBehaviour
{
    [Header("Componentes")]
    public TextMeshProUGUI dialogoNoticiero;
    public GameObject canvasEscenaInicio; // Referencia al Canvas de inicio
    public GameObject canvasReglasInstrucciones; // Referencia al Canvas de reglas e instrucciones

    [Header("Configuración de diálogo")]
    [TextArea(3, 5)]
    public string[] lineasDeTexto;
    public float velocidadDeTipeo = 0.05f;

    private int indiceActual = 0;
    private bool escribiendoTexto = false;

    void Start()
    {
        if (lineasDeTexto.Length > 0)
        {
            MostrarLinea(indiceActual);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (escribiendoTexto)
            {
                // Si está escribiendo, completar de inmediato el texto actual
                StopAllCoroutines();
                dialogoNoticiero.text = lineasDeTexto[indiceActual];
                escribiendoTexto = false;
            }
            else
            {
                // Si no está escribiendo, avanzar al siguiente texto
                SiguienteLinea();
            }
        }
    }

    void MostrarLinea(int indice)
    {
        StartCoroutine(TipearTexto(lineasDeTexto[indice]));
    }

    IEnumerator TipearTexto(string texto)
    {
        escribiendoTexto = true;
        dialogoNoticiero.text = "";
        foreach (char letra in texto.ToCharArray())
        {
            dialogoNoticiero.text += letra;
            yield return new WaitForSeconds(velocidadDeTipeo);
        }
        escribiendoTexto = false;
    }

    void SiguienteLinea()
    {
        if (indiceActual < lineasDeTexto.Length - 1)
        {
            indiceActual++;
            MostrarLinea(indiceActual);
        }
        else
        {
            // Si se llegó al final del diálogo, finalizar diálogo y mostrar el nuevo Canvas
            FinalizarDialogo();
        }
    }

    void FinalizarDialogo()
    {
        if (canvasEscenaInicio != null)
        {
            canvasEscenaInicio.SetActive(false);
        }

        if (canvasReglasInstrucciones != null)
        {
            canvasReglasInstrucciones.SetActive(true);
        }

        Debug.Log("Fin del diálogo. Canvas de reglas e instrucciones activado.");
    }
}
