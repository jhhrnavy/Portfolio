using Photon.Pun;
using UnityEngine;

public class ScoreManager : MonoBehaviourPun
{
    [SerializeField] private int _targetScore = 3; // ��ǥ ����
    [SerializeField] private int _targetWinCount = 3; // ��ǥ �¸� ����

    private int _aTeamScore = 0; // �� ���� ����
    private int _aTeamWinCount = 0; // ���� �¸� Ƚ��

    private int _bTeamScore = 0;
    private int _bTeamWinCount = 0;

    #region Initialized

    public void SetInit()
    {
        photonView.RPC("SetInitRPC", RpcTarget.All);
    }

    public void ResetWinCount()
    {
        photonView.RPC("ResetWinCountRPC", RpcTarget.All);
    }

    public void ResetScore()
    {
        photonView.RPC("ResetScoreRPC", RpcTarget.All);
    }

    #endregion

    #region Synchronization

    public void AddScore(TeamType teamType)
    {
        string team = teamType.ToString();
        photonView.RPC("AddScoreRPC", RpcTarget.All, team);
    }

    public void AddWinCount(string team)
    {
        photonView.RPC("AddWinCountRPC", RpcTarget.All, team);
    }

    #endregion

    #region Getter

    public TeamType GetWinningTeamInRound()
    {
        return (_aTeamScore > _bTeamScore) ? TeamType.A : TeamType.B;
    }

    public TeamType GetWinningTeam()
    {
        return (_aTeamWinCount > _bTeamWinCount) ? TeamType.A : TeamType.B;
    }

    public int GetTeamScore(TeamType team)
    {
        int score = -1;

        switch (team)
        {
            case TeamType.A:
                score = _aTeamScore;
                break;
            case TeamType.B:
                score = _bTeamScore;
                break;
        }

        return score;
    }

    public int GetTeamWinCount(TeamType team)
    {
        int count = -1;
        switch (team)
        {
            case TeamType.A:
                count = _aTeamWinCount;
                break;
            case TeamType.B:
                count = _bTeamWinCount;
                break;
        }
        return count;
    }

    public TeamType GetWinnerTeam()
    {
        TeamType team = _aTeamWinCount > _bTeamWinCount ? TeamType.A : TeamType.B;
        return team;
    }

    #endregion

    #region Check Reached Target

    public bool HasReachedTargetScore()
    {
        bool hasReached = false;
        if (_aTeamScore == _targetScore || _bTeamScore == _targetScore)
        {
            hasReached = true;
        }
        return hasReached;
    }

    public bool HasReachedTargetWinCount()
    {
        bool hasReached = false;
        if(_aTeamWinCount == _targetWinCount || _bTeamWinCount == _targetWinCount)
        {
            hasReached = true;
        }
        return hasReached;
    }
    #endregion

    #region PunRPC Methods

    [PunRPC]
    public void AddScoreRPC(string team) // ���� ȹ��
    {
        //Debug.Log("AddScore Call");

        if(team == "A")
        {
            ++_aTeamScore;
            InGameUI.Instance?.UpdateScore(TeamType.A, _aTeamScore);
        }
        else if(team == "B")
        {
            ++_bTeamScore;
            InGameUI.Instance?.UpdateScore(TeamType.B, _bTeamScore);
        }
    }

    [PunRPC]
    public void AddWinCountRPC(string team) // �¸� ����Ʈ ȹ��
    {
        var winTeam = TeamType.A;
        var winCount = 0;

        if( team == "A")
        {
            ++_aTeamWinCount;
            winCount = _aTeamWinCount;
        }
        else if(team == "B")
        {
            winTeam = TeamType.B;

            ++_bTeamWinCount;
            winCount = _bTeamWinCount;
        }
        InGameUI.Instance?.UpdateWinCount(winTeam, winCount);
        InGameUI.Instance?.ShowRoundResultPanel(winTeam, winCount, OnRoundResultShown); // UI ǥ�� ( �ڷ�ƾ )
    }

    [PunRPC]
    public void ResetScoreRPC()
    {
        _aTeamScore = 0;
        _bTeamScore = 0;
        InGameUI.Instance?.UpdateScore(TeamType.A, _aTeamScore);
        InGameUI.Instance?.UpdateScore(TeamType.B, _bTeamScore);
    }

    [PunRPC]
    public void ResetWinCountRPC()
    {
        _aTeamWinCount = 0;
        _bTeamWinCount = 0;
        if(InGameUI.Instance != null)
        {
            InGameUI.Instance.UpdateWinCount(TeamType.A, _aTeamWinCount);
            InGameUI.Instance.UpdateWinCount(TeamType.B, _bTeamWinCount);
        }
    }

    [PunRPC]
    public void SetInitRPC()
    {
        _aTeamScore = 0;
        _bTeamScore = 0;
        _aTeamWinCount = 0;
        _bTeamWinCount = 0;
        InGameUI.Instance?.UpdateScore(TeamType.A, _aTeamScore);
        InGameUI.Instance?.UpdateScore(TeamType.B, _bTeamScore);
    }

    #endregion


    /// <summary>
    /// ���� ���� �� ����� ǥ�õ� ���� ȣ��� �̺�Ʈ
    /// </summary>
    private void OnRoundResultShown() //TODO : ���� �ʿ�
    {
        //Debug.Log("Event OnRoundResultShown Call");
        if (HasReachedTargetWinCount())
        {
            //Debug.Log(" ���� ���� ȣ��");
            GameManager.Instance?.ChangeState(new GameOverState(GameManager.Instance));
        }
        else
        {
            //Debug.Log("���� ���� ����");
            GameManager.Instance?.ChangeState(new WaitingState(GameManager.Instance));
        }
    }
}
