using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A classe <c> MainMenu()</c> controla a tela do menu principal.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// A funcao <c> PlayGame()</c> inicia o jogo, carregando a cena do mapa.
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// A funcao <c> QuitGame()</c> finaliza o jogo e fecha a aplicação.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
