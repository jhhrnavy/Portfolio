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
  
    public bool _spawnComplete = false;
    public bool _isPlaying = false;
    #endregion

    #region Start And Update

    private void Start()
    {
        _timer = _gameTime;
        UIManager.Instance.TimeText = _timer.ToString("F1");
    }

    private void Update()
    {
        // Timer
        if (_isPlaying)
        {
            _timer -= Time.deltaTime;
            UIManager.Instance.TimeText = _timer.ToString("F1");
            if (!_spawnComplete)// 적 한번 생성
            {
                SpawnEnemy(_spawnNum);
            }
        }
        // --

        if (_timer <= 0)
            OnGameFailed("시간 초과");
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

    #endregion

    #region Manage Enemy Spawn
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
    }

    private void EnemyClear()
    {
        GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
        Destroy(enemy);
    }

    #endregion
}
