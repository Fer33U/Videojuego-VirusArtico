using UnityEngine;
using System.IO; // Para manejo de archivos
using Newtonsoft.Json; // Para deserialización JSON
using System.Collections.Generic; // Para listas
using TMPro; // Usar TextMeshPro

public class Nivel2 : MonoBehaviour
{
    public GameObject personaje1_1; // Asigna en el Inspector el GameObject del primer personaje
    public GameObject personaje2_0; // Asigna en el Inspector el GameObject del segundo personaje
    public GameObject ImageVida1;  // Asigna el GameObject ImageVida1 en el Inspector
    public GameObject CanvasGameOver; // Asigna el GameObject CanvasGameOver en el Inspector
    public GameObject CanvasWin; // Asigna en el Inspector el CanvasWin
    public GameObject CanvasEscenaInicio; // Asigna el CanvasEscenaInicio en el Inspector
    public GameObject CanvasReglasInstrucciones; // Asigna el CanvasReglasInstrucciones en el Inspector

    public TextMeshProUGUI temporizadorText; // Asigna el TextMeshProUGUI que muestra el temporizador en el Inspector

    private Player currentPlayer; // Jugador actual obtenido desde Nivel1Utilities

    private const string FilePath = "Assets/Data/players.json"; // Ruta del archivo JSON
    private const string currentPlayerFilePath = "Assets/Data/JugadorActual.json"; // Ruta del archivo del jugador actual

    private float tiempoRestante = 10f; // 2 minutos en segundos
    private bool tiempoActivo = true; // Bandera para controlar el temporizador

    void Start()
    {
        Time.timeScale = 0; // Inicialmente detener el tiempo

        // Validar que los objetos estén asignados
        if (personaje1_1 == null || personaje2_0 == null || ImageVida1 == null || CanvasGameOver == null || CanvasWin == null || temporizadorText == null || CanvasEscenaInicio == null || CanvasReglasInstrucciones == null)
        {
            Debug.LogError("Uno o más GameObjects no están asignados en el Inspector.");
            return;
        }

        // Obtener el jugador actual desde el archivo JSON o el sistema compartido
        LoadPlayersFromJson();

        // Configurar personajes en el nivel 2 según el jugador actual
        ConfigureCharacters();
    }

    void Update()
    {
        // Verificar si ImageVida1 está desactivado, si es así, mostrar CanvasGameOver y pausar el tiempo
        if (ImageVida1 != null && !ImageVida1.activeSelf)
        {
            ShowGameOver();
        }

        // Controlar el temporizador
        if (tiempoActivo)
        {
            tiempoRestante -= Time.deltaTime; // Reducir el tiempo restante

            // Convertir el tiempo restante a minutos y segundos
            int minutos = Mathf.FloorToInt(tiempoRestante / 60);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60);

            // Mostrar el tiempo en el UI usando TextMeshPro
            temporizadorText.text = string.Format("{0:00}:{1:00}", minutos, segundos);

            // Verificar si el tiempo se ha agotado
            if (tiempoRestante <= 0)
            {
                tiempoRestante = 0;
                tiempoActivo = false;
                ShowWinCanvas(); // Mostrar el CanvasWin cuando se agote el tiempo
            }
        }

        // Cambiar Time.timeScale a 1 cuando CanvasEscenaInicio y CanvasReglasInstrucciones estén inactivos
        if (!CanvasEscenaInicio.activeSelf && !CanvasReglasInstrucciones.activeSelf)
        {
            Time.timeScale = 1;
        }
    }

    void LoadPlayersFromJson()
    {
        if (File.Exists(FilePath))
        {
            try
            {
                string json = File.ReadAllText(FilePath);
                List<Player> playersList = JsonConvert.DeserializeObject<List<Player>>(json) ?? new List<Player>();

                // Seleccionamos el último jugador como el jugador actual
                currentPlayer = playersList.Count > 0 ? playersList[playersList.Count - 1] : null;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al cargar los datos del archivo JSON: " + ex.Message);
                currentPlayer = null;
            }
        }
        else
        {
            Debug.LogWarning("El archivo players.json no existe.");
            currentPlayer = null;
        }
    }

    void ConfigureCharacters()
    {
        if (currentPlayer != null)
        {
            Debug.Log($"Jugador actual en Nivel2: {currentPlayer.nombre}, personajeAsset: {currentPlayer.personajeAsset}");

            // Activar/Desactivar personajes según el personajeAsset
            if (currentPlayer.personajeAsset == "player1")
            {
                personaje1_1.SetActive(true);
                personaje2_0.SetActive(false);
            }
            else if (currentPlayer.personajeAsset == "player2")
            {
                personaje1_1.SetActive(false);
                personaje2_0.SetActive(true);
            }
            else
            {
                Debug.LogWarning("El personajeAsset del jugador no es válido.");
                ActivarPredeterminado();
            }
        }
        else
        {
            Debug.LogWarning("No se encontró el jugador actual en Nivel2.");
            ActivarPredeterminado();
        }
    }

    void ActivarPredeterminado()
    {
        Debug.Log("Activando configuración predeterminada en Nivel2.");
        personaje1_1.SetActive(true);
        personaje2_0.SetActive(false);
    }

    void ShowGameOver()
    {
        // Activar el Canvas de Game Over
        CanvasGameOver.SetActive(true);

        // Pausar el tiempo
        Time.timeScale = 0;
    }

    void ShowWinCanvas()
    {
        // Activar el CanvasWin cuando el temporizador llegue a cero
        CanvasWin.SetActive(true);
        Time.timeScale = 0;

        // Actualizar el nivel del jugador a nivel 2
        if (currentPlayer != null)
        {
            currentPlayer.nivel2 = true; // Establecer nivel2 a true
            SavePlayerData(); // Guardar los datos actualizados del jugador
            SaveCurrentPlayer(); // Guardar el jugador actual
            Debug.Log("Nivel 2 actualizado a true para el jugador actual.");
        }
    }

    // Método para guardar los datos del jugador en el archivo JSON
    void SavePlayerData()
    {
        if (currentPlayer != null)
        {
            List<Player> playersList = new List<Player>();
            if (File.Exists(FilePath))
            {
                try
                {
                    string json = File.ReadAllText(FilePath);
                    playersList = JsonConvert.DeserializeObject<List<Player>>(json) ?? new List<Player>();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError("Error al leer el archivo JSON de jugadores: " + ex.Message);
                }
            }

            // Actualizar o agregar el jugador actual
            bool playerFound = false;
            for (int i = 0; i < playersList.Count; i++)
            {
                if (playersList[i].id == currentPlayer.id)
                {
                    playersList[i] = currentPlayer;
                    playerFound = true;
                    break;
                }
            }

            if (!playerFound)
            {
                playersList.Add(currentPlayer);
            }

            // Guardar los datos actualizados en el archivo
            try
            {
                string updatedJson = JsonConvert.SerializeObject(playersList, Formatting.Indented);
                File.WriteAllText(FilePath, updatedJson);
                Debug.Log("Datos del jugador guardados correctamente.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error al guardar el archivo JSON: " + ex.Message);
            }
        }
    }

    // Método para guardar los datos del jugador actual en el archivo JugadorActual.json
    void SaveCurrentPlayer()
    {
        try
        {
            string json = JsonConvert.SerializeObject(currentPlayer, Formatting.Indented);
            string directoryPath = Path.GetDirectoryName(currentPlayerFilePath);

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(currentPlayerFilePath, json);
            Debug.Log("Jugador actual guardado correctamente en JugadorActual.json.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error al guardar el jugador actual: " + ex.Message);
        }
    }
}
