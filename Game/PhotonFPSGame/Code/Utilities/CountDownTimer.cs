using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class CountDownTimer : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI _textDisplay; // Display
    [SerializeField] private int _time = 3;

    private bool _isFinished = false;
    public bool IsFinished { get => _isFinished; }

    public void StartCountDown()
    {
        StartCoroutine(CountDownCoroutine(_time));
    }

    IEnumerator CountDownCoroutine(int timeAmount)
    {
        //Debug.Log("카운트 다운 시작");
        photonView.RPC("ShowCountdownText", RpcTarget.All);
        for (int i = timeAmount; i>=0; i--)
        {
            if(i > 0)
                photonView.RPC("UpdateCountdownText", RpcTarget.All, i.ToString());
            else if(i == 0)
                photonView.RPC("UpdateCountdownText", RpcTarget.All, "START");
            yield return new WaitForSeconds(1f);
        }

        photonView.RPC("CloseCountdownText", RpcTarget.All);
        _isFinished = true;
    }

    [PunRPC]
    private void UpdateCountdownText(string time)
    {
        _textDisplay.gameObject.SetActive(true);
        _textDisplay.text = time;
    }

    [PunRPC]
    private void ShowCountdownText()
    {
        //Debug.Log("카운트다운 보이기");
        _textDisplay.gameObject.SetActive(true);
    }

    [PunRPC]
    private void CloseCountdownText()
    {
        _textDisplay.gameObject.SetActive(false);
    }
}
