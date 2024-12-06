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

    // Ruta del archivo JSON para el jugador actual y los jugadores
    private string jugadorActualFilePath = "Assets/Data/JugadorActual.json";
    private string playersFilePath = "Assets/Data/players.json";
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

        // Si la vida del jugador es 3, solo sumar puntos (10 puntos)
        if (currentPlayer.vidajugador == 3)
        {
            ActualizarDatosJugador(10, 0); // Solo sumar 10 al score
        }
        else
        {
            // Si la vida del jugador es menor a 3, sumar 10 puntos y 1 vida
            ActualizarDatosJugador(10, 1); // Sumar 10 al score y 1 a la vida
        }

        // Desactivar la píldora y disminuir el contador de píldoras rojas activas
        pildora.SetActive(false);
        pildorasRojaActivas--;
    }
    else if (pildora.CompareTag("pildoram"))
    {
        // Interacción con la píldora morada
        Debug.Log("Interacción con Píldora Morada");

        // Si el score es mayor que 0, restar 25 puntos y 1 vida
        if (currentPlayer.score > 0)
        {
            ActualizarDatosJugador(-25, -1); // Restar 25 al score y 1 a la vida
        }
        else
        {
            // Si el score es 0, solo restar 1 vida
            ActualizarDatosJugador(0, -1); // Solo restar 1 a la vida
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

        // Guardar los cambios en ambos archivos JSON
        GuardarDatosJugador();

        Debug.Log($"Datos actualizados para {currentPlayer.nombre}: Score = {currentPlayer.score}, Vida = {currentPlayer.vidajugador}");
    }

    void LoadCurrentPlayer()
    {
        // Leer el archivo JSON para el jugador actual
        if (File.Exists(jugadorActualFilePath))
        {
            string json = File.ReadAllText(jugadorActualFilePath);
            currentPlayer = JsonConvert.DeserializeObject<Player>(json);

            if (currentPlayer != null)
            {
                Debug.Log("Jugador cargado: " + currentPlayer.nombre);
            }
            else
            {
                Debug.LogError("Jugador no encontrado en el archivo JSON");
            }
        }
        else
        {
            Debug.LogError("El archivo de jugador actual no existe");
        }
    }

    void GuardarDatosJugador()
    {
        // Guardar datos en JugadorActual.json
        string currentPlayerJson = JsonConvert.SerializeObject(currentPlayer, Formatting.Indented);
        File.WriteAllText(jugadorActualFilePath, currentPlayerJson);
        Debug.Log("Datos guardados en el archivo JugadorActual.json");

        // Leer el archivo players.json
        string playersJson = File.ReadAllText(playersFilePath);
        List<Player> players = JsonConvert.DeserializeObject<List<Player>>(playersJson);

        // Actualizar los datos del jugador en la lista
        int index = players.FindIndex(p => p.nombre == currentPlayer.nombre);
        if (index >= 0)
        {
            players[index] = currentPlayer;
        }
        else
        {
            players.Add(currentPlayer); // Si no existe, agregarlo
        }

        // Guardar los cambios en players.json
        string updatedPlayersJson = JsonConvert.SerializeObject(players, Formatting.Indented);
        File.WriteAllText(playersFilePath, updatedPlayersJson);

        Debug.Log("Datos guardados en el archivo players.json");
    }
}
