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
    private string dataPath = "Assets/Data/players.json"; // Ruta al archivo JSON

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
        if (File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            var players = JsonConvert.DeserializeObject<Player[]>(json);

            // Buscar al jugador actual (por ejemplo, el que está en nivel1)
            if (players != null)
            {
                currentPlayer = System.Array.Find(players, player => player.nivel1);
            }
        }
        else
        {
            Debug.LogError($"El archivo JSON no existe en la ruta: {dataPath}");
        }
    }

    // Método para cambiar la vida del jugador actual y guardar en el archivo JSON
    public void SetPlayerVida(int vida)
    {
        if (currentPlayer != null)
        {
            currentPlayer.vidajugador = vida;

            // Guardar los cambios en el archivo JSON
            if (File.Exists(dataPath))
            {
                string json = File.ReadAllText(dataPath);
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

                    // Guardar los datos actualizados en el archivo JSON
                    File.WriteAllText(dataPath, JsonConvert.SerializeObject(players, Formatting.Indented));
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
