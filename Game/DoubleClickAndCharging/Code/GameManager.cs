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

    [Header(" [ ���� �ð� ]"), SerializeField]
    private float _gameTime = 20f;
    private float _timer;

    [Header("[ �� ���� �ݰ� ]"), SerializeField]
    private float _spawnRange = 5f;

    [Header("[ �� ���� ������ ]"), SerializeField]
    private int _spawnNum = 5;
  
    public bool _spawnComplete = false;
    public bool _isPlaying = false;

    private bool _isPause = false;
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
            if (!_spawnComplete)// �� �ѹ� ����
            {
                SpawnEnemy(_spawnNum);
            }
        }
        // --

        if (_timer <= 0)
            OnGameFailed("�ð� �ʰ�");

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
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void OnGameCleared()
    {
        PauseGame();
        UIManager.Instance.ShowPanelGameOver("���� Ŭ����!!");
    }

    public void OnGameFailed(string text) // text : ���� ����
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

    #region Manage Enemy Spawn
    private void SpawnEnemy(int spawnNum)
    {
        for (int i = 0; i < spawnNum; i++)
        {
            GameObject tmp = Instantiate(_enemyPrefs);
            Vector3 range = Random.onUnitSphere * _spawnRange; //insideUnitSphere �� ������ ���� ��ġ onUnitSphere : ǥ��� ���� ��ġ
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