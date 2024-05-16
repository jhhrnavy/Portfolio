using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public CoinDisplay coinDisplay;
    public TextMeshProUGUI clearTimeText;
    public TextMeshProUGUI rewardText;

    // Volume Options
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private void Start()
    {
        LoadVolume();
    }

    public void OnClickBtnPlayGame()
    {
        MissionManager.Instance.LoadLevel();
    }

    public void OnClickBtnSelectMission(int missionNumber)
    {
        MissionManager.Instance.SelectMission(missionNumber);
        SetClearTimeText(MissionManager.Instance.missionDatas[missionNumber].cleartime);
        SetRewardText();
    }

    public void OnClickBtnQuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetClearTimeText(float time)
    {
        int hours = (int)(time / 3600) % 24;
        int minutes = (int)(time / 60) % 60;
        int seconds = (int)time % 60;

        // 초의 소수점 두번쨰 자리까지의 숫자
        float fraction = time - (int)time;
        int milliseconds = (int)(fraction * 100);

        clearTimeText.text = string.Format("ClearTime : {0:00}:{1:00}:{2:00}:{3:00}", hours, minutes, seconds, milliseconds);
    }

    public void SetRewardText()
    {
        rewardText.text = "Reward : " + MissionManager.Instance.GetCurrentMissionData().reward;
    }
    #region Volume Method

    public void SetMusicVolume()
    {
        float volume = _musicSlider.value;
        AudioManager.Instance.AudioMixer.SetFloat("BGM", Mathf.Log10( volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = _sfxSlider.value;
        AudioManager.Instance.AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadVolume()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
            _musicSlider.value = PlayerPrefs.GetFloat("musicVolume");

        if(PlayerPrefs.HasKey("sfxVolume"))
            _sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVolume();
        SetSFXVolume();
    }

    #endregion
}
