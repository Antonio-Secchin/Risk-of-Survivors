using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A classe <c> Creditos()</c> controla a tela de créditos (Vitória do jogador).
/// </summary>
public class Creditos : MonoBehaviour
{
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
        SceneManager.LoadScene(0);
    }
}
