using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public UIManager UIManager;
    public GameState currentGameState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayerExited()
    {
        currentGameState = GameState.Won;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager.ShowWinScreen();
    }

    public void PlayerCaught()
    {
        currentGameState = GameState.Lost;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UIManager.ShowLoseScreen();
    }

    public void RestartGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}

public enum GameState
{
    Playing,
    Won,
    Lost
}
