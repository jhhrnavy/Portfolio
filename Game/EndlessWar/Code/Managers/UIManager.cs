using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private HealthBarUI _healthBarUI;
    [SerializeField] private ShieldBarUI _shieldBarUI;
    [SerializeField] private MagazineUI _magazineUI;
    [SerializeField] private GameObject _missionCompletePopup;
    [SerializeField] private GameObject _missionFailedPopup;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Stopwatch _stopwatch;

    // Popup Text
    [SerializeField] private TextMeshProUGUI _rewardText;
    [SerializeField] private TextMeshProUGUI _timeText;
    // -------------

    public HealthBarUI HealthBar { get => _healthBarUI; set => _healthBarUI = value; }
    public ShieldBarUI ShieldBar { get => _shieldBarUI; set => _shieldBarUI = value; }
    public MagazineUI Magazine { get => _magazineUI; set => _magazineUI = value; }
    public Stopwatch Stopwatch { get => _stopwatch;}

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            Debug.Log("call");
        }
    }

    private void Start()
    {
        _missionCompletePopup.SetActive(false);
        _missionFailedPopup.SetActive(false);
        _pausePanel.SetActive(false);
    }
    public void ActivePausePanel()
    {
        _pausePanel.SetActive(!_pausePanel.activeInHierarchy);
    }

    public void ShowMissionCompletePopup(int rewardAmount)
    {
        _missionCompletePopup.SetActive(true);
        UpdateMissionSuccessPanelTexts(rewardAmount, _stopwatch.TimerText);
    }

    public void ShowMissionFailedPopup()
    {
        _missionFailedPopup.SetActive(true);
    }

    private void UpdateMissionSuccessPanelTexts(int reward, string time)
    {
        _rewardText.text = reward.ToString();
        _timeText.text = time;
    }

    public void OnClickBtnRestartGame()
    {
        GameManager.Instance?.ReStart();
    }

    public void OnClickBtnGoToMainMenu()
    {
        GameManager.Instance?.LoadMainMenu();
    }
}
