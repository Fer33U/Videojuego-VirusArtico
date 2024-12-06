using System.IO;
using UnityEngine;
using TMPro; // Importar la librería de TextMesh Pro
using Newtonsoft.Json;

public class ScoreJugador : MonoBehaviour
{
    private Player currentPlayer; // Jugador actual
    public TMP_Text TextScore;    // Referencia al componente TextMesh Pro para mostrar el score
    private string dataPathPlayers = "Assets/Data/players.json"; // Ruta al archivo JSON de players
    private string dataPathJugadorActual = "Assets/Data/JugadorActual.json"; // Ruta al archivo JSON de JugadorActual

    void Start()
    {
        // Cargar los datos del jugador actual
        LoadCurrentPlayer();

        // Actualizar la interfaz de usuario con el score inicial
        UpdateScoreUI();
    }

    void Update()
    {
        // Cargar dinámicamente los datos actualizados del archivo JSON
        LoadCurrentPlayer();

        // Actualizar la interfaz de usuario con el score actualizado
        UpdateScoreUI();
    }

    // Método para cargar los datos del jugador actual
    private void LoadCurrentPlayer()
    {
        if (File.Exists(dataPathJugadorActual))
        {
            string json = File.ReadAllText(dataPathJugadorActual);
            currentPlayer = JsonConvert.DeserializeObject<Player>(json);
        }
        else
        {
            Debug.LogError($"El archivo JSON no existe en la ruta: {dataPathJugadorActual}");
        }
    }

    // Método para actualizar la interfaz de usuario con el score
    private void UpdateScoreUI()
    {
        if (currentPlayer != null)
        {
            int score = currentPlayer.score;

            // Formatear el score para que tenga 3 dígitos (Ejemplo: 000, 080, 999)
            TextScore.text = "Score: " + score.ToString("000");
        }
    }
}
