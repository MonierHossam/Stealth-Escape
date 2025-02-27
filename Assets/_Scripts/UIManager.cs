using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] PopUp winScreen;
    [SerializeField] PopUp loseScreen;

    [SerializeField] List<Button> restartButtons;

    private void Start()
    {
        foreach (var button in restartButtons)
        {
            button.onClick.AddListener(RestartClicked);
        }

        //set UIManager refernce
        GameManager.Instance.UIManager = this;
        //for when scene is restarted
        GameManager.Instance.currentGameState = GameState.Playing;
    }

    private void RestartClicked()
    {
        GameManager.Instance.RestartGame();
    }

    public void ShowWinScreen()
    {
        winScreen.Show();
    }

    public void ShowLoseScreen()
    {
        loseScreen.Show();
    }
}
