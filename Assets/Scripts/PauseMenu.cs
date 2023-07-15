using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A classe <c> PauseMenu()</c> controla a funcionalidade de pause do jogo e seu menu.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    /// <summary>
    /// A funcao <c> Resume()</c> sai do menu de pause, reinstaurando o jogo.
    /// </summary>
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    /// <summary>
    /// A funcao <c> Pause()</c> abre o menu de pause, pausando o jogo.
    /// </summary>
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    /// <summary>
    /// A funcao <c> QuitGame()</c> finaliza o jogo e fecha a aplicação.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    /// <summary>
    /// A funcao <c> GoToMainMenu)</c> muda a cena para o menu inicial.
    /// </summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
