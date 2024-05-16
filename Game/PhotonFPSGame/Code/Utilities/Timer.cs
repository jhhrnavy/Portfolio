using Photon.Pun;
using System;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviourPun
{
    [SerializeField]
    private TextMeshProUGUI _timerText;

    private bool _isRunning = false;
    private bool _isEnded = false;

    [SerializeField] private float _timeLimit = 60f; // 경과 시간
    private double _startTime;

    public string TimerText { get => _timerText.text; set => _timerText.text = value; }
    public float TimeLimit { get => _timeLimit; }
    public bool IsEnded { get => _isEnded; }

    private void Start()
    {
        SetInit();
    }

    private void Update()
    {
        if (_isRunning && _isEnded == false)
        {
            double elapsedTime = PhotonNetwork.Time - _startTime;
            double leftTime = _timeLimit - elapsedTime;
            UpdateTimeText(leftTime);

            if (elapsedTime >= _timeLimit)
            {
                _isRunning = false;
                _isEnded = true;
            }
        }
    }

    private void SetInit()
    {
        _startTime = 0f;
        _isEnded = false;
        _isRunning = false;
    }

    public void StartTimer()
    {
        _startTime = PhotonNetwork.Time;
        UpdateTimeText(_timeLimit);
        _isRunning = true;
    }

    public void PauseTimer()
    {
        _isRunning = false;
    }

    public void ResetTimer()
    {
        _startTime = 0f;
        _isEnded = false;
        _isRunning = false;
        UpdateTimeText(_timeLimit);
    }

    private void UpdateTimeText(double leftTime)
    {
        photonView.RPC("UpdateTimeTextRPC",RpcTarget.All, leftTime);
    }

    [PunRPC]
    public void UpdateTimeTextRPC(double leftTime)
    {
        int hours = (int)(leftTime / 3600) % 24;
        int minutes = (int)(leftTime / 60) % 60;
        int seconds = (int)leftTime % 60;

        // 초의 소수점 두번쨰 자리까지의 숫자
        double fraction = leftTime - (int)leftTime;
        int milliseconds = (int)(fraction * 100);

        TimerText = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", hours, minutes, seconds, milliseconds);
    }
}
