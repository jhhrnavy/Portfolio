using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 인게임(게임플레이) 콘텐츠 관리, 및 동기화
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
        // 라운드 재입장시 모든 플레이어 리스폰
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
        GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, Vector3.zero, Quaternion.identity); // 객체 생성
        if (player == null) return;
        var playerTeam = player.GetComponent<PlayerTeam>();
        playerTeam.photonView.RPC("SetTeam", RpcTarget.All); //player의 team 정보 세팅
        var team = _teamList[(int)playerTeam.Type]; // player가 속한 팀
        //Debug.Log(playerTeam);

        int spawnIndex = team.Players.Count; // 들어온 순서대로 위치 지정

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
        photonView.RPC("CloseRoundResultPanelRPC", RpcTarget.All); // 라운드 결과창 닫기
        _timer.ResetTimer(); // 라운드 제한시간 초기화
        _scoreManager.ResetScore();
    }

    public void GameOver()
    {
        photonView.RPC("GameOverRPC", RpcTarget.All);
    }

    /// <summary>
    /// 대기방으로 이동
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
                    // 로컬 플레이어의 팀이 승리 팀인지 체크
                    result = team.Type == winTeam ? GameResult.Win : GameResult.Lose;
                    _inGameUI.ShowGameResultPanel(result); // 결과 표시
                    return;
                }
            }
        }
    }

    [PunRPC]
    public void ReturnToLobbyRPC()
    {
        PhotonNetwork.LoadLevel(0); //WaitingRoom이 있는 대기 씬으로 이동
    }
    #endregion
}
