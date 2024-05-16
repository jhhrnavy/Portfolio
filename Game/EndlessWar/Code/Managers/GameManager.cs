using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public string thisScene;
    private string _mainMenu = "Main Menu";

    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += PlayerDied;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= PlayerDied;
    }

    private void Start()
    {
        thisScene = SceneManager.GetActiveScene().name;

        Time.timeScale = 1f;
        UIManager.Instance?.Stopwatch.StartStopwatch();
    }

    public void PauseOrContinue()
    {
        Time.timeScale = (Time.timeScale == 1) ? 0f : 1f;
    }

    public void ReStart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(thisScene, LoadSceneMode.Single);
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive); // UI

    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(_mainMenu);
    }

    // Mission Failed
    public void PlayerDied()
    {
        PauseOrContinue();
        UIManager.Instance?.ShowMissionFailedPopup();
    }
}
