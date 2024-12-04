using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void Jugar()
    {
        // Cargar directamente la escena llamada "MenuNiveles"
        SceneManager.LoadScene("MenuSeleccionPersonaje");
    }

    public void Salir()
    {
        // Redirigir a la escena "CreacionJugador" en lugar de salir
        SceneManager.LoadScene("CreacionJugador");
    }

    public void SalirMenuPrincipal()
    {
        // Redirigir a la escena "CreacionJugador" en lugar de salir
        SceneManager.LoadScene("MenuPrincipal");
    }


    public void JugarANiveles()
    {
        // Cargar directamente la escena llamada "MenuNiveles"
        SceneManager.LoadScene("MenuNiveles");
    }
}
