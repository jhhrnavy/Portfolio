using TMPro;
using UnityEngine;

public class Stopwatch : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _timerText;

    private bool _isRunning = false;
    private float _elapsedTime; // 경과 시간

    public string TimerText { get => _timerText.text; set => _timerText.text = value; }
    public float ClearTime { get => _elapsedTime; }

    private void Update()
    {
        if (_isRunning)
        {
            _elapsedTime += Time.deltaTime;
            UpdateTimeText();
        }
    }

    public void StartStopwatch()
    {
        _isRunning = true;
    }

    public void StopStopwatch()
    {
        _isRunning = false;
    }

    public void ResetStopwatch()
    {
        _elapsedTime = 0;
        UpdateTimeText();
    }

    private void UpdateTimeText()
    {
        int hours = (int)(_elapsedTime / 3600) % 24;
        int minutes = (int)(_elapsedTime / 60) % 60;
        int seconds = (int)_elapsedTime % 60;

        // 초의 소수점 두번쨰 자리까지의 숫자
        float fraction = _elapsedTime - (int)_elapsedTime;
        int milliseconds = (int)(fraction * 100);

        TimerText = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", hours, minutes, seconds, milliseconds);
    }

    private float GetElapsedTime()
    {
        return _elapsedTime;
    }
}
