using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// A classe <c> Timer()</c> controla o timer do jogador.
/// </summary>
public class Timer : MonoBehaviour
{
    [SerializeField] private Image uiFill;
    [SerializeField] private Text uiText;

    public int Duration;

    private int remainingDuration;

    private bool Pause;

    private void Start()
    {
        Being(Duration);
    }

    /// <summary>
    /// A funcao <c> Being()</c> inicializa o timer..
    /// </summary>
    private void Being(int Second)
    {
        remainingDuration = Second;
        StartCoroutine(UpdateTimer());
    }

    /// <summary>
    /// A funcao <c> UpdateTimer()</c> atualiza o timer, inclusive na UI.
    /// </summary>
    private IEnumerator UpdateTimer()
    {
        while (remainingDuration >= 0)
        {
            if (!Pause)
            {
                uiText.text = $"{remainingDuration / 60:00}:{remainingDuration % 60:00}";
                uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
                remainingDuration--;
                yield return new WaitForSeconds(1f);
            }
            yield return null;
        }
        OnEnd();
    }

    private void OnEnd()
    {
        // Vitória do jogador!
        // Carrega a cena de créditos do jogo
        print("End");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
