using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class InGameUI : MonoBehaviour
{
    public static InGameUI Instance;

    [SerializeField] private GameObject _escPanel;
    [SerializeField] private GameObject _roundResultPanel;

    [SerializeField] private TextMeshProUGUI _currentAmmoText;
    [SerializeField] private TextMeshProUGUI[] _teamRoundResultTexts;

    // Score
    [SerializeField] private TextMeshProUGUI _aTeamScore;
    [SerializeField] private TextMeshProUGUI _bTeamScore;
    [SerializeField] private TextMeshProUGUI _aTeamWinCount;
    [SerializeField] private TextMeshProUGUI _bTeamWinCount;

    //Game Result
    [SerializeField] private GameObject _gameResultPanel;
    [SerializeField] private GameObject _winText;
    [SerializeField] private GameObject _loseText;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _escPanel.SetActive(false);
        _gameResultPanel.SetActive(false);
        _roundResultPanel.SetActive(false);
        _winText.SetActive(false);
        _loseText.SetActive(false);
    }

    /// <param name="currentAmmo"> ���� ������ �Ѿ� ����</param>
    /// <param name="reserveAmmo"> �������� ���� �ܿ� �Ѿ� ����</param>
    public void UpdateAmmoDisplay(int currentAmmo, int reserveAmmo)
    {
        _currentAmmoText.text = $"{currentAmmo} / {reserveAmmo}";
    }

    public void ShowEscPanel(bool active)
    {
        _escPanel.SetActive(active);
    }

    public void UpdateScore(TeamType team, int score)
    {
        switch (team)
        {
            case TeamType.A:
                _aTeamScore.text = score.ToString();
                break;
            case TeamType.B:
                _bTeamScore.text = score.ToString();
                break;
        }
    }

    public void UpdateWinCount(TeamType team, int count)
    {
        switch (team)
        {
            case TeamType.A:
                _aTeamWinCount.text = count.ToString();
                break;
            case TeamType.B:
                _bTeamWinCount.text = count.ToString();
                break;
        }
    }

    /// <param name="callback"> �ڷ�ƾ �Լ��� ������ ȣ���� �̺�Ʈ </param>
    public void ShowRoundResultPanel(TeamType winTeam, int count, Action callback)
    {
        StartCoroutine(RoundResultCoroutine(winTeam, count, callback));
    }

    public void CloseRoundResultPanel()
    {
        //Debug.Log("���� ��� â �ݱ�");
        _roundResultPanel.SetActive(false);
    }

    public void ShowGameResultPanel(GameResult result)
    {
        _gameResultPanel.SetActive(true);

        switch (result)
        {
            case GameResult.Draw:
                break;
            case GameResult.Win:
                _winText.SetActive(true);
                _gameResultPanel.GetComponent<Animator>().SetTrigger("Win");// Animation���� �ؽ�Ʈ ȿ��
                break;
            case GameResult.Lose:
                _loseText.SetActive(true);
                _loseText.GetComponent<TextEffect>().Play(); // ��ũ���� �ؽ�Ʈ ȿ��
                break;
        }
    }

    #region TextEffect

    public IEnumerator RoundResultCoroutine(TeamType winTeam, int count, Action callback)
    {
        //Debug.Log("RoundResultCoroutine ����");
        _roundResultPanel.SetActive(true);
        int teamIdx = (int)winTeam;

        _teamRoundResultTexts[teamIdx].gameObject.SetActive(true);
        var textEfx = _teamRoundResultTexts[teamIdx].GetComponent<TextEffect>();

        textEfx.PlayEffect(TextEffect.EffectType.FadeOut);
        while (!textEfx.IsEnded)
        {
            yield return null;
        }
        _teamRoundResultTexts[teamIdx].text = count.ToString(); // ���� ����
        textEfx.PlayEffect(TextEffect.EffectType.FadeIn);
        while (!textEfx.IsEnded)
        {
            yield return null;
        }
        //Debug.Log("���̵� �ƿ� ����");
        yield return new WaitForSeconds(1f);

        callback?.Invoke();
    }

    #endregion
}

public enum GameResult
{
    Draw = -1,
    Win,
    Lose
}