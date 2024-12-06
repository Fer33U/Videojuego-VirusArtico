using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class VidaJugador : MonoBehaviour
{
    private Player currentPlayer; // Jugador actual
    public Image ImageVida1;      // Imagen de la primera vida
    public Image ImageVida2;      // Imagen de la segunda vida
    public Image ImageVida3;      // Imagen de la tercera vida
    private string dataPathPlayers = "Assets/Data/players.json"; // Ruta al archivo JSON
    private string dataPathJugadorActual = "Assets/Data/JugadorActual.json"; // Ruta al archivo JSON de JugadorActual

    void Start()
    {
        // Cargar el jugador actual desde el archivo JSON
        LoadCurrentPlayer();

        // Cambiar la vida inicial del jugador a 3
        SetPlayerVida(3);

        // Actualizar la interfaz de usuario con la vida inicial
        UpdateLifeUI();
    }

    void Update()
    {
        // Cargar dinámicamente los datos actualizados del archivo JSON
        LoadCurrentPlayer();

        // Actualizar la interfaz de usuario en tiempo real
        UpdateLifeUI();
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

    // Método para cambiar la vida del jugador actual y guardar en el archivo JSON
    public void SetPlayerVida(int vida)
    {
        if (currentPlayer != null)
        {
            currentPlayer.vidajugador = vida;

            // Guardar los cambios en el archivo JSON de JugadorActual
            File.WriteAllText(dataPathJugadorActual, JsonConvert.SerializeObject(currentPlayer, Formatting.Indented));

            // También actualizar los datos en players.json
            if (File.Exists(dataPathPlayers))
            {
                string json = File.ReadAllText(dataPathPlayers);
                var players = JsonConvert.DeserializeObject<Player[]>(json);

                if (players != null)
                {
                    for (int i = 0; i < players.Length; i++)
                    {
                        if (players[i].nombre == currentPlayer.nombre && players[i].personajeAsset == currentPlayer.personajeAsset)
                        {
                            players[i] = currentPlayer;
                            break;
                        }
                    }

                    // Guardar los datos actualizados en el archivo JSON de players.json
                    File.WriteAllText(dataPathPlayers, JsonConvert.SerializeObject(players, Formatting.Indented));
                }
            }
        }
    }

    // Método para actualizar las imágenes de vida en la interfaz
    private void UpdateLifeUI()
    {
        if (currentPlayer != null)
        {
            int vida = currentPlayer.vidajugador;

            // Activar o desactivar imágenes basadas en la vida del jugador
            ImageVida1.gameObject.SetActive(vida >= 1);
            ImageVida2.gameObject.SetActive(vida >= 2);
            ImageVida3.gameObject.SetActive(vida >= 3);
        }
    }
}
