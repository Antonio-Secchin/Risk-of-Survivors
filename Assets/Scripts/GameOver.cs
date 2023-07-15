using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A classe <c> GameOver()</c> controla a tela de game over (Derrota do jogador).
/// </summary>
public class GameOver : MonoBehaviour
{
    /// <summary>
    /// A funcao <c> PlayGame()</c> inicia o jogo, carregando a cena do mapa.
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// A funcao <c> GoToMainMenu)</c> muda a cena para o menu inicial.
    /// </summary>
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
