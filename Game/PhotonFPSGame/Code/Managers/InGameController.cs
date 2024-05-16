using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ΰ���(�����÷���) ������ ����, �� ����ȭ
/// </summary>
public class InGameController : MonoBehaviourPun
{
    // Spawn
    [SerializeField] private GameObject _playerPrefab;

    [SerializeField]
    private List<Team> _teamList = 
        new List<Team> { new Team(TeamType.A), new Team(TeamType.B) };

    [SerializeField] private CountDownTimer _countdownTimer;
    [SerializeField] private Timer _timer;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private InGameUI _inGameUI;

    #region Properties

    public List<Team> TeamList { get => _teamList; }
    public Timer Timer { get => _timer; }
    public CountDownTimer CountdownTimer { get => _countdownTimer; }
    public ScoreManager ScoreManager { get => _scoreManager; }
    public InGameUI UI { get => _inGameUI; }

    #endregion

    #region Spawn Management Methods

    public void RespawnPlayer(PlayerController player)
    {
        player.MoveToSpawnPoint();
    }

    public void SpawnPlayer()
    {
        if(photonView.IsMine)
            photonView.RPC("SpawnPlayerRPC",RpcTarget.All);
    }

    public void RespawnAllPlayer()
    {
        // ���� ������� ��� �÷��̾� ������
        foreach (var team in _teamList)
        {
            for (int i = 0; i < team.Players.Count; i++)
            {
                team.Players[i].MoveToSpawnPoint();
                //Debug.Log($"{team.Players[i].gameObject.name} : respawn");
            }
        }
    }

    [PunRPC]
    public void SpawnPlayerRPC()
    {
        GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, Vector3.zero, Quaternion.identity); // ��ü ����
        if (player == null) return;
        var playerTeam = player.GetComponent<PlayerTeam>();
        playerTeam.photonView.RPC("SetTeam", RpcTarget.All); //player�� team ���� ����
        var team = _teamList[(int)playerTeam.Type]; // player�� ���� ��
        //Debug.Log(playerTeam);

        int spawnIndex = team.Players.Count; // ���� ������� ��ġ ����

        if (player.GetComponent<PlayerController>().photonView.IsMine)
        {
            player.GetComponent<PlayerController>().photonView.RPC("SetSpawnPointRPC",
                RpcTarget.All, team.SpawnPoints[spawnIndex].position, team.SpawnPoints[spawnIndex].rotation);
            player.GetComponent<PlayerController>().MoveToSpawnPoint();
        }

        player.GetComponent<PhotonView>().RPC("AddPlayerToPlayerList", RpcTarget.AllBuffered, player.GetComponent<PhotonView>().ViewID);
    }

    #endregion

    #region Sync Function

    public void ResetRound()
    {
        photonView.RPC("CloseRoundResultPanelRPC", RpcTarget.All); // ���� ���â �ݱ�
        _timer.ResetTimer(); // ���� ���ѽð� �ʱ�ȭ
        _scoreManager.ResetScore();
    }

    public void GameOver()
    {
        photonView.RPC("GameOverRPC", RpcTarget.All);
    }

    /// <summary>
    /// �������� �̵�
    /// </summary>
    public void ReturnToLobby()
    {
        photonView.RPC("ReturnToLobbyRPC", RpcTarget.All);
    }

    #endregion

    #region RPC Methods

    [PunRPC]
    public void CloseRoundResultPanelRPC()
    {
        _inGameUI.CloseRoundResultPanel();
    }

    [PunRPC]
    public void GameOverRPC()
    {
        TeamType winTeam = ScoreManager.GetWinnerTeam();
        GameResult result = GameResult.Lose;

        foreach (var team in TeamList)
        {
            foreach (var player in team.Players)
            {
                if (player.photonView.IsMine)
                {
                    // ���� �÷��̾��� ���� �¸� ������ üũ
                    result = team.Type == winTeam ? GameResult.Win : GameResult.Lose;
                    _inGameUI.ShowGameResultPanel(result); // ��� ǥ��
                    return;
                }
            }
        }
    }

    [PunRPC]
    public void ReturnToLobbyRPC()
    {
        PhotonNetwork.LoadLevel(0); //WaitingRoom�� �ִ� ��� ������ �̵�
    }
    #endregion
}
