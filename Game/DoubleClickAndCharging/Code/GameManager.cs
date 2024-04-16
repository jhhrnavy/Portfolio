using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    #endregion

    #region Reference

    [SerializeField] GameObject _enemyPrefs;
    [SerializeField] GameObject _player;
    [SerializeField] Transform _startPos;

    #endregion

    #region Fields

    [Header(" [ 게임 시간 ]"), SerializeField]
    private float _gameTime = 20f;
    private float _timer;

    [Header("[ 적 생성 반경 ]"), SerializeField]
    private float _spawnRange = 5f;

    [Header("[ 적 생성 마리수 ]"), SerializeField]
    private int _spawnNum = 5;

    private bool _spawnComplete = false;
    private bool _isPlaying = false;

    private bool _isPause = false;

    private int _leftEnemy = 0;

    #endregion

    #region Properties

    #endregion
    #region Start And Update

    private void Start()
    {
        _timer = _gameTime;
        UIManager.Instance.TimeText = _timer.ToString("F1");
        PauseGame();
    }

    private void Update()
    {
        if (!_isPlaying) return;

        // Timer
        _timer -= Time.deltaTime;
        UIManager.Instance.TimeText = _timer.ToString("F1");

        // 적 생성
        if (!_spawnComplete)
            SpawnEnemy(_spawnNum);

        // 승리 패배 확인
        if (_timer <= 0)
            OnGameFailed("시간 초과");

        if (_leftEnemy == 0)
            OnGameCleared();
        //--

        // Ese button click Pause Or Continue
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isPause = !_isPause;

            if (_isPause)
                PauseGame();
            else
                ContinueGame();

            UIManager.Instance.ShowPasuePanel(_isPause);
        }
    }

    #endregion

    #region Custom Functions

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        _isPlaying = true;
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void OnGameCleared()
    {
        PauseGame();
        UIManager.Instance.ShowPanelGameOver("게임 클리어!!");
    }

    public void OnGameFailed(string text) // text : 실패 이유
    {
        UIManager.Instance.ShowPanelGameOver(text);
        EnemyClear();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion

    #region Manage Enemy Spawn And Count
    private void SpawnEnemy(int spawnNum)
    {
        for (int i = 0; i < spawnNum; i++)
        {
            GameObject tmp = Instantiate(_enemyPrefs);
            Vector3 range = Random.onUnitSphere * _spawnRange; //insideUnitSphere 구 내부의 임의 위치 onUnitSphere : 표면상 임의 위치
            range.y = 0;
            tmp.transform.position = range;

            Vector3 dir = _player.transform.position - tmp.transform.position;
            transform.LookAt(dir);
        }
        _spawnComplete = true;

        // Set left enemy
        SetLeftEnemyCount(spawnNum);
    }

    private void EnemyClear()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        Destroy(enemy);
    }

    // 플레이어
    private void SetLeftEnemyCount(int count)
    {
        _leftEnemy = count;
        UIManager.Instance.UpdateLeftEnemyCountDisplay(_leftEnemy);
    }

    // When enemy destroy
    public void ChangeLeftEnemyCount(int amount)
    {
        _leftEnemy -= amount;
        UIManager.Instance.UpdateLeftEnemyCountDisplay(_leftEnemy);
    }

    #endregion

}
