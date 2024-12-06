using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class Nivel1Funcionalidad : MonoBehaviour
{
    private bool isNearPildora = false; // Para verificar si el personaje está cerca de una píldora
    private GameObject nearbyPildora; // Para almacenar la píldora cercana

    // Referencias a los Canvas de victoria, Game Over y Score
    public GameObject CanvasWin;
    public GameObject CanvasGameOver;


    // Ruta del archivo JSON
    private string filePath = "Assets/Data/players.json";
    private Player currentPlayer; // Jugador actual

    // Contador de las píldoras con el tag "pildorab" activas
    private int pildorasRojaActivas;

    void Start()
    {
        // Cargar los datos del jugador actual
        LoadCurrentPlayer();

        // Contar las píldoras rojas activas
        pildorasRojaActivas = GameObject.FindGameObjectsWithTag("pildorab").Length;
    }

    void Update()
    {
        // Verificar si el jugador presiona la tecla E y está cerca de una píldora
        if (isNearPildora && Input.GetKeyDown(KeyCode.E))
        {
            InteractWithPildora(nearbyPildora);
        }

        // Verificar si la vida del jugador llegó a 0
        if (currentPlayer.vidajugador <= 0 && !CanvasGameOver.activeSelf)
        {
            CanvasGameOver.SetActive(true); // Mostrar el Canvas de Game Over
            Debug.Log("¡Game Over!");
        }

        // Si ya no hay píldoras rojas activas, activar el Canvas de victoria
        if (pildorasRojaActivas == 0 && !CanvasWin.activeSelf)
        {
            CanvasWin.SetActive(true); // Mostrar el Canvas de Victoria
            Debug.Log("¡Victoria!");
        }
    }

    // Usar OnTriggerEnter2D para 2D en lugar de OnTriggerEnter
    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el personaje ha entrado en el rango de una píldora
        if (other.CompareTag("pildorab") || other.CompareTag("pildoram"))
        {
            isNearPildora = true;
            nearbyPildora = other.gameObject; // Almacenar la referencia de la píldora cercana
            Debug.Log("Píldora detectada: " + other.tag);
        }
    }

    // Usar OnTriggerExit2D para 2D en lugar de OnTriggerExit
    void OnTriggerExit2D(Collider2D other)
    {
        // Verifica si el personaje ha salido del rango de la píldora
        if (other.CompareTag("pildorab") || other.CompareTag("pildoram"))
        {
            isNearPildora = false;
            nearbyPildora = null; // Limpiar la referencia de la píldora
            Debug.Log("Píldora fuera de alcance: " + other.tag);
        }
    }

    void InteractWithPildora(GameObject pildora)
    {
        if (pildora.CompareTag("pildorab"))
        {
            // Interacción con la píldora roja
            Debug.Log("Interacción con Píldora Roja");

            // Si la vida del jugador es 3, solo sumar puntos, no sumar vida
            if (currentPlayer.vidajugador == 3)
            {
                ActualizarDatosJugador(10, 0); // Solo sumar 10 al score
            }
            else
            {
                ActualizarDatosJugador(10, 1); // Sumar 10 al score y 1 a la vida
            }

            // Desactivar la píldora y disminuir el contador de píldoras rojas activas
            pildora.SetActive(false);
            pildorasRojaActivas--;
        }
        else if (pildora.CompareTag("pildoram"))
        {
            // Interacción con la píldora morada
            Debug.Log("Interacción con Píldora Morada: Restar 25 puntos y vida -1");

            // Actualizar los datos del jugador
            if (currentPlayer.score > 0) // Si el score es mayor que 0, restar puntos y vida
            {
                ActualizarDatosJugador(-25, -1); // Restar 25 al score y 1 a la vida
            }

            // Desactivar la píldora
            pildora.SetActive(false);
        }
    }

    void ActualizarDatosJugador(int puntos, int vida)
    {
        // Actualizar los datos del jugador actual
        currentPlayer.score += puntos;
        currentPlayer.vidajugador += vida;

        // Asegurarse de que la vida no exceda el límite (por ejemplo, vida máxima 3)
        if (currentPlayer.vidajugador > 3) currentPlayer.vidajugador = 3;

        // Asegurarse de que el score no sea menor que 0
        if (currentPlayer.score < 0) currentPlayer.score = 0;

        // Guardar los cambios en el archivo JSON
        GuardarDatosJugador();

        Debug.Log($"Datos actualizados para {currentPlayer.nombre}: Score = {currentPlayer.score}, Vida = {currentPlayer.vidajugador}");
    }

    void LoadCurrentPlayer()
    {
        // Leer el archivo JSON
        string json = File.ReadAllText(filePath);
        List<Player> players = JsonConvert.DeserializeObject<List<Player>>(json);

        // Obtener el jugador actual (esto debe ser determinado de alguna manera en tu juego)
        string jugadorActivo = "Fernando"; // Cambia esto por el nombre del jugador activo

        // Buscar al jugador en la lista
        currentPlayer = players.Find(p => p.nombre == jugadorActivo);

        if (currentPlayer != null)
        {
            Debug.Log("Jugador cargado: " + currentPlayer.nombre);
        }
        else
        {
            Debug.LogError("Jugador no encontrado en el archivo JSON");
        }
    }

    void GuardarDatosJugador()
    {
        // Leer el archivo JSON
        string json = File.ReadAllText(filePath);
        List<Player> players = JsonConvert.DeserializeObject<List<Player>>(json);

        // Actualizar los datos del jugador en la lista
        int index = players.FindIndex(p => p.nombre == currentPlayer.nombre);
        if (index >= 0)
        {
            players[index] = currentPlayer;
        }

        // Guardar los cambios en el archivo JSON
        string updatedJson = JsonConvert.SerializeObject(players, Formatting.Indented);
        File.WriteAllText(filePath, updatedJson);

        Debug.Log("Datos guardados en el archivo JSON");
    }
}
