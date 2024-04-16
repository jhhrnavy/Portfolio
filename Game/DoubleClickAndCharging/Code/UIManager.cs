using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton

    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    #endregion

    #region Field

    [SerializeField,HideInInspector] private GameObject[] _panels;
    [SerializeField] Text _timeText;
    [SerializeField] Text _successText;
    [SerializeField] Slider _bombGage;
    [SerializeField] Slider _laserGage;
    [SerializeField] Slider _playerHpGage;

    #endregion

    #region Properties

    public string TimeText { get => _timeText.text; set => _timeText.text = value; }

    #endregion

    private void Start()
    {
        _panels[(int)PanelType.Start].SetActive(true);
    }

    #region Button Methods

    public void OnClickToStart()
    {
        _panels[(int)PanelType.Start].SetActive(false);
        GameManager.Instance.StartGame();
    }

    public void OnClickToReStart()
    {
        _panels[(int)PanelType.End].SetActive(false);
        GameManager.Instance.RestartGame();
    }

    public void OnClickToQuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    #endregion

    #region Set Init Methods

    public void SetHpDisplay(int maxHp)
    {
        _playerHpGage.maxValue = maxHp;
        _playerHpGage.value = maxHp;
    }

    public void SetLaserGageDisplay(float maxValue)
    {
        _laserGage.maxValue = maxValue;
        _laserGage.value = maxValue;
    }

    public void SetBombGageDisplay(float maxValue)
    {
        _bombGage.maxValue = maxValue;
        _bombGage.value = maxValue;

    }

    #endregion

    #region Panel

    public void ShowPanelGameOver(string text)
    {
        _panels[(int)PanelType.End].SetActive(true);
        _successText.text = text;
    }

    public void ShowPasuePanel(bool active)
    {
        _panels[(int)PanelType.Pause].SetActive(active);
    }

    #endregion

    #region Update Display Methods

    public void UpdateBombGageDisplay(float time)
    {
        _bombGage.value = time;
    }

    public void UpdateLaserGageDisplay(float time)
    {
        _laserGage.value = time;
    }

    public void UpdatePlayerHpDisplay(int hp)
    {
        _playerHpGage.value = hp;
    }

    #endregion
}

public enum PanelType
{
    Start,
    Pause,
    End,
    GamePlay,
}
